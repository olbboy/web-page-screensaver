# Contributing to Web Page Screensaver

Thank you for your interest in contributing! This document provides guidelines for contributing to this project.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [How to Contribute](#how-to-contribute)
- [Pull Request Process](#pull-request-process)
- [Coding Standards](#coding-standards)
- [Security](#security)
- [Testing](#testing)
- [Documentation](#documentation)

## Code of Conduct

This project adheres to a [Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to the project maintainers.

## Getting Started

### Prerequisites

- **.NET 8 SDK**: [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Visual Studio 2022** (recommended) or **VS Code** with C# extension
- **Git**: For version control
- **Windows 10/11**: Required for WebView2 development

### Fork and Clone

1. Fork the repository on GitHub
2. Clone your fork locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/web-page-screensaver.git
   cd web-page-screensaver
   ```
3. Add upstream remote:
   ```bash
   git remote add upstream https://github.com/olbboy/web-page-screensaver.git
   ```

## Development Setup

### Build the Project

```bash
# Restore dependencies
dotnet restore

# Build in Debug mode
dotnet build

# Build in Release mode (generates .scr file)
dotnet build --configuration Release
```

### Run the Screensaver

```bash
# Open configuration dialog
.\bin\Debug\net8.0-windows\Web-Page-Screensaver.exe /c

# Run screensaver
.\bin\Debug\net8.0-windows\Web-Page-Screensaver.exe /s
```

### Development Tools

- **Visual Studio 2022**: Full IDE with debugger
- **VS Code**: Lightweight editor with C# Dev Kit
- **dotnet CLI**: Command-line build and test tools

## How to Contribute

### Reporting Bugs

Before creating bug reports, please check existing issues. When creating a bug report, include:

- **Clear title and description**
- **Steps to reproduce**
- **Expected vs actual behavior**
- **Screenshots** (if applicable)
- **Environment details**:
  - OS version (Windows 10/11)
  - .NET runtime version
  - WebView2 runtime version

**Use the Bug Report template** when creating issues.

### Suggesting Enhancements

Enhancement suggestions are welcome! Please:

- **Check existing feature requests** first
- **Provide clear use cases**
- **Explain benefits**
- **Consider security implications**

**Use the Feature Request template** when creating issues.

### Security Vulnerabilities

**DO NOT** report security vulnerabilities via public issues.

Please see [SECURITY.md](SECURITY.md) for responsible disclosure procedures.

## Pull Request Process

### 1. Create a Branch

```bash
# Update your fork
git fetch upstream
git checkout main
git merge upstream/main

# Create feature branch
git checkout -b feature/your-feature-name
```

### Branch Naming Convention

- `feature/description` - New features
- `fix/description` - Bug fixes
- `security/description` - Security improvements
- `docs/description` - Documentation changes
- `refactor/description` - Code refactoring
- `test/description` - Test additions/changes

### 2. Make Changes

- Follow [Coding Standards](#coding-standards)
- Write clear, descriptive commit messages
- Keep commits focused and atomic
- Add tests for new features
- Update documentation as needed

### 3. Test Your Changes

```bash
# Build and verify
dotnet build --configuration Release

# Run the screensaver to test
.\bin\Release\net8.0-windows\Web-Page-Screensaver.scr /c
```

### 4. Commit Your Changes

```bash
# Stage changes
git add .

# Commit with descriptive message
git commit -m "feat: add new multi-monitor feature

- Implement feature X
- Fix edge case Y
- Update documentation"
```

**Commit Message Format:**
```
<type>: <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `security`: Security improvement
- `docs`: Documentation changes
- `refactor`: Code refactoring
- `test`: Test changes
- `chore`: Build/tooling changes

### 5. Push and Create PR

```bash
# Push to your fork
git push origin feature/your-feature-name
```

Then create a Pull Request on GitHub with:

- **Clear title** describing the change
- **Detailed description** of what and why
- **Link to related issues** (if any)
- **Screenshots** (if UI changes)
- **Testing performed**

### 6. Code Review

- Address reviewer feedback promptly
- Keep discussions professional and constructive
- Update PR based on feedback
- Squash commits if requested

## Coding Standards

### C# Style Guide

Follow these conventions:

#### Naming

```csharp
// PascalCase for classes, methods, properties
public class ScreensaverForm { }
public void InitializeWebView2() { }
public string UrlPattern { get; set; }

// camelCase for local variables and parameters
var webView2 = new WebView2();
void Navigate(string url) { }

// _camelCase for private fields
private WebView2 _webView2;

// UPPER_CASE for constants
private const string DEFAULT_URL = "https://example.com";
```

#### Code Organization

```csharp
// Order: public → protected → private
// Within each: fields → constructors → properties → methods

public class Example
{
    // Public fields (avoid, use properties)

    // Private fields
    private WebView2 _webView2;

    // Constructors
    public Example() { }

    // Public properties
    public string Url { get; set; }

    // Public methods
    public void Initialize() { }

    // Private methods
    private void ValidateUrl() { }
}
```

#### Documentation

```csharp
/// <summary>
/// Validates if a URL is safe to load.
/// </summary>
/// <param name="url">URL to validate</param>
/// <param name="errorMessage">Error message if validation fails</param>
/// <returns>True if URL is valid, false otherwise</returns>
public static bool IsValidUrl(string url, out string errorMessage)
{
    // Implementation
}
```

### Security Best Practices

**IMPORTANT**: All contributions must follow security guidelines:

✅ **Input Validation**
- Validate all user inputs
- Use whitelist validation (not blacklist)
- Sanitize before use

✅ **Error Handling**
- Use try-catch for external calls
- Don't expose sensitive info in errors
- Log security events

✅ **Secure Coding**
- No hardcoded credentials
- Use parameterized queries (if applicable)
- Follow OWASP guidelines

❌ **Security Anti-Patterns**
- Don't disable security features
- Don't use `eval()` or similar
- Don't trust user input
- Don't ignore security warnings

### Code Quality

- **Keep methods short** (< 50 lines preferred)
- **Use meaningful names** (avoid abbreviations)
- **Add comments** for complex logic
- **Follow SOLID principles**
- **Prefer composition over inheritance**
- **Use nullable reference types** (`string?` for nullables)

### Example Good Code

```csharp
/// <summary>
/// Validates URL and navigates to it securely.
/// Implements defense-in-depth security approach.
/// </summary>
private void BrowseTo(string url)
{
    // Input validation (first line of defense)
    if (!UrlValidator.IsValidUrl(url, out string errorMessage))
    {
        Debug.WriteLine($"[SECURITY] Blocked invalid URL: {errorMessage}");
        return;
    }

    // Proceed with navigation
    try
    {
        webView2.CoreWebView2.Navigate(url);
        Debug.WriteLine($"[INFO] Navigated to: {MaskUrl(url)}");
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"[ERROR] Navigation failed: {ex.Message}");
        // Handle error gracefully
    }
}

// Helper method for privacy-compliant logging
private string MaskUrl(string url)
{
    // Mask query parameters for privacy (GDPR compliance)
    var uri = new Uri(url);
    return $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}";
}
```

## Security

### Security-First Development

All contributions must consider security implications:

1. **Threat Modeling**: Consider potential threats
2. **Input Validation**: Validate all external inputs
3. **Least Privilege**: Minimize permissions
4. **Defense-in-Depth**: Multiple security layers
5. **Security Testing**: Test for vulnerabilities

### Security Checklist

Before submitting PR, ensure:

- [ ] All user inputs are validated
- [ ] No hardcoded secrets or credentials
- [ ] Error messages don't leak sensitive info
- [ ] Security logging is in place
- [ ] OWASP guidelines followed
- [ ] No new security warnings introduced

### Compliance

This project adheres to:
- **OWASP Top 10** guidelines
- **NIST SP 800-53** controls
- **GDPR** privacy requirements
- **Microsoft SDL** practices

See [SECURITY.md](SECURITY.md) for details.

## Testing

### Manual Testing

Test your changes thoroughly:

1. **Build test**:
   ```bash
   dotnet build --configuration Release
   ```

2. **Functional test**:
   - Open settings dialog (`/c`)
   - Add/remove URLs
   - Test rotation
   - Test multi-monitor modes

3. **Security test**:
   - Try invalid URLs (javascript:, data:)
   - Test with dangerous file extensions
   - Verify validation messages

4. **Edge cases**:
   - Empty URL lists
   - Very long URLs
   - Special characters
   - Network disconnection

### Future: Automated Tests

We plan to add:
- Unit tests (xUnit)
- Integration tests
- Security tests
- Performance tests

## Documentation

### Update Documentation

When making changes, update:

- **README.md**: User-facing features
- **SECURITY.md**: Security-related changes
- **PRIVACY.md**: Privacy implications
- **MIGRATION.md**: Migration impacts
- **Code comments**: Complex logic
- **XML docs**: Public APIs

### Documentation Style

- Use clear, concise language
- Include code examples
- Add screenshots for UI changes
- Update table of contents
- Check spelling and grammar

## Project Structure

```
web-page-screensaver/
├── .github/               # GitHub-specific files
│   ├── workflows/         # CI/CD workflows
│   ├── ISSUE_TEMPLATE/    # Issue templates
│   └── dependabot.yml     # Dependency updates
├── Properties/            # Assembly metadata
├── Security/              # Security implementation
│   ├── UrlValidator.cs
│   └── WebView2SecurityManager.cs
├── Program.cs             # Entry point
├── ScreensaverForm.cs     # Main screensaver
├── PreferencesForm.cs     # Settings UI
├── PreferencesManager.cs  # Configuration
├── SECURITY.md            # Security policy
├── PRIVACY.md             # Privacy policy
├── CONTRIBUTING.md        # This file
├── CODE_OF_CONDUCT.md     # Code of conduct
├── CHANGELOG.md           # Version history
└── README.md              # Main documentation
```

## Getting Help

### Resources

- **Documentation**: Read the docs in this repo
- **Issues**: Search existing issues
- **Discussions**: GitHub Discussions (if enabled)
- **Security**: See [SECURITY.md](SECURITY.md)

### Communication

- Be respectful and professional
- Provide context and details
- Be patient with responses
- Follow up on feedback

## Recognition

Contributors will be recognized in:
- **README.md** - Credits section
- **CHANGELOG.md** - Version releases
- **GitHub Contributors** - Automatic recognition

Thank you for contributing to Web Page Screensaver! 🎉

---

**Questions?** Open an issue or discussion on GitHub.
