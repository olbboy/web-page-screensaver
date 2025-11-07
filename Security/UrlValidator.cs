using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace pl.polidea.lab.Web_Page_Screensaver.Security
{
    /// <summary>
    /// URL validation and sanitization to prevent injection attacks and ensure secure web content loading.
    /// Implements OWASP URL Validation best practices.
    /// </summary>
    public static class UrlValidator
    {
        // Allowed URL schemes (whitelist approach - OWASP recommendation)
        private static readonly HashSet<string> AllowedSchemes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "http",
            "https",
            "file"
        };

        // Blocked URL patterns (security threats)
        private static readonly List<string> BlockedPatterns = new List<string>
        {
            @"javascript:",      // JavaScript injection
            @"data:",           // Data URI injection
            @"vbscript:",       // VBScript injection
            @"about:",          // About pages
            @"ms-appx:",        // App packages
            @"ms-appdata:",     // App data
            @"<script",         // Inline scripts
            @"<iframe",         // Inline iframes
            @"<object",         // Inline objects
            @"<embed",          // Inline embeds
        };

        // Dangerous file extensions
        private static readonly HashSet<string> DangerousExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".exe", ".bat", ".cmd", ".com", ".pif", ".scr", ".vbs", ".js",
            ".jar", ".msi", ".hta", ".cpl", ".dll", ".ps1", ".psm1"
        };

        /// <summary>
        /// Validates if a URL is safe to load in the screensaver.
        /// Implements defense-in-depth security approach.
        /// </summary>
        /// <param name="url">URL to validate</param>
        /// <param name="errorMessage">Error message if validation fails</param>
        /// <returns>True if URL is safe, false otherwise</returns>
        public static bool IsValidUrl(string url, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Step 1: Null/Empty check
            if (string.IsNullOrWhiteSpace(url))
            {
                errorMessage = "URL cannot be empty";
                return false;
            }

            // Step 2: Trim and normalize
            url = url.Trim();

            // Step 3: Check for blocked patterns (injection prevention)
            foreach (var pattern in BlockedPatterns)
            {
                if (url.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    errorMessage = $"URL contains potentially dangerous content: {pattern}";
                    Debug.WriteLine($"[SECURITY] Blocked URL with pattern '{pattern}': {url}");
                    return false;
                }
            }

            // Step 4: Try to parse as URI
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
            {
                errorMessage = "Invalid URL format";
                return false;
            }

            // Step 5: Check allowed schemes (whitelist)
            if (!AllowedSchemes.Contains(uri.Scheme))
            {
                errorMessage = $"URL scheme '{uri.Scheme}' is not allowed. Allowed schemes: {string.Join(", ", AllowedSchemes)}";
                Debug.WriteLine($"[SECURITY] Blocked URL with disallowed scheme '{uri.Scheme}': {url}");
                return false;
            }

            // Step 6: Check for dangerous file extensions (for file:// URLs)
            if (uri.Scheme.Equals("file", StringComparison.OrdinalIgnoreCase))
            {
                string extension = Path.GetExtension(uri.LocalPath);
                if (DangerousExtensions.Contains(extension))
                {
                    errorMessage = $"File extension '{extension}' is not allowed for security reasons";
                    Debug.WriteLine($"[SECURITY] Blocked file URL with dangerous extension '{extension}': {url}");
                    return false;
                }
            }

            // Step 7: Additional validation for file URLs
            if (uri.Scheme.Equals("file", StringComparison.OrdinalIgnoreCase))
            {
                // Check if file path looks suspicious
                string path = uri.LocalPath;
                if (path.Contains("..") || path.Contains("%"))
                {
                    errorMessage = "File path contains potentially dangerous characters";
                    Debug.WriteLine($"[SECURITY] Blocked file URL with suspicious path: {url}");
                    return false;
                }
            }

            // Step 8: Length validation (prevent resource exhaustion)
            if (url.Length > 2048)
            {
                errorMessage = "URL is too long (maximum 2048 characters)";
                return false;
            }

            // URL passed all security checks
            return true;
        }

        /// <summary>
        /// Validates a list of URLs and returns only valid ones.
        /// Logs security violations for audit purposes.
        /// </summary>
        /// <param name="urls">List of URLs to validate</param>
        /// <param name="removedUrls">List of URLs that were removed due to security checks</param>
        /// <returns>List of valid URLs</returns>
        public static List<string> ValidateUrlList(List<string> urls, out List<(string Url, string Reason)> removedUrls)
        {
            var validUrls = new List<string>();
            removedUrls = new List<(string Url, string Reason)>();

            if (urls == null || urls.Count == 0)
                return validUrls;

            foreach (var url in urls)
            {
                if (IsValidUrl(url, out string errorMessage))
                {
                    validUrls.Add(url);
                }
                else
                {
                    removedUrls.Add((url, errorMessage));
                    Debug.WriteLine($"[SECURITY] Removed invalid URL: {url} - Reason: {errorMessage}");
                }
            }

            return validUrls;
        }

        /// <summary>
        /// Sanitizes a URL by removing potentially dangerous characters.
        /// Note: Validation should be performed first; sanitization is a secondary defense layer.
        /// </summary>
        /// <param name="url">URL to sanitize</param>
        /// <returns>Sanitized URL</returns>
        public static string SanitizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            // Remove control characters
            url = Regex.Replace(url, @"[\x00-\x1F\x7F]", string.Empty);

            // Trim whitespace
            url = url.Trim();

            return url;
        }

        /// <summary>
        /// Checks if a URL points to a local resource (localhost or local network).
        /// This can be useful for additional security policies.
        /// </summary>
        /// <param name="url">URL to check</param>
        /// <returns>True if URL is local, false otherwise</returns>
        public static bool IsLocalUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
                return false;

            if (uri.Scheme.Equals("file", StringComparison.OrdinalIgnoreCase))
                return true;

            var host = uri.Host.ToLowerInvariant();
            return host == "localhost" ||
                   host == "127.0.0.1" ||
                   host == "::1" ||
                   host.StartsWith("192.168.") ||
                   host.StartsWith("10.") ||
                   host.StartsWith("172.16.") ||
                   host.EndsWith(".local");
        }

        /// <summary>
        /// Generates a security audit log entry for URL validation.
        /// Implements NIST logging requirements for security events.
        /// </summary>
        /// <param name="url">URL being validated</param>
        /// <param name="isValid">Validation result</param>
        /// <param name="reason">Reason for failure if invalid</param>
        /// <returns>Formatted audit log entry</returns>
        public static string GenerateAuditLogEntry(string url, bool isValid, string reason = "")
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var status = isValid ? "ALLOWED" : "BLOCKED";
            var maskedUrl = MaskSensitiveData(url);

            return $"[{timestamp}] URL_VALIDATION {status} | URL: {maskedUrl} | Reason: {reason}";
        }

        /// <summary>
        /// Masks potentially sensitive data in URLs for logging (GDPR compliance).
        /// </summary>
        /// <param name="url">URL to mask</param>
        /// <returns>Masked URL</returns>
        private static string MaskSensitiveData(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return "[empty]";

            try
            {
                if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
                {
                    // Mask query parameters and fragments (may contain sensitive data)
                    var maskedUrl = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}";
                    if (!string.IsNullOrEmpty(uri.Query))
                        maskedUrl += "?[REDACTED]";
                    if (!string.IsNullOrEmpty(uri.Fragment))
                        maskedUrl += "#[REDACTED]";
                    return maskedUrl;
                }
            }
            catch
            {
                // If parsing fails, mask the entire URL
            }

            // For invalid URLs, show only first 50 chars
            return url.Length > 50 ? url.Substring(0, 50) + "...[TRUNCATED]" : url;
        }
    }
}
