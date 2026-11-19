<div align="center">
  <a href="https://www.suntekai.com">
    <picture>
      <source media="(prefers-color-scheme: dark)" srcset="https://raw.githubusercontent.com/SUNTEK-Official/.github/main/profile/assets/logo-dark.png">
      <source media="(prefers-color-scheme: light)" srcset="https://raw.githubusercontent.com/SUNTEK-Official/.github/main/profile/assets/logo-light.png">
      <img alt="SUNTEK Technology Co., Ltd." src="https://raw.githubusercontent.com/SUNTEK-Official/.github/main/profile/assets/logo-light.png" height="44">
    </picture>
  </a>

  <h1>SUNTEK Peripheral Integration Suite</h1>

  <p>
    <strong>Official Integration Guides, Drivers &amp; Protocol Docs</strong> &middot; <em>Enjoy Smarter Service</em><br>
    Pure English technical documentation and driver distribution guide maintained by <strong>SUNTEK Technology Co., Ltd.</strong>
  </p>

  <p>
    <a href="./LICENSE"><img src="https://img.shields.io/badge/License-Commercial_SDK-172A88?style=for-the-badge" alt="License"></a>
    <a href="https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases"><img src="https://img.shields.io/github/v/release/SUNTEK-Official/suntek-peripherals-docs?style=for-the-badge&color=F39800&label=Release" alt="Latest Release"></a>
    <img src="https://img.shields.io/badge/Platforms-Windows_%7C_Linux-2563EB?style=for-the-badge" alt="Platforms">
    <a href="https://www.suntekai.com"><img src="https://img.shields.io/badge/Website-suntekai.com-18339F?style=for-the-badge" alt="Official Website"></a>
    <a href="mailto:support@suntekai.com"><img src="https://img.shields.io/badge/Support-support%40suntekai.com-0B1437?style=for-the-badge" alt="Support"></a>
  </p>

  <p>
    <a href="#supported-hardware--platforms">Hardware</a> &nbsp;&bull;&nbsp;
    <a href="#documentation-index--directory-layout">Docs &amp; Layout</a> &nbsp;&bull;&nbsp;
    <a href="#prebuilt-drivers-tools--downloads-github-releases">Downloads</a> &nbsp;&bull;&nbsp;
    <a href="#5-minute-quick-start">Quick Start</a> &nbsp;&bull;&nbsp;
    <a href="#troubleshooting--faq">FAQ</a> &nbsp;&bull;&nbsp;
    <a href="#documentation-changelog--support">Support</a>
  </p>

  <img src="https://raw.githubusercontent.com/SUNTEK-Official/.github/main/profile/assets/banner-product-family.webp" alt="SUNTEK Peripheral Integration Suite" width="100%">
</div>

---

## Supported Hardware & Platforms

| Hardware / Module | Interface / Protocol | Windows Support | Ubuntu / Linux Support | Notes |
| :--- | :--- | :--- | :--- | :--- |
| **SUNTEK Thermal Label Printer (XP Series)** | USB / TSPL | Complete driver suite (GitHub Releases) & install guide | — | Covers XP-20B through XP-T375U TSPL label models; do not use for MP80PC receipt printers |
| **SUNTEK 80 mm Thermal Receipt Printer (MP80PC)** | USB / ESC-POS | Standard ESC/POS raw printing (no TSPL driver) | One-click CUPS raw-queue script and usage guide | Pure ESC/POS receipt printer (USB VID:PID `0483:5743`); never install the XP-series TSPL driver |
| **SUNTEK Barcode Scanner** | USB HID (default) &middot; optional RS-232 / USB-COM | Plug-and-play HID + serial SDK (`.dll` / `.lib`) | Plug-and-play HID quick start | Serial mode is optional; default mode needs no driver |
| **SUNTEK RGB LED Controller** | USB serial (`115200 8N1`) | Serial HEX command reference | Serial HEX command reference | Fixed-color commands verified on hardware |

> [!NOTE]
> All subdirectories and sub-documents use lowercase `kebab-case` (for example, `printer/windows/` and `barcode-scanner/ubuntu/usb-hid-quick-start.md`). Only standardized root files use uppercase.

---

## Documentation Index & Directory Layout

```text
suntek-peripherals-docs/
├── printer/
│   ├── windows/                         # XP-series Windows driver installation guide (complete driver zip on Releases)
│   └── ubuntu/                          # MP80PC CUPS setup script & usage guide
├── barcode-scanner/
│   ├── windows/                         # USB HID & serial guides, SDK, C# sample
│   └── ubuntu/                          # USB HID plug-and-play guide
├── led/
│   ├── windows/                         # Serial HEX command reference
│   └── ubuntu/                          # Serial HEX command reference
├── CHANGELOG.md
├── LICENSE
├── THIRD_PARTY_NOTICES.md
└── README.md
```

### Complete Guide Index

- [`printer/windows/installation-guide.md`](./printer/windows/installation-guide.md) — Windows XP-series TSPL label printer driver installation and upgrade guide
- [`printer/ubuntu/README.md`](./printer/ubuntu/README.md) — Ubuntu CUPS raw-queue setup
- [`printer/ubuntu/setup-pos80.sh`](./printer/ubuntu/setup-pos80.sh) — one-click Ubuntu setup script (requires root)
- [`printer/ubuntu/setup-usage.md`](./printer/ubuntu/setup-usage.md) — running the setup script on another device
- [`barcode-scanner/windows/usb-hid-quick-start.md`](./barcode-scanner/windows/usb-hid-quick-start.md) — Windows plug-and-play scanner setup
- [`barcode-scanner/ubuntu/usb-hid-quick-start.md`](./barcode-scanner/ubuntu/usb-hid-quick-start.md) — Ubuntu plug-and-play scanner setup
- [`barcode-scanner/windows/serial-port-development-guide.md`](./barcode-scanner/windows/serial-port-development-guide.md) — serial SDK API reference
- [`barcode-scanner/windows/serial-port-test-guide.md`](./barcode-scanner/windows/serial-port-test-guide.md) — serial test tool instructions
- [`barcode-scanner/windows/sample-serial-csharp/`](./barcode-scanner/windows/sample-serial-csharp/) — clean C# serial sample source
- [`led/windows/led-control-sdk.md`](./led/windows/led-control-sdk.md) — Windows serial HEX command reference
- [`led/ubuntu/led-control-sdk.md`](./led/ubuntu/led-control-sdk.md) — Ubuntu serial HEX command reference

---

## Prebuilt Drivers, Tools & Downloads (GitHub Releases)

All `.exe` utilities, Windows driver installer suites, and large `.cab`/`.ddz` driver packages are
hosted on [GitHub Releases](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases)
under tag `v1.0.0` and are **never** stored in the Git source tree.

| Package / Asset | Target OS | Contents | Download Link |
| :--- | :--- | :--- | :--- |
| `suntek-printer-windows-driver-v1.0.0.zip` | Windows 10 / 11 (x64 & x86) | Complete XP-series TSPL label printer driver package (DriverWizard, INF/CAT/CAB/DDZ, TCP/IP port monitor DLLs, install guide, EULA) | [v1.0.0 Release](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases/tag/v1.0.0) |
| `suntek-barcode-scanner-test-tool-v1.0.0.zip` | Windows 10 / 11 | `SerialCOMTest.exe` test tool and `BarcodeScanner.dll` / `.lib` SDK | [v1.0.0 Release](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases/tag/v1.0.0) |
| `SHA256SUMS.txt` | All | SHA-256 checksums for every Release asset | [v1.0.0 Release](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases/tag/v1.0.0) |

Verify downloaded archives before running or deploying:

```powershell
# Windows PowerShell
Get-FileHash .\suntek-printer-windows-driver-v1.0.0.zip -Algorithm SHA256
```

```bash
# Linux / macOS
shasum -a 256 -c SHA256SUMS.txt
```

---

## 5-Minute Quick Start

1. **Windows XP-series TSPL label printer**: Download `suntek-printer-windows-driver-v1.0.0.zip` from
   [GitHub Releases](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases/tag/v1.0.0),
   verify its SHA-256 checksum, unpack it, run `DriverWizard.exe`, and follow
   [`printer/windows/installation-guide.md`](./printer/windows/installation-guide.md).
2. **Ubuntu 80 mm receipt printer (MP80PC)**: Run [`printer/ubuntu/setup-pos80.sh`](./printer/ubuntu/setup-pos80.sh) as root with the printer connected over USB; see [`printer/ubuntu/README.md`](./printer/ubuntu/README.md).
3. **Barcode scanner**: Plug in the USB cable; the scanner is recognized as a keyboard and scan results appear at the cursor. See [`barcode-scanner/windows/usb-hid-quick-start.md`](./barcode-scanner/windows/usb-hid-quick-start.md) or the [Ubuntu guide](./barcode-scanner/ubuntu/usb-hid-quick-start.md). Serial mode is optional.
4. **RGB LED controller**: Open the USB serial port at `115200 8N1`, send HEX frames with no spaces or line breaks. See [`led/windows/led-control-sdk.md`](./led/windows/led-control-sdk.md) or the [Ubuntu guide](./led/ubuntu/led-control-sdk.md).

---

## Troubleshooting & FAQ

<details>
<summary><b>Where are the .exe utilities and driver installer archives?</b></summary>
<p>All <code>.exe</code> tools, driver wizards, and large installer archives are published under <a href="https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases">GitHub Releases</a> with a verified <code>SHA256SUMS.txt</code> instead of bloating Git history.</p>
</details>

<details>
<summary><b>Windows reports the driver is not digitally signed. What should I do?</b></summary>
<p>You can safely ignore this message during installation; the driver has been tested with current Windows versions. If your organization enforces signed-driver policies, contact <a href="mailto:support@suntekai.com">support@suntekai.com</a> for a signed package.</p>
</details>

<details>
<summary><b>Does the barcode scanner need a driver or serial setup?</b></summary>
<p>No. By default it is a USB HID keyboard — plug it in and scan. Serial / USB-COM mode is only for applications that need programmatic control; see the serial development guide.</p>
</details>

<details>
<summary><b>How do I get technical assistance for hardware modules?</b></summary>
<p>Contact SUNTEK Technical Support at <a href="mailto:support@suntekai.com">support@suntekai.com</a> or visit <a href="https://www.suntekai.com">www.suntekai.com</a>.</p>
</details>

---

## Documentation, Changelog & Support

- **Developer Portal**: [`suntek-docs`](https://github.com/SUNTEK-Official/suntek-docs)
- **Release History**: [`CHANGELOG.md`](./CHANGELOG.md)
- **Third-Party Notices**: [`THIRD_PARTY_NOTICES.md`](./THIRD_PARTY_NOTICES.md)
- **Technical Support**: [support@suntekai.com](mailto:support@suntekai.com) &middot; [Open an Issue](https://github.com/SUNTEK-Official/suntek-peripherals-docs/issues/new/choose)
- **Security Disclosures**: See [`SECURITY.md`](https://github.com/SUNTEK-Official/.github/blob/main/SECURITY.md) or email [support@suntekai.com](mailto:support@suntekai.com)

---

<div align="center">
  <img src="https://raw.githubusercontent.com/SUNTEK-Official/.github/main/profile/assets/banner-about-factory.webp" alt="SUNTEK Smart Manufacturing Facility" width="100%">
  <br><br>
  <strong>SUNTEK Technology Co., Ltd.</strong> &mdash; <em>Smart Commercial Terminal Hardware Manufacturer Since 2010</em><br>
  <sub>80+ Export Countries &middot; 5,000,000+ Terminals Shipped &middot; 500+ ODM Projects &middot; ISO 9001 / ISO 14001 / ISO 27001 / CE / RoHS</sub><br>
  <sub><a href="https://www.suntekai.com">www.suntekai.com</a> &middot; Copyright &copy; 2026 SUNTEK Technology Co., Ltd. All rights reserved.</sub>
</div>
