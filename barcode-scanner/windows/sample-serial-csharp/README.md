# SUNTEK Barcode Scanner - Windows Serial Sample (C#)

A minimal Windows Forms sample that demonstrates driving a SUNTEK barcode scanner in its optional RS-232 / USB-COM serial mode through the `BarcodeScanner.dll` native SDK.

> **Default connection is USB HID (keyboard emulation) and needs no software.** Use this sample only when you explicitly switch the scanner to serial mode. See [`../serial-port-development-guide.md`](../serial-port-development-guide.md) for the full API reference and [`../usb-hid-quick-start.md`](../usb-hid-quick-start.md) for the default plug-and-play mode.

## Requirements

- Windows 10 / 11
- Visual Studio with the .NET Framework 4 (Client Profile) targeting pack
- `BarcodeScanner.dll` and `BarcodeScanner.lib` from the SDK (placed next to the built executable)

## What the sample demonstrates

| Button | SDK call | Purpose |
| :--- | :--- | :--- |
| Open Port | `OpenPort` + `SetPortBoudRate` / `SetPortDataBits` / `SetPortParity` / `SetPortStopBits` | Open a COM port at `115200, N, 8, 1` |
| Close Port | `ClosePort` | Close the open COM port |
| Baud Rate | `SetPortBoudRate` | Switch between 9600 and 115200 baud |
| Trigger Decode / Stop Decode | `Trigger` / `UnTrigger` | Start or stop scanning |
| Read Data | `ReadBarcode` | Read the decoded barcode bytes |
| Send | `SendData` + `ReadBarcode` | Send a hex command and print the hex response |

Enter the COM port number (for example `12`) in the small input box, set the baud rate if needed, then click **Open Port**.

## Build

Open `SerialCOMTest.sln` in Visual Studio and build the `Release|x86` configuration. Copy `BarcodeScanner.dll` into the output folder before running.

The prebuilt `SerialCOMTest.exe` test tool is distributed through
[GitHub Releases](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases) rather than the Git source tree.

## Serial parameters

```text
Baud rate: 115200
Data bits: 8
Parity:    None
Stop bits: 1
Flow control: None
```

## Technical support

support@suntekai.com | https://www.suntekai.com
