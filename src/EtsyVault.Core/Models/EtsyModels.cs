using System.Text.Json.Serialization;

namespace EtsyVault.Core.Models;

/// <summary>
/// Basic Etsy Shop information DTO mapped directly to Etsy OpenAPI 3.0 specs.
/// </summary>
public sealed record EtsyShopDto
{
    [JsonPropertyName("shop_id")]
    public long ShopId { get; init; }

    [JsonPropertyName("shop_name")]
    public string ShopName { get; init; } = string.Empty;

    [JsonPropertyName("user_id")]
    public long UserId { get; init; }

    [JsonPropertyName("listing_active_count")]
    public int ListingActiveCount { get; init; }

    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;

    [JsonPropertyName("currency_code")]
    public string CurrencyCode { get; init; } = "USD";
}

/// <summary>
/// Etsy Listing representation with fields necessary for backup and Shopify/Excel migration.
/// </summary>
public sealed record EtsyListingDto
{
    [JsonPropertyName("listing_id")]
    public long ListingId { get; init; }

    [JsonPropertyName("user_id")]
    public long UserId { get; init; }

    [JsonPropertyName("shop_id")]
    public long ShopId { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; init; } = "active"; // active, draft, inactive, expired

    [JsonPropertyName("creation_timestamp")]
    public long CreationTimestamp { get; init; }

    [JsonPropertyName("created_timestamp")]
    public long CreatedTimestamp { get; init; }

    [JsonPropertyName("ending_timestamp")]
    public long EndingTimestamp { get; init; }

    [JsonPropertyName("original_creation_timestamp")]
    public long OriginalCreationTimestamp { get; init; }

    [JsonPropertyName("last_modified_timestamp")]
    public long LastModifiedTimestamp { get; init; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; init; }

    [JsonPropertyName("price")]
    public EtsyMoneyDto? Price { get; init; }

    [JsonPropertyName("tags")]
    public List<string> Tags { get; init; } = new();

    [JsonPropertyName("materials")]
    public List<string> Materials { get; init; } = new();

    [JsonPropertyName("sku")]
    public List<string> Sku { get; init; } = new();

    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;

    [JsonPropertyName("views")]
    public int Views { get; init; }

    [JsonPropertyName("num_favorers")]
    public int NumFavorers { get; init; }

    [JsonPropertyName("who_made")]
    public string? WhoMade { get; init; }

    [JsonPropertyName("when_made")]
    public string? WhenMade { get; init; }

    [JsonPropertyName("is_personalizable")]
    public bool IsPersonalizable { get; init; }

    [JsonPropertyName("personalization_required")]
    public bool PersonalizationRequired { get; init; }

    [JsonPropertyName("item_weight")]
    public decimal? ItemWeight { get; init; }

    [JsonPropertyName("item_weight_unit")]
    public string? ItemWeightUnit { get; init; }

    // Associated original high-res images
    [JsonIgnore]
    public List<EtsyListingImageDto> Images { get; set; } = new();
}

public sealed record EtsyMoneyDto
{
    [JsonPropertyName("amount")]
    public long Amount { get; init; }

    [JsonPropertyName("divisor")]
    public int Divisor { get; init; } = 100;

    [JsonPropertyName("currency_code")]
    public string CurrencyCode { get; init; } = "USD";

    [JsonIgnore]
    public decimal DecimalValue => Divisor > 0 ? (decimal)Amount / Divisor : Amount;
}

public sealed record EtsyListingImageDto
{
    [JsonPropertyName("listing_id")]
    public long ListingId { get; init; }

    [JsonPropertyName("listing_image_id")]
    public long ListingImageId { get; init; }

    [JsonPropertyName("hex_code")]
    public string? HexCode { get; init; }

    [JsonPropertyName("red")]
    public int? Red { get; init; }

    [JsonPropertyName("green")]
    public int? Green { get; init; }

    [JsonPropertyName("blue")]
    public int? Blue { get; init; }

    [JsonPropertyName("rank")]
    public int Rank { get; init; }

    [JsonPropertyName("url_75x75")]
    public string? Url75x75 { get; init; }

    [JsonPropertyName("url_170x135")]
    public string? Url170x135 { get; init; }

    [JsonPropertyName("url_570xN")]
    public string? Url570xN { get; init; }

    [JsonPropertyName("url_fullxfull")]
    public string UrlFullxfull { get; init; } = string.Empty; // Full original resolution
}

public sealed record EtsyPaginatedResponse<T>
{
    [JsonPropertyName("count")]
    public int Count { get; init; }

    [JsonPropertyName("results")]
    public List<T> Results { get; init; } = new();
}
