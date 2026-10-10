using EtsyVault.Core.Models;

namespace EtsyVault.Core.Client;

/// <summary>
/// Mock Etsy Client providing realistic test data without requiring real shop API credentials.
/// Enables offline testing, UI verification, and safe demo mode.
/// </summary>
public sealed class MockEtsyClient : IEtsyClient
{
    private readonly int _mockListingCount;

    public MockEtsyClient(int mockListingCount = 25)
    {
        _mockListingCount = mockListingCount;
    }

    public Task<EtsyShopDto?> GetShopMeAsync(CancellationToken cancellationToken = default)
    {
        var shop = new EtsyShopDto
        {
            ShopId = 12345678,
            ShopName = "ArtisanStudioCrafts",
            UserId = 87654321,
            ListingActiveCount = _mockListingCount,
            CurrencyCode = "USD",
            Url = "https://www.etsy.com/shop/ArtisanStudioCrafts"
        };
        return Task.FromResult<EtsyShopDto?>(shop);
    }

    public async Task<IReadOnlyList<EtsyListingDto>> GetAllListingsAsync(
        long shopId, 
        IProgress<string>? logProgress = null, 
        CancellationToken cancellationToken = default)
    {
        var listings = new List<EtsyListingDto>(_mockListingCount);

        string[] sampleTitles = 
        {
            "Handmade Leather Minimalist Wallet - Full Grain Veg Tan",
            "Botanical Wall Art Print Set of 3 - Minimalist Sage Green Boho Decor",
            "Custom Ceramic Coffee Mug - Handcrafted Speckled Stoneware",
            "Vintage Aesthetic Oversized Graphic T-Shirt - Organic Cotton",
            "Solid Oak Wood Desk Organizer - Wireless Charger Stand",
            "Hand-poured Soy Candle - French Vanilla & Amber Oakmoss",
            "Personalized Name Necklace - 18K Gold Plated Dainty Jewelry",
            "Linen Throw Pillow Cover - Neutral Farmhouse Cushion Case"
        };

        for (int i = 1; i <= _mockListingCount; i++)
        {
            await Task.Delay(20, cancellationToken); // Simulating fast local fetch
            var title = sampleTitles[(i - 1) % sampleTitles.Length] + $" #{i}";

            listings.Add(new EtsyListingDto
            {
                ListingId = 1000000000 + i,
                ShopId = shopId,
                Title = title,
                Description = $"Exquisitely crafted {title}. Designed and handmade locally with premium materials.\n\nDimensions: 10 x 15 cm\nCare: Wipe with damp cloth.",
                Quantity = 15,
                State = "active",
                Sku = new List<string> { $"SKU-ART-{i:D4}" },
                Tags = new List<string> { "gift for her", "handmade", "aesthetic", "minimalist", "home decor", "artisan" },
                Materials = new List<string> { "leather", "cotton", "ceramic", "oak wood" },
                Price = new EtsyMoneyDto { Amount = 2850 + (i * 200), Divisor = 100, CurrencyCode = "USD" },
                Views = 150 + (i * 35),
                NumFavorers = 42 + (i * 7),
                CreatedTimestamp = DateTimeOffset.UtcNow.AddDays(-i * 5).ToUnixTimeSeconds()
            });

            if (i % 5 == 0)
            {
                logProgress?.Report($"[Mock] Loaded {i}/{_mockListingCount} sample listings...");
            }
        }

        return listings;
    }

    public Task<IReadOnlyList<EtsyListingImageDto>> GetListingImagesAsync(long listingId, CancellationToken cancellationToken = default)
    {
        // 2 sample high-res public placeholder images per listing
        var images = new List<EtsyListingImageDto>
        {
            new()
            {
                ListingId = listingId,
                ListingImageId = listingId * 10 + 1,
                Rank = 1,
                UrlFullxfull = "https://images.unsplash.com/photo-1544816155-12df9643f363?auto=format&fit=crop&w=1200&q=80"
            },
            new()
            {
                ListingId = listingId,
                ListingImageId = listingId * 10 + 2,
                Rank = 2,
                UrlFullxfull = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=1200&q=80"
            }
        };

        return Task.FromResult<IReadOnlyList<EtsyListingImageDto>>(images);
    }
}
