using System.Net.Http.Headers;
using System.Text.Json;
using EtsyVault.Core.Models;
using EtsyVault.Core.Throttle;

namespace EtsyVault.Core.Client;

/// <summary>
/// Production Etsy API Client built against Etsy OpenAPI 3.0 specs.
/// Uses SafeRateLimiter (~2.5 QPS) to strictly prevent HTTP 429 rate limit errors.
/// </summary>
public sealed class EtsyApiClient : IEtsyClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly SafeRateLimiter _rateLimiter;
    private readonly string _keystring;
    private readonly string _accessToken;

    private const string BaseApiUrl = "https://api.etsy.com/v3/application/";

    public EtsyApiClient(string keystring, string accessToken, SafeRateLimiter? rateLimiter = null)
    {
        _keystring = keystring ?? throw new ArgumentNullException(nameof(keystring));
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        _rateLimiter = rateLimiter ?? new SafeRateLimiter(2.5); // Safe 2.5 QPS throttle

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseApiUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _keystring);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
    }

    public async Task<EtsyShopDto?> GetShopMeAsync(CancellationToken cancellationToken = default)
    {
        await _rateLimiter.WaitForSlotAsync(cancellationToken);

        using var response = await _httpClient.GetAsync("users/me/shops", cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var paginated = JsonSerializer.Deserialize<EtsyPaginatedResponse<EtsyShopDto>>(json);
        return paginated?.Results.FirstOrDefault();
    }

    public async Task<IReadOnlyList<EtsyListingDto>> GetAllListingsAsync(
        long shopId, 
        IProgress<string>? logProgress = null, 
        CancellationToken cancellationToken = default)
    {
        var allListings = new List<EtsyListingDto>();
        int limit = 100; // Max allowed by Etsy API v3
        int offset = 0;
        int totalCount = int.MaxValue;

        while (offset < totalCount)
        {
            await _rateLimiter.WaitForSlotAsync(cancellationToken);

            var endpoint = $"shops/{shopId}/listings?limit={limit}&offset={offset}&state=active";
            logProgress?.Report($"Fetching listings batch: offset {offset}...");

            using var response = await _httpClient.GetAsync(endpoint, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var page = JsonSerializer.Deserialize<EtsyPaginatedResponse<EtsyListingDto>>(json);

            if (page == null || page.Results.Count == 0)
                break;

            totalCount = page.Count;
            allListings.AddRange(page.Results);
            offset += page.Results.Count;

            logProgress?.Report($"Retrieved {allListings.Count}/{totalCount} listings.");
        }

        return allListings;
    }

    public async Task<IReadOnlyList<EtsyListingImageDto>> GetListingImagesAsync(long listingId, CancellationToken cancellationToken = default)
    {
        await _rateLimiter.WaitForSlotAsync(cancellationToken);

        var endpoint = $"listings/{listingId}/images";
        using var response = await _httpClient.GetAsync(endpoint, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return Array.Empty<EtsyListingImageDto>();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var page = JsonSerializer.Deserialize<EtsyPaginatedResponse<EtsyListingImageDto>>(json);
        return page?.Results ?? (IReadOnlyList<EtsyListingImageDto>)Array.Empty<EtsyListingImageDto>();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        _rateLimiter.Dispose();
    }
}
