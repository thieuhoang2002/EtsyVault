using System.Security.Cryptography;
using System.Text;

namespace EtsyVault.Core.Auth;

/// <summary>
/// Implements PKCE (Proof Key for Code Exchange - RFC 7636) for secure OAuth 2.0 authentication.
/// Zero external dependencies, highly optimized.
/// </summary>
public static class PkceHelper
{
    /// <summary>
    /// Generates a cryptographically secure random Code Verifier (43-128 chars).
    /// </summary>
    public static string GenerateCodeVerifier(int length = 64)
    {
        if (length < 43 || length > 128)
            throw new ArgumentOutOfRangeException(nameof(length), "PKCE code verifier length must be between 43 and 128 characters.");

        const string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";
        var bytes = new byte[length];
        RandomNumberGenerator.Fill(bytes);

        var sb = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            sb.Append(unreservedChars[bytes[i] % unreservedChars.Length]);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Generates the SHA256 Code Challenge from the Code Verifier using Base64-URL encoding.
    /// </summary>
    public static string GenerateCodeChallenge(string codeVerifier)
    {
        var bytes = Encoding.ASCII.GetBytes(codeVerifier);
        var hash = SHA256.HashData(bytes);

        // Convert to Base64Url (RFC 7636 Section 4.2)
        return Convert.ToBase64String(hash)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
