# Windows Barcode Scanner Serial Port Development Guide

| Item | Details |
| :--- | :--- |
| **Target Hardware** | SUNTEK Barcode Scanner (optional RS-232 / USB-COM serial mode) |
| **Supported OS** | Windows 10 / 11 (x64 & x86; x86 native SDK) |
| **Interface / Protocol** | RS-232 / USB-COM virtual serial (`BarcodeScanner.dll`, `BarcodeScanner.lib`) |
| **Prebuilt Tools & Drivers** | [GitHub Releases](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases) (`suntek-barcode-scanner-test-tool-v1.0.0.zip`) |

## Overview

This guide covers Windows barcode recognition over the serial port.

The repository includes `sdk/BarcodeScanner.dll`, `sdk/BarcodeScanner.lib`, and clean C# sample source code in [`./sample-serial-csharp/`](./sample-serial-csharp/). The prebuilt `SerialCOMTest.exe` utility is distributed through [GitHub Releases](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases).

## 1. APIs provided in the DLL

Functions are generally used as shown below — for example, restoring factory settings:

```cpp
int result = DefaultControl(GlobalFunAndVar.fd);
if (result > 0)
{
    // configured successfully
}
```

### 1. Restore factory settings

```cpp
int DefaultControl(PORT com_port);
```
- **Function:** Restore factory settings.
- **Return:** positive value on success; non-positive on failure.
- **Parameters:** `com_port` — serial port handle.

### 2. Barcode suffix setting

```cpp
int EditTailType(PORT com_port, int type);
```
- **Function:** Control the suffix appended to the scanner's output data.
- **Return:** positive value on success; non-positive on failure.
- **Parameters:** `com_port` — serial port handle; `type` — suffix type:
  - `0` — no suffix
  - `1` — add carriage return (CR)
  - `2` — add CR + LF
  - `3` — add line feed (LF)
  - `4` — add `&` suffix
  - `5` — add `&` + CR suffix
  - `6` — add `|` suffix
  - `7` — add `|` + CR suffix
  - `8` — add comma suffix
  - `9` — add comma + CR suffix

Example:

```csharp
string str = this.box3.Text;
if (str.Length == 0)
    return;
int number = int.Parse(str);
int result = EditTailType(GlobalFunAndVar.fd, number);
```

### 3. Scan mode setting

```cpp
int EditTriggerMode(PORT com_port, int type);
```
- **Function:** Switch the scanning mode.
- **Return:** positive value on success; non-positive on failure.
- **Parameters:** `com_port` — serial port handle; `type`: `0` = default auto-sense mode, `1` = continuous scan mode, `2` = trigger-button mode.

Example:

```csharp
int result = EditTriggerMode(GlobalFunAndVar.fd, type);
if (result > 0)
{
    // configured successfully: type
}
```

### 4. Port control

> **Note:** Under a USB-COM interface, just use the open-port function — there is no need to configure serial parameters. Under RS232 you must set the baud rate, stop bits and parity.

```cpp
PORT OpenPort(int idx);
```
- **Function:** Open the serial port at the given index.
- **Return:** port handle on success; `NULL` on failure.
- **Parameters:** `idx` — port index.

```csharp
string str = this.box1.Text;
if (str.Length == 0)
    return;
GlobalFunAndVar.fd = OpenPort((IntPtr)(int.Parse(str)));
```

```cpp
void ClosePort(PORT com_port);
```
- **Function:** Close an already-open serial port.
- **Return:** none.
- **Parameters:** `com_port` — serial port handle.

```csharp
ClosePort(GlobalFunAndVar.fd);
```

```cpp
int SetPortBoudRate(PORT com_port, int rate);
```
- **Function:** Set the baud rate of the given serial port.
- **Return:** `TRUE` on success; `FALSE` on failure.
- **Parameters:** `com_port` — serial port handle; `rate` — baud rate value.

```cpp
int SetPortDataBits(PORT com_port, int databits);
```
- **Function:** Set the data bits of the given serial port.
- **Return:** `TRUE` on success; `FALSE` on failure.
- **Parameters:** `com_port` — serial port handle; `databits` — number of data bits.

```cpp
int SetPortStopBits(PORT com_port, int stopbits);
```
- **Function:** Set the stop bits of the given serial port.
- **Return:** `TRUE` on success; `FALSE` on failure.
- **Parameters:** `com_port` — serial port handle; `stopbits` — number of stop bits.

```cpp
int SetPortParity(PORT com_port, int parity);
```
- **Function:** Set the parity of the given serial port.
- **Return:** `TRUE` on success; `FALSE` on failure.
- **Parameters:** `com_port` — serial port handle; `parity` — parity type.

```cpp
int GetPortBoudRate(PORT com_port);
```
- **Function:** Get the baud rate of the given serial port.
- **Return:** baud rate value.
- **Parameters:** `com_port` — serial port handle.

```cpp
int GetPortDataBits(PORT com_port);
```
- **Function:** Get the data bits of the given serial port.
- **Return:** number of data bits.
- **Parameters:** `com_port` — serial port handle.

```cpp
int GetPortStopBits(PORT com_port);
```
- **Function:** Get the stop bits of the given serial port.
- **Return:** number of stop bits.
- **Parameters:** `com_port` — serial port handle.

```cpp
int GetPortParity(PORT com_port);
```
- **Function:** Get the parity of the given serial port.
- **Return:** parity type.
- **Parameters:** `com_port` — serial port handle.

Example configuration:

```csharp
SetPortBoudRate(GlobalFunAndVar.fd, baudRate);
SetPortDataBits(GlobalFunAndVar.fd, 8);
SetPortParity(GlobalFunAndVar.fd, 'n');
SetPortStopBits(GlobalFunAndVar.fd, 1);
```

### 5. Trigger functions

```cpp
int Trigger(PORT com_port);
```
- **Function:** Trigger the device associated with the serial port.
- **Return:** number of bytes of the trigger command sent (default 3).
- **Parameters:** `com_port` — serial port handle.

```csharp
private void Trigger_button(object sender, EventArgs e)
{
    Trigger(GlobalFunAndVar.fd);
}
```

```cpp
int UnTrigger(PORT com_port);
```
- **Function:** Cancel triggering of the device associated with the serial port.
- **Return:** number of bytes of the stop-trigger command sent (default 3).
- **Parameters:** `com_port` — serial port handle.

```csharp
private void UnTrigger_button(object sender, EventArgs e)
{
    UnTrigger(GlobalFunAndVar.fd);
}
```

### 6. Read/write functions

```cpp
int ReadBarcode(PORT com_port, char * revivedata, int len, DWORD TimeOut);
```
- **Function:** Read barcode data from the serial port.
- **Return:** number of bytes read.
- **Parameters:** `com_port` — serial port handle; `revivedata` — receive buffer; `len` — buffer length; `TimeOut` — timeout.

```csharp
private void ReadBarcode_button(object sender, EventArgs e)
{
    this.box.Text = "";
    ReadBarcode(GlobalFunAndVar.fd,
        GlobalFunAndVar.revivedata, GlobalFunAndVar.len, GlobalFunAndVar.timeout);
    string str = System.Text.Encoding.Default.GetString(GlobalFunAndVar.revivedata);
    this.box.Text += str;
}
```

```cpp
int SendData(PORT com_port, const char * data);
```
- **Function:** Send data over the serial port.
- **Return:** number of bytes sent.
- **Parameters:** `com_port` — serial port handle; `data` — data to send.

```csharp
string inputMessage = this.box2.Text;
if (string.IsNullOrEmpty(inputMessage))
    return;
byte[] messageBytes = StringToByteArray(inputMessage);
int result = SendData(GlobalFunAndVar.fd, messageBytes);
```

```cpp
int ReciveData(PORT com_port, char * data, int len, int TimeOut);
```
- **Function:** Receive data from the serial port.
- **Return:** number of bytes received.
- **Parameters:** `com_port` — serial port handle; `data` — receive buffer; `len` — buffer length; `TimeOut` — timeout.

## 2. Calling the DLL from C#

1. Import the DLL and the functions you want via `DllImport`. For example, import `BarcodeScanner.dll` and the `DefaultControl` function:

```csharp
[DllImport("BarcodeScanner.dll", EntryPoint = "DefaultControl",
    CallingConvention = CallingConvention.Cdecl)]
public static extern int DefaultControl(IntPtr com_port);
```

2. Define a component and link it to the function:

```csharp
Button DefaultControl = new Button();
DefaultControl.Location = new Point(50, 100);
DefaultControl.Size = new Size(80, 25);
DefaultControl.Text = "Restore Factory";
DefaultControl.Click += DefaultControl_button;
this.Controls.Add(DefaultControl);
```

3. Implement the function's behavior:

```csharp
private void DefaultControl_button(object sender, EventArgs e)
{
    int result = DefaultControl(GlobalFunAndVar.fd);
    if (result > 0)
    {
        MessageBox.Show("Factory settings restored!", "Success",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    else
    {
        MessageBox.Show("Factory reset control failed!", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
```

## 3. Build and deployment

Open `sample-serial-csharp/SerialCOMTest.sln` in Visual Studio and build the `Release|x86` configuration. Copy `BarcodeScanner.dll` next to `SerialCOMTest.exe` in `bin\Release\` before running, or download the prebuilt `suntek-barcode-scanner-test-tool-v1.0.0.zip` package from [GitHub Releases](https://github.com/SUNTEK-Official/suntek-peripherals-docs/releases).
