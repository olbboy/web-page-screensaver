# Installation & Troubleshooting Guide

Comprehensive installation guide for Web Page Screensaver v2.0 with world-class support documentation.

---

## 📋 Table of Contents

1. [System Requirements](#system-requirements)
2. [Pre-Installation Checklist](#pre-installation-checklist)
3. [Installation Steps](#installation-steps)
4. [First-Time Setup](#first-time-setup)
5. [Verification](#verification)
6. [Troubleshooting](#troubleshooting)
7. [Uninstallation](#uninstallation)
8. [Advanced Configuration](#advanced-configuration)

---

## 🖥️ System Requirements

### Minimum Requirements

| Component | Requirement | Notes |
|-----------|-------------|-------|
| **Operating System** | Windows 10 version 19041+ or Windows 11 | May 2020 Update or later |
| **Architecture** | x64 (64-bit) or x86 (32-bit) | AnyCPU build supports both |
| **RAM** | 512 MB available | More recommended for complex web pages |
| **Disk Space** | 50 MB for application + runtime | WebView2 cache may use additional space |
| **Display** | Any resolution | Tested up to 4K displays |

### Required Runtime Components

#### 1. .NET 8 Desktop Runtime ⚠️ CRITICAL

**What it is:** Microsoft's modern application runtime framework

**Download:** https://dotnet.microsoft.com/download/dotnet/8.0

**Which version to download:**
- ✅ **Desktop Runtime** (x64 or x86) - ~50MB
- ❌ NOT SDK (developers only)
- ❌ NOT ASP.NET Core Runtime (web servers only)

**Installation:**
```
1. Download "Desktop Runtime" installer
2. Run the installer
3. Follow prompts (default settings recommended)
4. Restart computer if prompted
```

**Verification:**
```powershell
# Run in PowerShell
dotnet --list-runtimes

# Expected output should include:
# Microsoft.WindowsDesktop.App 8.0.x [...]
```

#### 2. Microsoft Edge WebView2 Runtime ⚠️ CRITICAL

**What it is:** Modern Chromium-based web rendering engine

**Download:** https://go.microsoft.com/fwlink/p/?LinkId=2124703

**Automatic Installation:**
- Pre-installed on **Windows 11**
- Pre-installed on **Windows 10** with recent updates (21H2+)

**Manual Installation (if needed):**
```
1. Download "Evergreen Standalone Installer"
2. Run MicrosoftEdgeWebview2Setup.exe
3. Wait for installation (automatic, silent)
4. No restart required
```

**Verification:**
```
1. Open: C:\Program Files (x86)\Microsoft\EdgeWebView\Application
2. Check for version folder (e.g., 120.0.2210.144)
3. Or check Windows Settings → Apps → Microsoft Edge WebView2 Runtime
```

### Recommended Requirements

- **Windows 10 21H2** or **Windows 11** (WebView2 pre-installed)
- **4 GB RAM** (for multiple monitors or heavy web pages)
- **SSD** (faster page loading)
- **Internet connection** (for web content, not required for local files)

### Unsupported Systems

❌ **Windows 7** - End of support, .NET 8 not available
❌ **Windows 8/8.1** - .NET 8 not supported
❌ **Windows 10 older than 19041** - WebView2 not supported
❌ **Windows Server 2012 R2 and older** - Not supported

---

## ✅ Pre-Installation Checklist

Before installing the screensaver, ensure:

- [ ] Windows 10 19041+ or Windows 11 installed
- [ ] .NET 8 Desktop Runtime installed (verify with `dotnet --list-runtimes`)
- [ ] WebView2 Runtime installed (check Programs and Features)
- [ ] Administrator access (if installing to Program Files)
- [ ] Antivirus/SmartScreen not blocking .scr files
- [ ] At least one URL ready to display (e.g., https://www.google.com)

---

## 📦 Installation Steps

### Method 1: Release Package (Recommended)

**For End Users - Download Pre-Built Package**

1. **Download Latest Release**
   ```
   Visit: https://github.com/olbboy/web-page-screensaver/releases
   Download: Web-Page-Screensaver-v2.0.0.zip
   ```

2. **Extract Files**
   ```
   Right-click → Extract All
   Choose destination: C:\Screensavers\Web-Page-Screensaver
   ```

3. **Install Screensaver**
   ```
   Method A (Recommended):
   - Right-click Web-Page-Screensaver.scr
   - Select "Install"
   - Screen Saver Settings dialog opens automatically

   Method B (Manual):
   - Copy .scr file to C:\Windows\System32\ (requires admin)
   - Open Control Panel → Appearance → Screen Saver
   - Select "Web-Page-Screensaver" from dropdown
   ```

4. **Configure Settings**
   ```
   - Click "Settings..." button
   - Add URLs (one per line or space-separated)
   - Set rotation interval (seconds)
   - Enable shuffle if desired
   - Set "Close on activity" behavior
   - Click OK to save
   ```

### Method 2: Build from Source

**For Developers - Build from Source Code**

**Prerequisites:**
- .NET 8 SDK (not just runtime)
- Git (optional, for cloning)
- Visual Studio 2022 or VS Code (optional)

**Steps:**

1. **Clone Repository**
   ```bash
   git clone https://github.com/olbboy/web-page-screensaver.git
   cd web-page-screensaver
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Build Release**
   ```bash
   dotnet build --configuration Release
   ```

4. **Locate Output**
   ```
   File: bin/Release/net8.0-windows/Web-Page-Screensaver.scr
   Size: ~500 KB + dependencies
   ```

5. **Install** (follow Method 1, step 3)

---

## 🎯 First-Time Setup

### Step 1: Configure URLs

**Open Settings:**
- Right-click .scr file → Configure (before install)
- Or: Control Panel → Screen Saver → Settings button (after install)

**Add URLs - Examples:**

```
Web Pages:
https://www.google.com/trends/hottrends/visualize?nrow=5&ncol=5
https://screensaver.twingly.com/
https://www.bing.com/images/trending

Local Files:
file:///C:/Users/YourName/Documents/dashboard.html
file:///D:/Dashboards/sales.html

Local Network:
http://192.168.1.100:8080/dashboard
http://homeserver.local/monitoring
```

**URL Requirements:**
- ✅ Must start with `http://`, `https://`, or `file:///`
- ✅ For local files, use full path with forward slashes
- ✅ URLs must be on separate lines or space-separated
- ❌ No JavaScript injection (`javascript:`, `data:`, `vbscript:`)
- ❌ No executable files (`.exe`, `.bat`, `.js`, etc.)

### Step 2: Configure Rotation

**Rotation Interval:**
- Default: 30 seconds
- Minimum: 5 seconds (practical limit)
- Maximum: 3600 seconds (1 hour)
- Set to 0 to disable rotation (single URL only)

**Shuffle Order:**
- ✅ Enable: Randomize URL order each screensaver session
- ❌ Disable: Sequential order from top to bottom

### Step 3: Configure Multi-Monitor (if applicable)

**Three modes available:**

1. **Separate** (Default)
   - Different content on each monitor
   - Configure URLs per screen
   - Independent rotation settings
   - Best for: Different dashboards per screen

2. **Span**
   - Single webpage stretched across all displays
   - One URL list for all screens
   - Best for: Wide panoramic content

3. **Mirror**
   - Same content on all displays
   - Synchronized rotation
   - Best for: Consistent display across monitors

### Step 4: Set Windows Screensaver Timing

**Control Panel → Screen Saver:**
- Wait time: 5-60 minutes (recommended: 10 minutes)
- "On resume, display logon screen": Enable for security
- Power settings: Configure display sleep separately

---

## ✔️ Verification

### Test Screensaver Immediately

**Method 1: Preview (Limited)**
```
Control Panel → Screen Saver → Preview button
Note: Preview mode not fully supported, may show blank
```

**Method 2: Command Line (Recommended)**
```cmd
# Test screensaver mode
Web-Page-Screensaver.scr /s

# Test configuration dialog
Web-Page-Screensaver.scr /c
```

**Method 3: Wait for Timeout**
```
Leave computer idle for configured wait time
Screensaver should activate automatically
```

### Verify Installation Success

✅ **Checklist:**

- [ ] Screensaver appears in Control Panel dropdown
- [ ] Settings button opens configuration dialog
- [ ] URLs are saved after clicking OK
- [ ] Test mode (`/s`) displays web content
- [ ] Page rotates after interval (if multiple URLs)
- [ ] Mouse/keyboard activity closes screensaver
- [ ] No error messages appear

### Check Runtime Dependencies

**PowerShell Verification Script:**

```powershell
# Check .NET 8 Desktop Runtime
$dotnetRuntimes = & dotnet --list-runtimes 2>&1
if ($dotnetRuntimes -match "Microsoft.WindowsDesktop.App 8.0") {
    Write-Host "✅ .NET 8 Desktop Runtime: INSTALLED" -ForegroundColor Green
} else {
    Write-Host "❌ .NET 8 Desktop Runtime: MISSING" -ForegroundColor Red
    Write-Host "Download: https://dotnet.microsoft.com/download/dotnet/8.0"
}

# Check WebView2 Runtime
$webview2Path = "${env:ProgramFiles(x86)}\Microsoft\EdgeWebView\Application"
if (Test-Path $webview2Path) {
    $version = (Get-ChildItem $webview2Path | Where-Object { $_.PSIsContainer } | Select-Object -First 1).Name
    Write-Host "✅ WebView2 Runtime: INSTALLED (v$version)" -ForegroundColor Green
} else {
    Write-Host "❌ WebView2 Runtime: MISSING" -ForegroundColor Red
    Write-Host "Download: https://go.microsoft.com/fwlink/p/?LinkId=2124703"
}

# Check Windows Version
$osVersion = [System.Environment]::OSVersion.Version
if ($osVersion.Build -ge 19041) {
    Write-Host "✅ Windows Version: COMPATIBLE (Build $($osVersion.Build))" -ForegroundColor Green
} else {
    Write-Host "❌ Windows Version: INCOMPATIBLE (Build $($osVersion.Build))" -ForegroundColor Red
    Write-Host "Required: Windows 10 Build 19041 or later"
}
```

---

## 🔧 Troubleshooting

### Issue 1: Screensaver Won't Start

**Symptom:** Black screen or immediate exit when screensaver should activate

**Possible Causes & Solutions:**

#### A. .NET 8 Runtime Not Installed

**Error Message:** "To run this application, you must install .NET Desktop Runtime..."

**Solution:**
```
1. Download .NET 8 Desktop Runtime:
   https://dotnet.microsoft.com/download/dotnet/8.0

2. Select "Desktop Runtime" (x64 or x86 matching your Windows)

3. Install and restart computer

4. Verify: Open PowerShell and run:
   dotnet --list-runtimes

5. Look for: Microsoft.WindowsDesktop.App 8.0.x
```

#### B. WebView2 Runtime Not Installed

**Error Message:** "Failed to initialize web rendering engine... WebView2 Runtime is required"

**Solution:**
```
1. Download WebView2 Evergreen Installer:
   https://go.microsoft.com/fwlink/p/?LinkId=2124703

2. Run MicrosoftEdgeWebview2Setup.exe

3. Wait for automatic installation (no prompts)

4. Verify: Check Programs and Features for "Microsoft Edge WebView2 Runtime"
```

#### C. No URLs Configured

**Symptom:** Screensaver starts but shows blank black screen

**Solution:**
```
1. Right-click .scr file → Configure (or use /c switch)
2. Add at least one valid URL
3. Click OK to save
4. Test again with /s switch
```

#### D. Corrupted Registry Settings

**Symptom:** Settings dialog opens but doesn't save, or screensaver ignores settings

**Solution:**
```
1. Delete registry key:
   HKEY_CURRENT_USER\Software\Web-Page-Screensaver

2. Reinstall screensaver (.scr file)

3. Reconfigure URLs from scratch
```

### Issue 2: Web Pages Not Loading

**Symptom:** Screensaver starts but pages don't load, blank or error display

**Possible Causes & Solutions:**

#### A. Invalid URLs

**Check URL Format:**
```
✅ Correct:
https://www.example.com
http://192.168.1.100:8080/dashboard
file:///C:/Users/Name/dashboard.html

❌ Incorrect:
www.example.com (missing protocol)
C:\Users\Name\dashboard.html (wrong format for file://)
javascript:alert('test') (blocked for security)
```

#### B. Firewall/Network Issues

**Solution:**
```
1. Test URLs in Microsoft Edge browser first
2. Check internet connection
3. Verify firewall allows screensaver network access
4. For corporate networks, check proxy settings
```

#### C. URL Blocked by Security Validation

**Symptom:** URLs work in browser but not in screensaver

**Check Event Viewer:**
```
1. Open: Event Viewer → Windows Logs → Application
2. Look for "Web-Page-Screensaver" events
3. Check for "[SECURITY] Blocked URL" messages
```

**Security Validation Rules:**
- ✅ Allowed: `http://`, `https://`, `file:///`
- ❌ Blocked: `javascript:`, `data:`, `vbscript:`, `about:`
- ❌ Blocked: Executable file extensions (`.exe`, `.bat`, `.js`, `.vbs`)
- ❌ Blocked: Path traversal characters (`..`, `%`)

**Solution:**
- Remove blocked URLs from configuration
- Use secure alternatives
- Report false positives in GitHub Issues

#### D. SSL/TLS Certificate Errors

**Symptom:** HTTPS pages don't load, HTTP works

**Solution:**
```
1. Verify system date/time is correct
2. Update Windows (certificate store)
3. For self-signed certs, add to Windows certificate store
4. Use HTTP for local/internal sites (if safe)
```

### Issue 3: Settings Not Saving

**Symptom:** Configuration dialog closes but settings revert to defaults

**Possible Causes & Solutions:**

#### A. Registry Permission Issues

**Solution:**
```
1. Run regedit as Administrator
2. Navigate to: HKEY_CURRENT_USER\Software
3. Right-click → Permissions
4. Ensure your user has "Full Control"
5. Try reinstalling screensaver
```

#### B. Registry Key Corruption

**Solution:**
```
PowerShell (as Administrator):

# Delete corrupted key
Remove-Item -Path "HKCU:\Software\Web-Page-Screensaver" -Recurse -Force

# Restart screensaver configuration
.\Web-Page-Screensaver.scr /c
```

### Issue 4: Performance Issues

**Symptom:** Screensaver is slow, choppy, or uses excessive CPU/RAM

**Possible Causes & Solutions:**

#### A. Complex Web Pages

**Solution:**
```
- Simplify URLs (avoid heavy animations, videos)
- Increase rotation interval to reduce loading frequency
- Reduce number of URLs in rotation
- Use static content when possible
```

#### B. Memory Leaks (rare)

**Solution:**
```
- Restart screensaver periodically (Windows will do automatically)
- Report issues with specific URLs on GitHub
- Monitor Task Manager for memory growth
```

#### C. Outdated WebView2

**Solution:**
```
1. Update Microsoft Edge (updates WebView2 automatically)
2. Or manually download latest WebView2 installer
3. Restart computer after update
```

### Issue 5: Multi-Monitor Issues

**Symptom:** Screensaver only appears on one monitor, or layout incorrect

**Solutions:**

#### A. Verify Display Settings

```
Windows Settings → System → Display
- Ensure all monitors are detected
- Set correct primary display
- Verify resolution and orientation
```

#### B. Check Multi-Monitor Mode

```
Screensaver Settings → Multi-Monitor Mode:
- Try "Mirror" mode to test all displays
- For "Separate" mode, configure URLs for each screen
- For "Span" mode, use single wide-format URL
```

### Issue 6: Screensaver Won't Close

**Symptom:** Mouse/keyboard input doesn't close screensaver

**Solutions:**

#### A. Check "Close on Activity" Setting

```
Settings dialog → "Close on activity" checkbox
- ✅ Enable: Closes immediately on mouse/keyboard
- ❌ Disable: Shows close button instead
```

#### B. Force Close

```
Task Manager method:
1. Press Ctrl+Alt+Delete
2. Task Manager
3. Find "Web-Page-Screensaver.exe"
4. End Task
```

### Issue 7: Security Warnings/Blocks

**Symptom:** Windows SmartScreen or antivirus blocks screensaver

**Solutions:**

#### A. SmartScreen Warning

```
"Windows protected your PC" message:
1. Click "More info"
2. Click "Run anyway"
3. Or: Right-click .scr → Properties → Unblock checkbox
```

#### B. Antivirus False Positive

```
1. Verify .scr file is from official GitHub release
2. Check file hash (provided in release notes)
3. Add exception in antivirus for .scr file
4. Report false positive to antivirus vendor
```

### Issue 8: Build Errors (Developers)

**Symptom:** `dotnet build` fails with compilation errors

**Common Errors & Solutions:**

#### A. CS0246: Type or namespace not found

**Error Example:**
```
error CS0246: The type or namespace name 'WebView2' could not be found
```

**Solution:**
```bash
# Restore NuGet packages
dotnet restore

# Clean and rebuild
dotnet clean
dotnet build --configuration Release
```

#### B. Missing .NET 8 SDK

**Error:**
```
error: The current .NET SDK does not support 'net8.0-windows' as a target framework
```

**Solution:**
```
1. Install .NET 8 SDK (not just runtime):
   https://dotnet.microsoft.com/download/dotnet/8.0

2. Verify: dotnet --version
   Expected: 8.0.x
```

#### C. NETSDK1045: Target framework not installed

**Solution:**
```
Install Windows Desktop workload:
dotnet workload install windows-desktop
```

### Issue 9: Version Conflicts / Old Files

**Symptom:** Settings or Preview buttons don't work, application crashes silently, or Event Viewer shows .NET Framework 4.x errors

**This is a CRITICAL issue affecting users upgrading from v1.x to v2.0!**

#### A. Old Screensaver File Still Installed

**Symptoms:**
- Event Viewer shows "Framework Version: v4.0.30319" (old .NET Framework)
- Application version shows 1.2.0.0 instead of 2.0.0.0
- Settings button doesn't open dialog
- Preview shows nothing
- No error messages displayed

**Event Viewer Evidence:**
```
Application: Web-Page-Screensaver.scr or Web-Page-Screensaver-2.scr
Framework Version: v4.0.30319 (.NET Framework 4.x)
Exception code: 0xe0434352 (CLR exception)
Faulting module: KERNELBASE.dll
```

**Root Cause:**
- Old v1.x .scr file (requiring .NET Framework 4.0) still in System32
- Windows runs old file instead of new v2.0 file (.NET 8)
- Old version crashes because .NET Framework 4.0 may not be installed

**Solution - Remove All Old Files:**

```powershell
# Open PowerShell as Administrator
# Navigate to System32
cd C:\Windows\System32

# List all Web-Page-Screensaver files
dir Web-Page-Screensaver*.scr

# You might see:
# - Web-Page-Screensaver.scr (old v1.x)
# - Web-Page-Screensaver-2.scr (old v1.x)
# - Other variants

# Delete ALL old versions (IMPORTANT!)
del Web-Page-Screensaver.scr
del Web-Page-Screensaver-2.scr
# Delete any other variants you found

# Verify they're gone
dir Web-Page-Screensaver*.scr

# Should show: File Not Found
```

**After Removing Old Files:**

1. **Reinstall v2.0:**
   ```
   - Right-click the NEW Web-Page-Screensaver.scr (v2.0)
   - Select "Install"
   - Verify version in error dialog if it appears (should say 2.0.0.0)
   ```

2. **Verify Installation:**
   ```
   - Open Control Panel → Screen Saver
   - Click "Settings..." button
   - Should now open configuration dialog successfully
   - New error dialogs (v2.0) show detailed diagnostic information
   ```

3. **Check Version in Event Viewer (if issues persist):**
   ```
   Event Viewer → Windows Logs → Application
   - Look for "Web-Page-Screensaver" events
   - Framework should show: .NET 8.0.x (not v4.0.30319)
   - Version should show: 2.0.0.0 (not 1.2.0.0)
   ```

#### B. .NET Framework 4.0 Dependency (Old Version)

**If you MUST run old v1.x** (not recommended):

```
Old v1.2.0.0 requires .NET Framework 4.0 (obsolete):
- Download: https://www.microsoft.com/download/details.aspx?id=17718
- NOT recommended - migrate to v2.0 instead
- v1.x has security vulnerabilities (uses IE engine)
- v1.x unsupported and deprecated
```

#### C. Identifying Which Version is Running

**Method 1: Error Dialog (v2.0 feature)**

If Settings crashes but shows error dialog:
```
✅ v2.0: Shows detailed error with version number
   "Application Version: 2.0.0.0"
   "Framework: .NET 8.0.x"
   Lists troubleshooting steps

❌ v1.x: Silent crash or generic Windows error
   No detailed diagnostics
   May show ".NET Framework initialization failed"
```

**Method 2: File Properties**

```
Right-click .scr file → Properties → Details tab

v1.2.0.0:
- File version: 1.2.0.0
- Product version: 1.2.0.0
- .NET Framework 4.0 dependency

v2.0.0.0:
- File version: 2.0.0.0
- Product version: 2.0.0.0
- .NET 8 dependency
```

**Method 3: DebugView (Advanced)**

```
1. Download DebugView from Sysinternals
2. Run as Administrator
3. Enable: Capture → Capture Global Win32
4. Run screensaver with /c switch
5. Check output:

v2.0 startup log:
======================================================================
[STARTUP] Web Page Screensaver v2.0.0.0
[STARTUP] Framework: .NET 8.0.x
[STARTUP] Executable: C:\Windows\System32\Web-Page-Screensaver.scr
======================================================================

v1.x: No startup log (no diagnostic logging)
```

#### D. Migration Checklist v1.x → v2.0

Before installing v2.0:

- [ ] Backup registry settings (see MIGRATION.md)
- [ ] Note down all configured URLs
- [ ] Export registry: `HKEY_CURRENT_USER\Software\Web-Page-Screensaver`
- [ ] Close screensaver if running
- [ ] Remove ALL old .scr files from System32 (see Solution A above)
- [ ] Install .NET 8 Desktop Runtime
- [ ] Install WebView2 Runtime
- [ ] Install v2.0 screensaver
- [ ] Reconfigure URLs (v2.0 is backward compatible with v1.x settings)
- [ ] Test with Settings button
- [ ] Test with screensaver activation

**Settings Migration:**

```
Good news! v2.0 reads v1.x registry settings automatically.
After installing v2.0:
1. Your URLs will be preserved
2. Multi-monitor settings preserved
3. Rotation intervals preserved
4. All preferences carried over

However, URLs will be validated for security:
- Blocked schemes removed (javascript:, data:, etc.)
- Dangerous file extensions blocked (.exe, .bat, etc.)
- Check Debug output for rejected URLs
```

---

## 🗑️ Uninstallation

### Complete Removal

**Step 1: Remove Screensaver**
```
1. Control Panel → Screen Saver
2. Change to "(None)" in dropdown
3. Click OK
```

**Step 2: Delete Files**
```
Option A: If installed to System32
- Delete: C:\Windows\System32\Web-Page-Screensaver.scr
- May require Administrator privileges

Option B: If using portable
- Delete entire extracted folder
```

**Step 3: Clean Registry (Optional)**
```
1. Run: regedit
2. Navigate: HKEY_CURRENT_USER\Software
3. Delete: Web-Page-Screensaver folder
4. This removes all saved URLs and settings
```

**Step 4: Remove Runtimes (Optional)**
```
Only if not used by other applications:

Uninstall .NET 8:
- Programs and Features → Microsoft .NET Runtime
- Uninstall all .NET 8.x entries

Uninstall WebView2:
- NOT RECOMMENDED (used by Edge and other apps)
- Only if you're certain no other apps use it
```

---

## ⚙️ Advanced Configuration

### Registry Settings

All settings stored in: `HKEY_CURRENT_USER\Software\Web-Page-Screensaver`

**Values:**

| Key | Type | Description | Default |
|-----|------|-------------|---------|
| `MultiScreenMode` | String | "Separate", "Span", or "Mirror" | "Separate" |
| `CloseOnActivity` | DWORD | 1 = close immediately, 0 = show button | 1 |
| `Screen0_Urls` | String | Pipe-separated URLs for screen 0 | (empty) |
| `Screen0_RotationInterval` | DWORD | Seconds between rotations | 30 |
| `Screen0_Randomize` | DWORD | 1 = shuffle, 0 = sequential | 0 |

**Example Manual Configuration:**

```reg
Windows Registry Editor Version 5.00

[HKEY_CURRENT_USER\Software\Web-Page-Screensaver]
"MultiScreenMode"="Separate"
"CloseOnActivity"=dword:00000001
"Screen0_Urls"="https://www.google.com|https://www.bing.com"
"Screen0_RotationInterval"=dword:0000001e
"Screen0_Randomize"=dword:00000001
```

### Command-Line Switches

```cmd
Web-Page-Screensaver.scr [/s | /c | /p]

/s or /S    Run screensaver (full screen)
/c or /C    Open configuration dialog
/p or /P    Preview mode (not supported, will exit)
```

### WebView2 User Data Folder

**Default Location:**
```
%LocalAppData%\Microsoft\EdgeWebView2\Web-Page-Screensaver
```

**Contains:**
- Cookies
- Cache
- Local Storage
- IndexedDB

**To clear browsing data:**
```
1. Close screensaver completely
2. Delete folder (may require admin)
3. Folder recreates on next run
```

### Custom WebView2 Configuration

**For Developers - Modify WebView2SecurityManager.cs:**

```csharp
// Example: Enable developer tools for debugging
settings.AreDevToolsEnabled = true;  // Change to true

// Example: Allow downloads
webView2.CoreWebView2.DownloadStarting += (s, e) => {
    e.Handled = false;  // Allow download
};
```

**⚠️ Warning:** Modifying security settings may introduce vulnerabilities!

---

## 📞 Getting Help

### Before Requesting Support

1. ✅ Read this entire guide
2. ✅ Check [README.md](README.md) for basic information
3. ✅ Review [SECURITY.md](SECURITY.md) for security-related issues
4. ✅ Search existing [GitHub Issues](https://github.com/olbboy/web-page-screensaver/issues)

### Requesting Support

**Create a new issue with:**

1. **System Information:**
   ```
   - Windows version (e.g., Windows 11 23H2)
   - .NET version (output of: dotnet --list-runtimes)
   - WebView2 version (from Programs and Features)
   ```

2. **Problem Description:**
   - What were you trying to do?
   - What happened instead?
   - Can you reproduce it consistently?

3. **Error Messages:**
   - Screenshots of error dialogs
   - Event Viewer logs (if applicable)
   - Output of verification PowerShell script

4. **URLs (if relevant):**
   - Example URL that fails (remove sensitive info)
   - Does it work in Edge browser?

5. **Logs:**
   - Run screensaver with DebugView (SysInternals)
   - Capture [SECURITY] and [AUDIT] log messages

### Security Issues

**Do NOT create public issues for security vulnerabilities!**

See [SECURITY.md](SECURITY.md) for responsible disclosure process.

---

## 📚 Additional Resources

- **Main Documentation:** [README.md](README.md)
- **Security Policy:** [SECURITY.md](SECURITY.md)
- **Privacy Policy:** [PRIVACY.md](PRIVACY.md)
- **Migration Guide:** [MIGRATION.md](MIGRATION.md)
- **Contributing:** [CONTRIBUTING.md](CONTRIBUTING.md)
- **Changelog:** [CHANGELOG.md](CHANGELOG.md)

---

**Last Updated:** 2025-11-07
**Version:** 2.0.0
**Status:** Production Ready 🚀
