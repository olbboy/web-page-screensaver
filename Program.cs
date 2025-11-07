using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Threading;

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
            // Setup global exception handlers for unhandled exceptions
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                // .NET 8 application configuration
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Log application start with version information
                LogStartupInfo(args);

                // Note: Browser emulation registry setting is no longer needed with WebView2
                // WebView2 uses Chromium engine which doesn't require IE emulation mode
                // Keeping this commented for reference:
                // SetBrowserEmulationMode();

                // Parse command-line arguments for screensaver modes
                if (args.Length > 0)
                {
                    string command = args[0].ToLower().Trim();

                    // /p[:hwnd] - Preview mode (not supported in v2.0)
                    if (command.StartsWith("/p"))
                    {
                        // Preview mode not supported - show message and exit
                        System.Diagnostics.Debug.WriteLine("[INFO] Preview mode requested but not supported");
                        // Windows will show blank preview window - this is expected behavior
                        return;
                    }

                    // /c[:hwnd] - Configuration mode
                    if (command.StartsWith("/c"))
                    {
                        RunConfigurationDialog();
                        return;
                    }

                    // /s - Screensaver mode (or any other argument)
                    // Fall through to run screensaver
                }

                // Run screensaver mode (default)
                RunScreensaver();
            }
            catch (Exception ex)
            {
                // Catch any exceptions that occur during initialization
                HandleFatalException("Application initialization failed", ex);
            }
        }

        /// <summary>
        /// Runs the configuration dialog with comprehensive error handling
        /// </summary>
        private static void RunConfigurationDialog()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[INFO] Opening configuration dialog");
                Application.Run(new PreferencesForm());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Configuration dialog failed: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");

                MessageBox.Show(
                    $"Failed to open screensaver settings:\n\n{ex.Message}\n\n" +
                    $"Application Version: {GetVersionInfo()}\n" +
                    $"Framework: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}\n\n" +
                    $"Please ensure:\n" +
                    $"• .NET 8 Desktop Runtime is installed\n" +
                    $"• No old versions of the screensaver are running\n" +
                    $"• Registry permissions are correct\n\n" +
                    $"See INSTALLATION.md for troubleshooting steps.",
                    "Web Page Screensaver - Configuration Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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
        /// Logs startup information for diagnostics
        /// </summary>
        private static void LogStartupInfo(string[] args)
        {
            try
            {
                var version = GetVersionInfo();
                var framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
                var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? "Unknown";

                System.Diagnostics.Debug.WriteLine("=".PadRight(70, '='));
                System.Diagnostics.Debug.WriteLine($"[STARTUP] Web Page Screensaver v{version}");
                System.Diagnostics.Debug.WriteLine($"[STARTUP] Framework: {framework}");
                System.Diagnostics.Debug.WriteLine($"[STARTUP] Executable: {exePath}");
                System.Diagnostics.Debug.WriteLine($"[STARTUP] Arguments: {(args.Length > 0 ? string.Join(" ", args) : "(none)")}");
                System.Diagnostics.Debug.WriteLine($"[STARTUP] OS: {Environment.OSVersion}");
                System.Diagnostics.Debug.WriteLine($"[STARTUP] 64-bit Process: {Environment.Is64BitProcess}");
                System.Diagnostics.Debug.WriteLine($"[STARTUP] 64-bit OS: {Environment.Is64BitOperatingSystem}");
                System.Diagnostics.Debug.WriteLine("=".PadRight(70, '='));
            }
            catch
            {
                // Ignore logging errors
            }
        }

        /// <summary>
        /// Gets version information string
        /// </summary>
        private static string GetVersionInfo()
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version;
                return version?.ToString() ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Global handler for UI thread exceptions
        /// </summary>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[EXCEPTION] Unhandled UI thread exception: {e.Exception.Message}");
            System.Diagnostics.Debug.WriteLine($"[EXCEPTION] Stack trace: {e.Exception.StackTrace}");

            HandleFatalException("An unexpected error occurred", e.Exception);
        }

        /// <summary>
        /// Global handler for non-UI thread exceptions
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EXCEPTION] Unhandled domain exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[EXCEPTION] Is terminating: {e.IsTerminating}");
                System.Diagnostics.Debug.WriteLine($"[EXCEPTION] Stack trace: {ex.StackTrace}");

                HandleFatalException("A fatal error occurred", ex);
            }
        }

        /// <summary>
        /// Shows detailed error information to the user
        /// </summary>
        private static void HandleFatalException(string message, Exception ex)
        {
            try
            {
                var errorDetails = $"{message}:\n\n" +
                                  $"Error: {ex.Message}\n\n" +
                                  $"Application Version: {GetVersionInfo()}\n" +
                                  $"Framework: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}\n\n" +
                                  $"Common Solutions:\n" +
                                  $"1. Ensure .NET 8 Desktop Runtime is installed\n" +
                                  $"   Download: https://dotnet.microsoft.com/download/dotnet/8.0\n\n" +
                                  $"2. Ensure WebView2 Runtime is installed\n" +
                                  $"   Download: https://go.microsoft.com/fwlink/p/?LinkId=2124703\n\n" +
                                  $"3. Remove old screensaver versions from C:\\Windows\\System32\\\n" +
                                  $"   (Look for Web-Page-Screensaver*.scr files)\n\n" +
                                  $"4. Check Event Viewer for detailed error information\n\n" +
                                  $"For detailed troubleshooting, see INSTALLATION.md\n\n" +
                                  $"Technical Details:\n{ex.GetType().Name}";

                MessageBox.Show(
                    errorDetails,
                    "Web Page Screensaver - Critical Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch
            {
                // Last resort - show minimal error
                try
                {
                    MessageBox.Show(
                        $"Critical error: {ex.Message}\n\nPlease reinstall the screensaver.",
                        "Fatal Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch
                {
                    // Can't even show message box - give up
                }
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
