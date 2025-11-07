using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace pl.polidea.lab.Web_Page_Screensaver
{
    using System.Collections.Generic;
    using System.Drawing;

    static class Program
    {
        public static readonly string KEY = "Software\\Web-Page-Screensaver";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // .NET 8 application configuration
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Note: Browser emulation registry setting is no longer needed with WebView2
            // WebView2 uses Chromium engine which doesn't require IE emulation mode
            // Keeping this commented for reference:
            // SetBrowserEmulationMode();

            // Parse command-line arguments for screensaver modes
            if (args.Length > 0)
            {
                string command = args[0].ToLower().Trim();

                // /p[:hwnd] - Preview mode (not supported)
                if (command.StartsWith("/p"))
                {
                    // Preview mode not supported - just exit
                    return;
                }

                // /c[:hwnd] - Configuration mode
                if (command.StartsWith("/c"))
                {
                    Application.Run(new PreferencesForm());
                    return;
                }

                // /s - Screensaver mode (or any other argument)
                // Fall through to run screensaver
            }

            // Run screensaver mode (default)
            RunScreensaver();
        }

        private static void RunScreensaver()
        {
            try
            {
                var formsList = new List<Form>();
                var prefsManager = new PreferencesManager();
                var screens = prefsManager.EffectiveScreensList;

                foreach (var screen in screens)
                {
                    var screensaverForm = new ScreensaverForm(screen.ScreenNum)
                    {
                        Location = new Point(screen.Bounds.Left, screen.Bounds.Top),
                        Size = new Size(screen.Bounds.Width, screen.Bounds.Height)
                    };

                    formsList.Add(screensaverForm);
                }

                if (formsList.Count > 0)
                {
                    Application.Run(new MultiFormContext(formsList));
                }
            }
            catch (Exception ex)
            {
                // Log critical startup errors
                System.Diagnostics.Debug.WriteLine($"Screensaver startup error: {ex.Message}");
                MessageBox.Show(
                    $"Failed to start screensaver:\n{ex.Message}",
                    "Screensaver Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Sets IE browser emulation mode (legacy - not needed for WebView2)
        /// </summary>
        [Obsolete("Not needed with WebView2, kept for reference only")]
        private static void SetBrowserEmulationMode()
        {
            try
            {
                var exeName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? "");
                if (!string.IsNullOrEmpty(exeName))
                {
                    Registry.SetValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                        exeName,
                        0x2AF8, // IE11 emulation mode
                        RegistryValueKind.DWord);
                }
            }
            catch (Exception ex)
            {
                // Silently ignore registry errors
                System.Diagnostics.Debug.WriteLine($"Registry access error: {ex.Message}");
            }
        }
    }

    public class MultiFormContext : ApplicationContext
    {
        public MultiFormContext(List<Form> forms)
        {
            foreach (var form in forms)
            {
                form.FormClosed += (s, args) =>
                {
                    //When we have closed any form, 
                    //end the program.
                        ExitThread();
                };

                form.Show();
            }
        }
    }
}
