# Third-Party Software Notices

Copyright (c) 2026 SUNTEK Technology Co., Ltd.

Some driver packages distributed from this repository's GitHub Releases are built on third-party
hardware-module driver components. These components are supplied as prebuilt, signed binaries and
are included to preserve Windows driver signing integrity and correct device operation. SUNTEK
Technology Co., Ltd. verifies before release that no unlicensed strong-copyleft (AGPL / GPL)
runtime components are included in commercial distributions.

For all products covered by this repository, technical support is provided exclusively by SUNTEK
at [support@suntekai.com](mailto:support@suntekai.com) and
[https://www.suntekai.com](https://www.suntekai.com).

## Bundled third-party driver components

| Component | Type | License / Basis | Usage Scope |
| :--- | :--- | :--- | :--- |
| Thermal printer driver core (INF/CAT/CAB/DDZ package) | Prebuilt signed Windows driver package | Supplied under the driver vendor's redistribution terms | Enables Windows installation of SUNTEK XP-series TSPL thermal label printers |
| TCP/IP port monitor (Win32/x64) | Native Windows DLL | Bundled with the printer driver package | Provides standard and TLS-enabled TCP/IP printing ports |
| Barcode scanner serial SDK (`BarcodeScanner.dll`, `.lib`) | Native library | Supplied with SUNTEK scanner hardware | Optional RS-232 / USB-COM serial control of SUNTEK barcode scanners |

No third-party support channel, upstream customer-service address, or vendor contact route is
exposed in the public documentation of this repository. Do not contact the original module vendor
directly; all customer support is handled by SUNTEK Technology Co., Ltd.
