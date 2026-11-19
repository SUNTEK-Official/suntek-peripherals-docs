# Changelog

All notable changes to this repository are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Documentation updates for additional peripheral models and platform revisions under development.

## [1.0.0] - 2026-11-19

### Added
- Initial release of Windows and Ubuntu integration guides for SUNTEK thermal label and receipt printers, barcode scanners, and RGB LED controller boards.
- Printer: XP-series TSPL thermal label printer driver installation guide for Windows; MP80PC 80 mm ESC/POS CUPS setup script and usage guide for Ubuntu.
- Barcode scanner: USB HID plug-and-play quick start (both platforms), serial-port development and test guides, sanitized `BarcodeScanner.dll`, and a clean English C# serial sample.
- LED controller: serial HEX command reference with verified fixed-color commands (both platforms).
- Complete Windows XP-series printer driver suite and barcode scanner test tool published as checksummed `.zip` assets on GitHub Releases under tag `v1.0.0`.
- `THIRD_PARTY_NOTICES.md` documenting bundled third-party driver components.
- `assets/` with official SUNTEK PNG/WebP logo and banner artwork.

### Changed
- Standardized all directories and sub-documents to lowercase `kebab-case` paths.
- Converted the printer driver installation document from HTML to Markdown.
- Removed redundant partial Windows driver files from the Git tree; full driver packages are distributed via GitHub Releases.

### Security
- Driver package customer-facing documents use only `support@suntekai.com` and `https://www.suntekai.com` as support channels.
- Sanitized internal build PDB path inside `BarcodeScanner.dll`.
- Large driver installers and test executables are distributed exclusively through GitHub Releases rather than the Git source tree.
