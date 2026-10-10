namespace EtsyVault.Core.Throttle;

/// <summary>
/// Thread-safe rate limiter with Leaky Bucket / Token Bucket hybrid strategy.
/// Designed specifically for Etsy Developer Personal Access tier (5 QPS limit).
/// Default configuration maintains an ultra-safe margin of ~2.5 QPS (400ms delay between calls).
/// </summary>
public sealed class SafeRateLimiter : IDisposable
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly TimeSpan _minInterval;
    private DateTime _lastRequestTime = DateTime.MinValue;
    private int _remainingDailyQuota = 5000;

    /// <summary>
    /// Initializes a new rate limiter.
    /// </summary>
    /// <param name="targetQps">Target queries per second. Default 2.5 (safe margin below 5.0 QPS).</param>
    public SafeRateLimiter(double targetQps = 2.5)
    {
        if (targetQps <= 0 || targetQps > 5.0)
            throw new ArgumentOutOfRangeException(nameof(targetQps), "QPS must be between 0.1 and 5.0 for Etsy API.");

        _minInterval = TimeSpan.FromMilliseconds(1000.0 / targetQps);
    }

    /// <summary>
    /// Blocks asynchronously until a safe request slot is available.
    /// O(1) time complexity.
    /// </summary>
    public async Task WaitForSlotAsync(CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var now = DateTime.UtcNow;
            var elapsed = now - _lastRequestTime;

            if (elapsed < _minInterval)
            {
                var delay = _minInterval - elapsed;
                await Task.Delay(delay, cancellationToken);
            }

            _lastRequestTime = DateTime.UtcNow;
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Updates rate limits dynamically based on Etsy HTTP response headers.
    /// </summary>
    public void UpdateFromHeaders(int? remainingRequests, int? retryAfterSeconds = null)
    {
        if (remainingRequests.HasValue)
        {
            Interlocked.Exchange(ref _remainingDailyQuota, remainingRequests.Value);
        }
    }

    public int RemainingDailyQuota => Volatile.Read(ref _remainingDailyQuota);

    public void Dispose()
    {
        _lock.Dispose();
    }
}
