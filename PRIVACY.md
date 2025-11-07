# Privacy Policy

**Effective Date**: 2025-11-07
**Last Updated**: 2025-11-07
**Version**: 2.0.0

---

## Introduction

Web Page Screensaver is committed to protecting your privacy. This Privacy Policy explains how we handle information in compliance with the **General Data Protection Regulation (GDPR)** and other privacy regulations.

**Key Principle**: **We do not collect, transmit, or store any personal data beyond what you explicitly configure on your local device.**

---

## Quick Summary

✅ **No Personal Data Collection**: We don't collect names, emails, IP addresses, or any personal information
✅ **No Telemetry**: No usage statistics, analytics, or tracking
✅ **No Internet Communication**: The application doesn't send any data to servers (except when loading your configured web pages)
✅ **Local Storage Only**: All settings stored locally on your computer
✅ **No Third Parties**: No data sharing with third parties
✅ **No Cookies**: Application itself doesn't set cookies (websites you visit may set cookies in WebView2)
✅ **Open Source**: Full transparency - all code is publicly available

---

## Information We Do NOT Collect

We want to be crystal clear about what we **DO NOT** do:

- ❌ **Personal Identifiable Information (PII)**: No names, emails, phone numbers, addresses
- ❌ **Usage Analytics**: No tracking of how you use the application
- ❌ **Telemetry**: No error reports, crash reports, or diagnostics sent to us
- ❌ **Device Information**: No collection of device IDs, OS version, hardware specs
- ❌ **Location Data**: No GPS, IP geolocation, or location tracking
- ❌ **Behavioral Tracking**: No monitoring of websites you visit
- ❌ **Unique Identifiers**: No user IDs, session IDs, or tracking tokens
- ❌ **Advertising IDs**: No advertising or marketing data collection

---

## Information Stored Locally

The following information is stored **on your local computer only** (in Windows Registry):

### Configuration Data

| Data Type | Purpose | Location | Retention |
|-----------|---------|----------|-----------|
| **URLs** | Websites to display in screensaver | HKCU\Software\Web-Page-Screensaver | Until you delete |
| **Screen Configuration** | Multi-monitor mode (Separate/Span/Mirror) | HKCU\Software\Web-Page-Screensaver | Until you delete |
| **Rotation Settings** | Time interval, shuffle order | HKCU\Software\Web-Page-Screensaver | Until you delete |
| **Behavior Settings** | Close on activity setting | HKCU\Software\Web-Page-Screensaver | Until you delete |

**Storage Location**: Windows Registry under `HKEY_CURRENT_USER\Software\Web-Page-Screensaver`
- This is a **user-specific** storage (not system-wide)
- Only accessible by your Windows user account
- Protected by Windows user account security

### WebView2 Data

The Microsoft Edge WebView2 runtime (used for rendering web pages) may store:

- **Browser Cache**: Temporary files from websites you visit
- **Cookies**: Set by websites you visit (not by our application)
- **Local Storage**: Website data (localStorage, IndexedDB)
- **Browsing History**: In WebView2 user data folder

**Location**: `%LOCALAPPDATA%\Microsoft\Edge\User Data\`

**Control**: This data can be cleared using WebView2 APIs or manually deleting the folder.

---

## Data Processing

### Legal Basis (GDPR Article 6)

Our data processing is based on:

**Article 6(1)(b) - Necessity for Performance of a Contract**:
- The URLs and settings you configure are necessary for the screensaver to function

**Article 6(1)(f) - Legitimate Interests**:
- Storing configuration locally for application functionality
- No balancing test required (no third-party data processing)

### Data Minimization (GDPR Article 5)

We practice **strict data minimization**:
- Only store what's absolutely necessary (URLs and settings)
- No "nice-to-have" data collection
- No data collection "in case we need it later"

### Purpose Limitation (GDPR Article 5)

Data is used **only** for:
- Displaying configured web pages as screensaver
- Saving and loading your configuration
- Application functionality

We **never** use data for:
- Marketing or advertising
- Analytics or statistics
- Profiling or behavioral analysis
- Selling or sharing with third parties

---

## Your Privacy Rights (GDPR)

### Right of Access (Article 15)

✅ **How to exercise**:
- Your data is stored in Windows Registry
- View it: `regedit.exe` → `HKCU\Software\Web-Page-Screensaver`
- Export it: Right-click key → Export

### Right to Rectification (Article 16)

✅ **How to exercise**:
- Open screensaver settings dialog
- Edit URLs, rotation settings, etc.
- Changes saved automatically

### Right to Erasure ("Right to be Forgotten") (Article 17)

✅ **How to exercise**:
- **Option 1**: Uninstall the application
- **Option 2**: Delete registry key: `HKCU\Software\Web-Page-Screensaver`
- **Option 3**: Delete individual URLs in settings dialog

### Right to Restriction of Processing (Article 18)

✅ **Not applicable** - No automated data processing beyond local storage

### Right to Data Portability (Article 20)

✅ **How to exercise**:
- Export registry key: `regedit.exe` → Right-click → Export
- Saves as .reg file (human-readable text format)
- Can import on another computer

### Right to Object (Article 21)

✅ **Not applicable** - No profiling or direct marketing

### Rights Related to Automated Decision-Making (Article 22)

✅ **Not applicable** - No automated decision-making or profiling

---

## Third-Party Services

### Microsoft Edge WebView2

**What it is**: The web rendering engine used to display websites
**Privacy Policy**: [Microsoft Privacy Statement](https://privacy.microsoft.com/privacystatement)

**Data Collection by WebView2**:
- WebView2 itself doesn't send data to Microsoft by default
- Websites you visit may collect data (subject to their privacy policies)
- SmartScreen (phishing protection) may send URL reputation checks to Microsoft

**Your Control**:
- You choose which websites to visit
- SmartScreen can be disabled (not recommended)
- WebView2 data can be cleared

### Websites You Visit

**Important**: When the screensaver displays websites:
- Those websites may collect data according to their own privacy policies
- They may set cookies, tracking pixels, etc.
- This is outside our control

**Your Responsibility**:
- Only add URLs from websites you trust
- Review privacy policies of websites you add
- Consider using first-party URLs only (no third-party analytics/ads)

---

## Children's Privacy (COPPA)

This application:
- Does not target children under 13
- Does not knowingly collect data from children
- No age verification required (application doesn't collect any data)

---

## Security

### Data Protection (GDPR Article 32)

We implement security measures:

✅ **Access Control**:
- User-specific registry storage (HKEY_CURRENT_USER)
- Protected by Windows user account security
- No network-accessible data

✅ **Input Validation**:
- URLs validated before use
- Dangerous URL schemes blocked (javascript:, data:, etc.)
- Prevents malicious content injection

✅ **Security Logging**:
- Security events logged for audit (DEBUG builds only)
- Sensitive data masked in logs
- Logs stored locally only

✅ **No Data Transmission**:
- No telemetry or analytics sent to servers
- No automatic updates that transmit data
- Application is fully offline (except web page loading)

For detailed security information, see [SECURITY.md](SECURITY.md).

---

## Data Breach Notification (GDPR Article 33-34)

**Applicability**: Since we don't collect or process personal data:
- No centralized data breach possible
- Local data protected by Windows security
- Each user's data isolated on their own device

**In Case of Vulnerability**:
- If a security vulnerability is discovered, we will:
  - Disclose it responsibly (see SECURITY.md)
  - Release a fix promptly
  - Notify users via GitHub releases

---

## International Data Transfers

✅ **Not Applicable**
- No data leaves your computer (except to load websites)
- No cross-border data transfers
- No data stored on servers

---

## California Privacy Rights (CCPA)

### Do Not Sell My Personal Information

✅ **We do not sell personal information** - Period.
- We don't collect personal information
- We don't sell any data
- No business relationship with data brokers

### CCPA Rights

All CCPA rights (similar to GDPR) are satisfied:
- **Right to Know**: Data is in Windows Registry (user-accessible)
- **Right to Delete**: Delete registry key or uninstall
- **Right to Opt-Out**: No data collection to opt out of

---

## Cookie Policy

### Application Cookies

✅ **We don't use cookies** - The application itself does not set cookies

### Website Cookies

⚠️ **Websites you visit may use cookies**:
- Set by websites loaded in WebView2
- Stored in WebView2 user data folder
- Subject to website's cookie policy
- Can be cleared by deleting WebView2 data

---

## Changes to This Privacy Policy

We may update this Privacy Policy:
- To reflect changes in the application
- To comply with new regulations
- To improve transparency

**Notification Method**:
- Updated in this file (PRIVACY.md)
- Version number and date changed
- Announced in release notes

**Your Rights**:
- Continued use implies acceptance
- You may stop using the application at any time

---

## Data Protection Officer (DPO)

**Applicability**: For small open-source projects without a legal entity, a formal DPO is not required under GDPR Article 37.

**Contact for Privacy Questions**:
- GitHub Issues: For general questions
- Security Advisory: For privacy/security concerns

---

## Compliance Summary

### GDPR Compliance Checklist

✅ **Lawful Basis**: Article 6(1)(b) and (f)
✅ **Data Minimization**: Only essential data stored
✅ **Purpose Limitation**: Clear, limited purpose
✅ **Storage Limitation**: User controls retention
✅ **Integrity & Confidentiality**: Windows security + input validation
✅ **Accountability**: This policy + open source code
✅ **Privacy by Design**: Minimal data collection from the start
✅ **Privacy by Default**: No telemetry enabled
✅ **Rights Exercised**: All rights supported (access, erasure, portability)

### Other Privacy Regulations

✅ **CCPA (California Consumer Privacy Act)**: Compliant
✅ **PIPEDA (Canada)**: Compliant
✅ **LGPD (Brazil)**: Compliant
✅ **Privacy Act (Australia)**: Compliant

---

## Frequently Asked Questions (FAQ)

### Q: Does the application send my data to the internet?

**A**: No. The application itself never sends any data to servers. The only network activity is when WebView2 loads websites you configured - and that's the same as opening those sites in your web browser.

### Q: Can you see what websites I add to my screensaver?

**A**: No. All settings are stored locally on your computer. We have no access to your configuration.

### Q: Does the application track which websites I visit?

**A**: No. The application doesn't track anything. It simply loads the URLs you configured.

### Q: What data does Microsoft collect via WebView2?

**A**: WebView2 itself doesn't collect data by default. SmartScreen (phishing protection) may check URL reputations with Microsoft. Websites you visit have their own data collection policies.

### Q: Can I use this application offline?

**A**: Yes, if you use `file://` URLs pointing to local HTML files. For online websites, you need an internet connection to load them.

### Q: How do I delete all my data?

**A**:
1. Uninstall the application, OR
2. Delete registry key: `HKCU\Software\Web-Page-Screensaver`, OR
3. Delete WebView2 data folder: `%LOCALAPPDATA%\Microsoft\Edge\User Data\`

### Q: Is my data encrypted?

**A**: Your data is stored in Windows Registry, which is protected by Windows user account security. For additional security, use Windows BitLocker to encrypt your entire drive.

### Q: Do you use cookies or tracking pixels?

**A**: No. The application doesn't use cookies, tracking pixels, or any tracking technology.

### Q: Will you ever add telemetry or analytics?

**A**: No plans to add telemetry. If we ever consider it, it would be:
- Opt-in only (disabled by default)
- Clearly disclosed in this policy
- Announced in release notes
- Minimal and anonymized

---

## Legal Entity Information

**Software Name**: Web Page Screensaver
**License**: BSD-3-Clause
**Type**: Open Source Software
**Legal Entity**: N/A (Open Source Project)
**Jurisdiction**: As determined by user's location

---

## Contact Information

**Privacy Questions**: Create a GitHub Issue
**Security Concerns**: Use GitHub Security Advisory
**General Support**: See README.md

**Repository**: https://github.com/[YOUR_REPO]/web-page-screensaver

---

## Acknowledgments

This Privacy Policy was drafted with reference to:
- GDPR Official Text (Regulation EU 2016/679)
- ICO (UK Information Commissioner's Office) guidance
- NIST Privacy Framework
- OWASP Privacy Best Practices

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 2.0.0 | 2025-11-07 | Complete rewrite for .NET 8 migration, GDPR compliance |
| 1.0.0 | 2010 | Initial release (no formal privacy policy) |

---

**Last Updated**: 2025-11-07
**Effective Date**: 2025-11-07
**Review Frequency**: Annually or as needed
**Next Review**: 2026-01-07

---

*This privacy policy reflects our commitment to transparency and user privacy. We believe in privacy-first software development and respect your right to control your own data.*

**BY USING THIS SOFTWARE, YOU ACKNOWLEDGE THAT YOU HAVE READ AND UNDERSTOOD THIS PRIVACY POLICY.**
