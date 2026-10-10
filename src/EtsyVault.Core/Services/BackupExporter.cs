using System.Text.Json;
using EtsyVault.Core.Models;
using MiniExcelLibs;

namespace EtsyVault.Core.Services;

/// <summary>
/// Streams backup data into structured files (Excel Master sheet, Shopify CSV, and raw JSON).
/// Zero unnecessary in-memory object allocation using streaming serializers.
/// </summary>
public static class BackupExporter
{
    public static async Task ExportMasterExcelAsync(string filePath, IReadOnlyList<EtsyListingDto> listings)
    {
        var rows = listings.Select(l => new
        {
            ListingId = l.ListingId,
            Title = l.Title,
            Price = l.Price?.DecimalValue ?? 0,
            Currency = l.Price?.CurrencyCode ?? "USD",
            Quantity = l.Quantity,
            SKU = string.Join(", ", l.Sku),
            Tags = string.Join(", ", l.Tags),
            Materials = string.Join(", ", l.Materials),
            State = l.State,
            Views = l.Views,
            Favorites = l.NumFavorers,
            ImageCount = l.Images.Count,
            Url = l.Url,
            Description = l.Description
        });

        await MiniExcel.SaveAsAsync(filePath, rows, overwriteFile: true);
    }

    public static async Task ExportShopifyCsvAsync(string filePath, IReadOnlyList<EtsyListingDto> listings)
    {
        // Standard Shopify Products CSV schema
        var shopifyRows = listings.Select(l => new
        {
            Handle = SanitizeHandle(l.Title),
            Title = l.Title,
            Body_HTML = l.Description.Replace("\n", "<br/>"),
            Vendor = "Etsy Migrated",
            Type = "Handmade",
            Tags = string.Join(", ", l.Tags),
            Published = "true",
            Option1_Name = "Title",
            Option1_Value = "Default Title",
            Variant_SKU = l.Sku.FirstOrDefault() ?? $"ETSY-{l.ListingId}",
            Variant_Price = (l.Price?.DecimalValue ?? 0).ToString("0.00"),
            Variant_Inventory_Tracker = "shopify",
            Variant_Inventory_Qty = l.Quantity,
            Status = "active"
        });

        await MiniExcel.SaveAsAsync(filePath, shopifyRows, overwriteFile: true);
    }

    public static async Task ExportRawJsonAsync(string filePath, IReadOnlyList<EtsyListingDto> listings)
    {
        await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(stream, listings, new JsonSerializerOptions { WriteIndented = true });
    }

    private static string SanitizeHandle(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return "product";
        var clean = new string(title.ToLowerInvariant().Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-').ToArray());
        return string.Join("-", clean.Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }
}
