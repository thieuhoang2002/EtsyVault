using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EtsyVault.Core.Auth;

public sealed record EtsyTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;

    [JsonPropertyName("token_type")]
    public string TokenType { get; init; } = "Bearer";

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; } = string.Empty;

    [JsonPropertyName("error")]
    public string? Error { get; init; }

    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; init; }
}

/// <summary>
/// Handles OAuth 2.0 PKCE authentication against Etsy Open API v3.
/// Starts a local HttpListener, opens system browser, captures authorization code,
/// and exchanges it for Access/Refresh Tokens.
/// </summary>
public sealed class OAuthPkceAuthenticator
{
    private const string RedirectUri = "http://localhost:54321/callback/";
    private const string TokenEndpoint = "https://api.etsy.com/v3/public/oauth/token";
    private const string ConnectEndpoint = "https://www.etsy.com/oauth/connect";

    private readonly HttpClient _httpClient;

    public OAuthPkceAuthenticator(HttpClient? httpClient = null)
    {
        _httpClient = httpClient ?? new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
    }

    /// <summary>
    /// Authenticates the user via system browser with Etsy OAuth 2.0 PKCE.
    /// </summary>
    public async Task<EtsyTokenResponse> AuthenticateAsync(
        string keystring, 
        string? sharedSecret = null, 
        IProgress<string>? logProgress = null, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(keystring))
            throw new ArgumentException("Etsy Keystring (Client ID) is required.", nameof(keystring));

        // Step 1: Generate PKCE credentials
        var codeVerifier = PkceHelper.GenerateCodeVerifier(64);
        var codeChallenge = PkceHelper.GenerateCodeChallenge(codeVerifier);
        var state = Guid.NewGuid().ToString("N");

        // Scopes needed for read-only backup
        var scopes = "shops_r%20listings_r";

        var authorizationUrl = $"{ConnectEndpoint}?response_type=code" +
                               $"&client_id={Uri.EscapeDataString(keystring)}" +
                               $"&redirect_uri={Uri.EscapeDataString(RedirectUri)}" +
                               $"&scope={scopes}" +
                               $"&state={state}" +
                               $"&code_challenge={codeChallenge}" +
                               $"&code_challenge_method=S256";

        logProgress?.Report("Starting local authorization listener on port 54321...");

        using var listener = new HttpListener();
        listener.Prefixes.Add(RedirectUri);
        listener.Start();

        logProgress?.Report("Opening default web browser for Etsy authorization...");
        OpenBrowser(authorizationUrl);

        // Step 2: Await user authorization callback from browser
        string authCode;
        try
        {
            var context = await listener.GetContextAsync().WaitAsync(cancellationToken);
            var request = context.Request;
            var query = request.QueryString;

            var returnedState = query["state"];
            var returnedCode = query["code"];
            var returnedError = query["error"];

            // Send friendly HTML response to browser tab
            var response = context.Response;
            response.ContentType = "text/html; charset=utf-8";

            string htmlResponse;
            if (!string.IsNullOrEmpty(returnedError))
            {
                htmlResponse = $"""
                    <html>
                    <body style="font-family: system-ui, sans-serif; background: #18181b; color: #ef4444; text-align: center; padding: 60px;">
                        <h2>❌ Authorization Denied</h2>
                        <p>{returnedError}: {query["error_description"]}</p>
                        <p>You can close this tab and return to EtsyVault.</p>
                    </body>
                    </html>
                    """;
            }
            else
            {
                htmlResponse = """
                    <html>
                    <body style="font-family: system-ui, sans-serif; background: #18181b; color: #22c55e; text-align: center; padding: 60px;">
                        <h2>🎉 Authorization Successful!</h2>
                        <p style="color: #a1a1aa;">EtsyVault has safely captured your authorization code.</p>
                        <p style="color: #71717a;">You may safely close this browser tab and return to the application.</p>
                    </body>
                    </html>
                    """;
            }

            var buffer = Encoding.UTF8.GetBytes(htmlResponse);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, cancellationToken);
            response.OutputStream.Close();

            if (!string.IsNullOrEmpty(returnedError))
                throw new InvalidOperationException($"Etsy returned error: {returnedError} - {query["error_description"]}");

            if (returnedState != state)
                throw new InvalidOperationException("OAuth state mismatch. Potential CSRF security threat detected.");

            if (string.IsNullOrEmpty(returnedCode))
                throw new InvalidOperationException("No authorization code received from Etsy.");

            authCode = returnedCode;
        }
        finally
        {
            listener.Stop();
        }

        logProgress?.Report("Authorization code captured! Exchanging for Access Token...");

        // Step 3: Exchange Authorization Code + Code Verifier for Access Token
        var tokenParams = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", keystring },
            { "redirect_uri", RedirectUri },
            { "code", authCode },
            { "code_verifier", codeVerifier }
        };

        if (!string.IsNullOrWhiteSpace(sharedSecret))
        {
            tokenParams["client_secret"] = sharedSecret;
        }

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint)
        {
            Content = new FormUrlEncodedContent(tokenParams)
        };

        using var tokenResponse = await _httpClient.SendAsync(requestMessage, cancellationToken);
        var jsonContent = await tokenResponse.Content.ReadAsStringAsync(cancellationToken);

        if (!tokenResponse.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Token exchange failed ({tokenResponse.StatusCode}): {jsonContent}");
        }

        var tokenData = JsonSerializer.Deserialize<EtsyTokenResponse>(jsonContent);
        if (tokenData == null || string.IsNullOrEmpty(tokenData.AccessToken))
        {
            throw new InvalidOperationException("Failed to parse token response from Etsy API.");
        }

        logProgress?.Report("Token exchange successful! Connected securely to Etsy.");
        return tokenData;
    }

    private static void OpenBrowser(string url)
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
