# Windows Barcode Scanner Serial Port Test Guide

| Item | Details |
| :--- | :--- |
| **Target Hardware** | SUNTEK Barcode Scanner (optional RS-232 / USB-COM serial mode) |
| **Supported OS** | Windows 10 / 11 |
| **Interface / Protocol** | RS-232 / USB-COM virtual serial |
| **Prebuilt Tools & Drivers** | [GitHub Releases](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases) (`suntek-barcode-scanner-test-tool-v1.0.0.zip`) |

## Overview

This test guide covers the `SerialCOMTest` utility distributed as `suntek-barcode-scanner-test-tool-v1.0.0.zip` on [GitHub Releases](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases) (clean C# sample source is in [`./sample-serial-csharp/`](./sample-serial-csharp/)). It is provided for testing only; customers may develop their own application as needed.

It works with both RS232 serial ports and USB virtual serial ports. Note that in RS232 mode you must configure the serial parameters (baud rate, etc.) so that the tool and the scanner use identical serial settings.

## 1. Configuring the scanner's serial mode

This tool is used with both RS232 serial ports and USB virtual serial ports, but under RS232 you must configure the baud rate and other serial parameters so the tool and the scanner transmit consistently.

1. Restore the scanner to factory settings — scan the **Restore Factory Settings** barcode.
2. Switch the scanner interface to USB-COM or RS232 — scan the **USB COM** barcode or the **RS232** barcode.

## 2. Running the sample program

1. Put the sample program and the required DLL files in the same directory:
   - `BarcodeScanner.dll`
   - `SerialCOMTest.exe`
2. Launch `SerialCOMTest.exe`.

Main window controls: **Open Port** / **Close Port**, **Baud Rate**, **Trigger Decode** / **Stop Decode**, **Read Data**, and a **Send** button with a text box.

3. Select the scanner's COM port on the PC — you can find the exact port number in Device Manager (for example `Prolific USB-to-Serial Comm Port (COM34)`, `USB Serial Port (COM5)`, or `Communications Port (COM1)`).
4. If the scanner port is RS232, first confirm the scanner's baud rate, then set the same value in the sample program; enter the port number, click **Open Port**, and a success dialog confirms it opened. Under a virtual serial port you do not need to set the baud rate — just open the port directly.
5. Click **Trigger Decode** to make the scanner start scanning. After the scanner reads a code, click **Read Data** to see the scan result in the text box below.
6. You can also send commands from the text box to control the scanner. For example, sending `FF430D` queries whether the scanner is online; if it is online, `0615DEDF` appears in the output box. Enter the command and click **Send**.
