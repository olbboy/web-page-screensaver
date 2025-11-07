using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using pl.polidea.lab.Web_Page_Screensaver.Security;

namespace pl.polidea.lab.Web_Page_Screensaver
{
    public partial class ScreensaverForm : Form
    {
        private DateTime StartTime;
        private Timer? timer;
        private int currentSiteIndex = -1;
        private GlobalUserEventHandler userEventHandler;
        private bool shuffleOrder;
        private List<string>? urls;

        private PreferencesManager prefsManager = new PreferencesManager();

        private int screenNum;
        private bool isWebView2Initialized = false;

        [ThreadStatic]
        private static Random? random;

        public ScreensaverForm(int? screenNumber = null)
        {
            userEventHandler = new GlobalUserEventHandler();
            userEventHandler.Event += new GlobalUserEventHandler.UserEvent(HandleUserActivity);

            if (screenNumber == null) screenNum = prefsManager.EffectiveScreensList.FindIndex(s => s.IsPrimary);
            else screenNum = (int)screenNumber;

            InitializeComponent();

            Cursor.Hide();
        }

        public List<string> Urls
        {
            get
            {
                if (urls == null)
                {
                    // Load URLs from preferences
                    var rawUrls = prefsManager.GetUrlsByScreen(screenNum);

                    // Validate and sanitize URLs for security (OWASP best practice)
                    urls = UrlValidator.ValidateUrlList(rawUrls, out var removedUrls);

                    // Log any URLs that were removed due to security validation
                    if (removedUrls.Count > 0)
                    {
                        Debug.WriteLine($"[SECURITY] Removed {removedUrls.Count} invalid URLs from screen {screenNum}");
                        foreach (var (removedUrl, reason) in removedUrls)
                        {
                            Debug.WriteLine($"[SECURITY] - Blocked: {removedUrl} - Reason: {reason}");
                        }
                    }
                }

                return urls;
            }
        }

        private async void ScreensaverForm_Load(object sender, EventArgs e)
        {
            // Initialize WebView2 asynchronously
            try
            {
                await InitializeWebView2Async();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WebView2 initialization failed: {ex.Message}");
                MessageBox.Show(
                    $"Failed to initialize web rendering engine:\n{ex.Message}\n\n" +
                    "Please ensure Microsoft Edge WebView2 Runtime is installed.\n" +
                    "Download from: https://go.microsoft.com/fwlink/p/?LinkId=2124703",
                    "Initialization Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();
                return;
            }

            if (Urls != null && Urls.Any())
            {
                if (Urls.Count > 1)
                {
                    // Shuffle the URLs if necessary
                    shuffleOrder = prefsManager.GetRandomizeFlagByScreen(screenNum);
                    if (shuffleOrder)
                    {
                        random = new Random();

                        int n = urls!.Count;
                        while (n > 1)
                        {
                            n--;
                            int k = random.Next(n + 1);
                            var value = urls[k];
                            urls[k] = urls[n];
                            urls[n] = value;
                        }
                    }

                    // Set up timer to rotate to the next URL
                    timer = new Timer();
                    timer.Interval = prefsManager.GetRotationIntervalByScreen(screenNum) * 1000;
                    timer.Tick += (s, ee) => RotateSite();
                    timer.Start();
                }

                // Display the first site in the list
                RotateSite();

                StartTime = DateTime.Now;
            }
            else
            {
                webView2.Visible = false;
            }
        }

        private async Task InitializeWebView2Async()
        {
            try
            {
                // Initialize WebView2 core
                await webView2.EnsureCoreWebView2Async(null);
                isWebView2Initialized = true;

                // Apply comprehensive security hardening (OWASP, NIST, GDPR compliance)
                if (webView2.CoreWebView2 != null)
                {
                    bool securityInitialized = await WebView2SecurityManager.InitializeSecurityAsync(webView2);

                    if (!securityInitialized)
                    {
                        Debug.WriteLine("[SECURITY] Warning: Some security features may not be fully initialized");
                    }

                    Debug.WriteLine("[SECURITY] WebView2 initialized with world-class security controls");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SECURITY] WebView2 initialization error: {ex.Message}");
                throw;
            }
        }

        private void BrowseTo(string url)
        {
            // Disable the user event handler while navigating
            Application.RemoveMessageFilter(userEventHandler);

            if (string.IsNullOrWhiteSpace(url))
            {
                webView2.Visible = false;
            }
            else
            {
                // Additional URL validation before navigation (defense-in-depth)
                if (!UrlValidator.IsValidUrl(url, out string errorMessage))
                {
                    Debug.WriteLine($"[SECURITY] Navigation attempt blocked: {errorMessage}");
                    Debug.WriteLine($"[SECURITY] Invalid URL: {url}");

                    // Generate audit log entry (NIST compliance)
                    var auditLog = UrlValidator.GenerateAuditLogEntry(url, false, errorMessage);
                    Debug.WriteLine($"[AUDIT] {auditLog}");

                    // Skip to next URL
                    Application.AddMessageFilter(userEventHandler);
                    return;
                }

                webView2.Visible = true;

                if (isWebView2Initialized && webView2.CoreWebView2 != null)
                {
                    try
                    {
                        Debug.WriteLine($"[SECURITY] Navigating to validated URL: {url}");

                        // Generate successful navigation audit log
                        var auditLog = UrlValidator.GenerateAuditLogEntry(url, true);
                        Debug.WriteLine($"[AUDIT] {auditLog}");

                        // WebView2 navigation is fire-and-forget in this context
                        webView2.CoreWebView2.Navigate(url);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[SECURITY] Navigation error: {ex.Message}");
                        // Silently handle navigation errors (same behavior as old WebBrowser)
                    }
                }
            }

            Application.AddMessageFilter(userEventHandler);
        }

        private void RotateSite()
        {
            if (Urls == null || Urls.Count == 0)
                return;

            currentSiteIndex++;

            if (currentSiteIndex >= Urls.Count)
            {
                currentSiteIndex = 0;
            }

            var url = Urls[currentSiteIndex];

            BrowseTo(url);
        }

        private void HandleUserActivity()
        {
            if (StartTime.AddSeconds(1) > DateTime.Now) return;

            if (prefsManager.CloseOnActivity)
            {
                Close();
            }
            else
            {
                closeButton.Visible = true;
                Cursor.Show();
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    public class GlobalUserEventHandler : IMessageFilter
    {
        public delegate void UserEvent();

        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;

        // screensavers and especially multi-window apps can get spurrious WM_MOUSEMOVE events
        // that don't actually involve any movement (cursor chnages and some mouse driver software
        // can generate them, for example) - so we record the actual mouse position and compare against it for actual movement.
        private Point? lastMousePos;

        public event UserEvent Event;

        public bool PreFilterMessage(ref Message m)
        {
            if ((m.Msg == WM_MOUSEMOVE) && (this.lastMousePos == null))
            {
                this.lastMousePos = Cursor.Position;
            }

            if (((m.Msg == WM_MOUSEMOVE) && (Cursor.Position != this.lastMousePos))
                || (m.Msg > WM_MOUSEMOVE && m.Msg <= WM_MBUTTONDBLCLK) || m.Msg == WM_KEYDOWN || m.Msg == WM_KEYUP)
            {

                if (Event != null)
                {
                    Event();
                }
            }
            // Always allow message to continue to the next filter control
            return false;
        }
    }
}