using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EtsyVault.Core.Client;
using EtsyVault.Core.Models;
using EtsyVault.Core.Services;

namespace EtsyVault.UI.ViewModels;

public enum AppState
{
    ReadyToConnect,
    BackingUp,
    Completed,
    Error
}

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _keystring = string.Empty;

    [ObservableProperty]
    private string _sharedSecret = string.Empty;

    [ObservableProperty]
    private bool _showAdvancedSettings = false;

    [ObservableProperty]
    private bool _isDemoMode = true;

    [ObservableProperty]
    private bool _isAuthenticated = false;

    [ObservableProperty]
    private string _connectedShopName = string.Empty;

    [ObservableProperty]
    private AppState _currentState = AppState.ReadyToConnect;

    [ObservableProperty]
    private string _statusTitle = "Ready to Safeguard Your Etsy Store";

    [ObservableProperty]
    private string _statusSubtitle = "Archive your active listings, original high-res photos & variation data locally.";

    [ObservableProperty]
    private double _progressValue = 0;

    [ObservableProperty]
    private string _progressText = string.Empty;

    [ObservableProperty]
    private int _savedListingsCount = 0;

    [ObservableProperty]
    private int _savedImagesCount = 0;

    [ObservableProperty]
    private string _lastBackupFolderPath = string.Empty;

    public ObservableCollection<string> LogEntries { get; } = new();

    private CancellationTokenSource? _cts;

    [RelayCommand]
    private async Task StartBackupAsync()
    {
        CurrentState = AppState.BackingUp;
        StatusTitle = "Backup in Progress...";
        StatusSubtitle = "Fetching listings and downloading high-resolution assets from Etsy CDN...";
        ProgressValue = 0;
        LogEntries.Clear();
        _cts = new CancellationTokenSource();

        try
        {
            AddLog("Initializing Etsy Vault backup pipeline...");

            // Select client: Mock/Demo mode or real Etsy API
            IEtsyClient client;
            if (IsDemoMode)
            {
                AddLog("[DEMO MODE] Using simulated high-fidelity Etsy store data (25 products)...");
                client = new MockEtsyClient(25);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Keystring))
                {
                    throw new InvalidOperationException("Please enter your Etsy Keystring (Client ID).");
                }
                
                AddLog("Initiating OAuth 2.0 PKCE authentication...");
                var progressReporterAuth = new Progress<string>(AddLog);
                var authenticator = new EtsyVault.Core.Auth.OAuthPkceAuthenticator();
                var tokenResult = await authenticator.AuthenticateAsync(Keystring, SharedSecret, progressReporterAuth, _cts.Token);

                IsAuthenticated = true;
                AddLog("Access token acquired successfully. Connecting to Etsy API v3...");
                client = new EtsyApiClient(Keystring, tokenResult.AccessToken);
            }

            // Step 1: Get Shop info
            var shop = await client.GetShopMeAsync(_cts.Token);
            var shopName = shop?.ShopName ?? "EtsyShop";
            AddLog($"Connected to shop: {shopName} (ID: {shop?.ShopId})");

            // Step 2: Create backup folder on Desktop
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
            var backupDir = Path.Combine(desktopPath, $"EtsyBackup_{shopName}_{timestamp}");
            var imagesDir = Path.Combine(backupDir, "Images");
            Directory.CreateDirectory(imagesDir);
            LastBackupFolderPath = backupDir;

            // Step 3: Fetch listings
            var progressReporter = new Progress<string>(AddLog);
            var listings = await client.GetAllListingsAsync(shop?.ShopId ?? 0, progressReporter, _cts.Token);
            SavedListingsCount = listings.Count;
            AddLog($"Successfully fetched {listings.Count} listings with descriptions, tags, and pricing.");

            // Step 4: Gather image URLs
            AddLog("Resolving high-resolution photo URLs for all listings...");
            var imageJobs = new List<ImageJob>();
            foreach (var listing in listings)
            {
                var images = await client.GetListingImagesAsync(listing.ListingId, _cts.Token);
                listing.Images = images.ToList();

                var listingFolder = Path.Combine(imagesDir, $"[{listing.ListingId}]_{SanitizeFileName(listing.Title)}");
                int rank = 1;
                foreach (var img in images)
                {
                    if (!string.IsNullOrEmpty(img.UrlFullxfull))
                    {
                        var fileName = $"{rank:D2}_fullxfull_{img.ListingImageId}.jpg";
                        imageJobs.Add(new ImageJob(img.UrlFullxfull, Path.Combine(listingFolder, fileName), listing.ListingId));
                        rank++;
                    }
                }
            }
            SavedImagesCount = imageJobs.Count;
            AddLog($"Found {imageJobs.Count} original high-res product photos to archive.");

            // Step 5: Download photos in parallel via Channel pipeline
            var imageQueue = new ImageDownloadQueue(maxConcurrentDownloads: 4);
            var imgProgress = new Progress<(int completed, int total, string currentFileName)>(p =>
            {
                ProgressValue = (double)p.completed / p.total * 100;
                ProgressText = $"Downloading photos: {p.completed}/{p.total} ({p.currentFileName})";
            });

            await imageQueue.DownloadAllAsync(imageJobs, imgProgress, _cts.Token);
            AddLog("All product photos downloaded successfully!");

            // Step 6: Export Master Excel & Shopify CSV
            AddLog("Generating Products_Master.xlsx spreadsheet...");
            await BackupExporter.ExportMasterExcelAsync(Path.Combine(backupDir, "Products_Master.xlsx"), listings);

            AddLog("Generating Shopify_Ready_Import.csv migration file...");
            await BackupExporter.ExportShopifyCsvAsync(Path.Combine(backupDir, "Shopify_Ready_Import.csv"), listings);

            AddLog("Writing full Raw JSON snapshot (Backup_Snapshot.json)...");
            await BackupExporter.ExportRawJsonAsync(Path.Combine(backupDir, "Backup_Snapshot.json"), listings);

            AddLog($"🎉 Backup successfully completed at: {backupDir}");
            CurrentState = AppState.Completed;
            StatusTitle = "🎉 Backup Completed Successfully!";
            StatusSubtitle = $"Saved {SavedListingsCount} listings and {SavedImagesCount} original photos to your drive.";
        }
        catch (Exception ex)
        {
            CurrentState = AppState.Error;
            StatusTitle = "Backup Failed";
            StatusSubtitle = ex.Message;
            AddLog($"[ERROR] {ex.Message}");
        }
    }

    [RelayCommand]
    private void OpenBackupFolder()
    {
        if (Directory.Exists(LastBackupFolderPath))
        {
            OpenUrl(LastBackupFolderPath);
        }
    }

    [RelayCommand]
    private void OpenBuyMeACoffee()
    {
        OpenUrl("https://buymeacoffee.com/thhoang0903");
    }

    [RelayCommand]
    private void OpenApiGuide()
    {
        OpenUrl("https://github.com/thieuhoang2002/EtsyVault/blob/main/docs/how-to-get-etsy-api-key.md");
    }

    [RelayCommand]
    private void OpenWebSuite()
    {
        OpenUrl("https://etsy.thhoang.io.vn");
    }

    [RelayCommand]
    private void ResetToReady()
    {
        CurrentState = AppState.ReadyToConnect;
        StatusTitle = "Ready to Safeguard Your Etsy Store";
        StatusSubtitle = "Archive your active listings, original high-res photos & variation data locally.";
        ProgressValue = 0;
        ProgressText = string.Empty;
    }

    private void AddLog(string message)
    {
        LogEntries.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
    }

    private static string SanitizeFileName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = new string(name.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
        return sanitized.Length > 40 ? sanitized[..40].Trim() : sanitized.Trim();
    }

    private static void OpenUrl(string url)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start \"\" \"{url}\"") { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                Process.Start("xdg-open", url);
            }
        }
        catch { }
    }
}
