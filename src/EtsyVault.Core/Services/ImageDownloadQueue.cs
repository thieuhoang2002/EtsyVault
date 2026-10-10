using System.Threading.Channels;

namespace EtsyVault.Core.Services;

public sealed record ImageJob(string Url, string DestinationFilePath, long ListingId);

/// <summary>
/// High-throughput, non-blocking image download pipeline.
/// Uses System.Threading.Channels (O(1) queue operations) to download CDN images concurrently
/// while protecting the local system with bounded worker concurrency.
/// </summary>
public sealed class ImageDownloadQueue
{
    private readonly HttpClient _httpClient;
    private readonly int _maxConcurrentDownloads;

    public ImageDownloadQueue(int maxConcurrentDownloads = 4)
    {
        _maxConcurrentDownloads = maxConcurrentDownloads;
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(60)
        };
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; EtsyVault/1.0)");
    }

    public async Task DownloadAllAsync(
        IReadOnlyList<ImageJob> jobs,
        IProgress<(int completed, int total, string currentFileName)>? progress = null,
        CancellationToken cancellationToken = default)
    {
        if (jobs.Count == 0) return;

        var channel = Channel.CreateBounded<ImageJob>(new BoundedChannelOptions(jobs.Count)
        {
            SingleReader = false,
            SingleWriter = true
        });

        int completedCount = 0;
        int totalCount = jobs.Count;

        // Producer: Write all jobs into Channel O(1)
        foreach (var job in jobs)
        {
            await channel.Writer.WriteAsync(job, cancellationToken);
        }
        channel.Writer.Complete();

        // Consumers: Worker tasks
        var workers = Enumerable.Range(0, _maxConcurrentDownloads).Select(async _ =>
        {
            await foreach (var job in channel.Reader.ReadAllAsync(cancellationToken))
            {
                try
                {
                    var dir = Path.GetDirectoryName(job.DestinationFilePath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    using var response = await _httpClient.GetAsync(job.Url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                        await using var fileStream = new FileStream(job.DestinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);
                        await stream.CopyToAsync(fileStream, cancellationToken);
                    }
                }
                catch
                {
                    // Fault-tolerant: Ignore individual image failure to allow full batch completion
                }
                finally
                {
                    var current = Interlocked.Increment(ref completedCount);
                    progress?.Report((current, totalCount, Path.GetFileName(job.DestinationFilePath)));
                }
            }
        });

        await Task.WhenAll(workers);
    }
}
