# Installing Windows Printer Drivers (XP-Series TSPL Label Printers)

| Item | Details |
| :--- | :--- |
| **Target Hardware** | SUNTEK XP-Series TSPL Thermal Label Printers (`XP-20B` through `XP-T375U`) |
| **Supported OS** | Windows 10 / 11 (x64 & x86) |
| **Interface / Protocol** | USB / LPT / TCP-IP (TSPL) |
| **Prebuilt Driver Package** | `suntek-printer-windows-driver-v1.0.0.zip` ([GitHub Releases](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases)) |

## Contents

- [Introduction](#introduction)
- [Installing printer drivers](#installing-printer-drivers)
- [Installing the TCP/IP port monitor](#installing-the-tcpip-port-monitor)
- [Upgrading printer drivers](#upgrading-printer-drivers)
- [Removing printer drivers](#removing-printer-drivers)
- [Troubleshooting printing problems](#troubleshooting-printing-problems)

> [!NOTE]
> The complete driver package (`DriverWizard.exe`, INF/CAT/CAB/DDZ files, and TCP/IP port monitor DLLs) is distributed exclusively through
> [GitHub Releases](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases),
> not from the Git source tree. Download and unpack `suntek-printer-windows-driver-v1.0.0.zip`
> before following this guide.
>
> For the **SUNTEK MP80PC** 80 mm ESC/POS thermal receipt printer, do not install this TSPL label-printer driver; see [`../ubuntu/README.md`](../ubuntu/README.md).

## Introduction

This guide explains how to install and update the Windows printer drivers for SUNTEK XP-series TSPL thermal label printers (`XP-20B` through `XP-T375U`). After installation you can consult the help file bundled with the driver package for
answers to frequently asked questions and for in-depth troubleshooting.

To open the help file in Windows 10 or 11, right-click the printer on **Devices and Printers**,
choose **Printer properties** or **Printing preferences**, then click **Help** in the bottom-right
corner of the dialog.

The driver package includes the SUNTEK Driver Wizard, which is the recommended way to install,
remove, and upgrade drivers. The standard Windows "Add a Device" wizard does not always upgrade
existing drivers correctly.

## Installing printer drivers

1. Turn off the printer, connect the appropriate cable, then turn the printer back on.
2. If the printer supports Plug and Play and is connected over USB or parallel, Windows may detect
   it automatically and offer a driver. Click **Cancel** and do not install through that wizard.
3. In File Explorer, open the unpacked driver directory and double-click `DriverWizard.exe`.
4. Select **Install printer drivers**, then click **Next**.
5. Select the connection type, then click **Next**.
6. Complete the printer settings as prompted by the wizard.

> [!IMPORTANT]
> During installation Windows may display a message stating that the driver is not digitally
> signed. You can safely ignore this message — this driver has been tested with all current
> Windows versions. If your organization enforces signed-driver policies, contact
> [support@suntekai.com](mailto:support@suntekai.com) for a signed package.

## Installing the TCP/IP port monitor

Some printer models support TLS-encrypted communication. Their drivers include a TCP/IP port
monitor used to access and configure TLS settings. You can install it with the SUNTEK Driver
Wizard or through Windows Control Panel.

### Using the Driver Wizard

1. On the wizard's **Install Components** page, select the SUNTEK TCP/IP Port checkbox, then click **Next**.
   This component is available only for printers that support TLS or use non-standard port numbers.
2. On the **Specify Port** page, click **Create Port**.
3. In the **Create Port** dialog, select the SUNTEK TCP/IP Port, then click **New Port**.
4. In the **Add Port** dialog, enter a name for the port, then click **OK**.
5. In the **Configure Port** dialog, enter the printer host name or IP address and port number,
   optionally enable **Transport Layer Security (TLS)**, then click **OK**.
6. Finish the remaining printer settings in the wizard.

### Using Control Panel

1. Download and unpack the driver package for your printer.
2. On **Devices and Printers**, select any printer, then click **Print server properties**.
3. On the **Ports** tab, click **Change Port Settings**, then **Add Port**.
4. In the **Printer Ports** dialog, click **New Port Type**, then **Browse**.
5. Locate and open the printer's `.inf` file, which contains the port monitor information.
6. In the **Installing Port Monitor** dialog, click **OK**.
7. In the **Printer Ports** dialog, select the SUNTEK TCP/IP Port, then click **New Port**.
8. In the **Add Port** dialog, enter a name for the port, then click **OK**.
9. In the **Configure Port** dialog, enter the printer host name or IP address and port number, then click **OK**.
10. Close the **Printer Ports** dialog and the **Print Server Properties** page.

After installation the TCP/IP port appears on the **Ports** tab of the printer properties, where
you can add, delete, or configure it.

## Upgrading printer drivers

Use the SUNTEK Driver Wizard to upgrade drivers. Upgrading rather than removing preserves your
driver settings. Upgrading through the Windows "Add a Device" wizard is not recommended because it
may not update all driver files correctly.

1. Open the unpacked driver directory and double-click `DriverWizard.exe`.
2. Select **Upgrade printer drivers**, then click **Next**.
3. Select the printers to upgrade, then click **Next**.
4. Click **Finish**.

## Removing printer drivers

1. Open the unpacked driver directory and double-click `DriverWizard.exe`.
2. Select **Remove printer drivers**, then click **Next**.
3. Select the removal method, then click **Next**.
4. Verify the items to remove, click **Next**, then **Finish**.

## Troubleshooting printing problems

The help file bundled with the driver includes a **Technical Support** section with a
**Troubleshooting Printing Problems** sub-section listing critical settings to check and printing
tests that resolve most issues. Open it as described in [Introduction](#introduction).

For further assistance, contact [support@suntekai.com](mailto:support@suntekai.com) or visit
[https://www.suntekai.com](https://www.suntekai.com).
