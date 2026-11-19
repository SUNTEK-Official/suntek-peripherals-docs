# Barcode Scanner · USB HID Quick Start (Plug and Play)

By default the scanner works as a **USB HID keyboard**. Plug in the USB cable and it is recognized as a keyboard — no driver, no serial-port configuration, and no SDK are required. The scanned barcode is typed wherever the text cursor is, just like keyboard input.

> The serial-port / USB-COM material in this folder is for the **optional** RS232 or virtual-COM mode used by custom applications. For normal use you do not need it — just plug in and scan.

## 1. Connect

1. Plug the scanner's USB cable into the PC.
2. Confirm the device is detected:

```bash
lsusb                 # look for a Suntek HID / keyboard device
dmesg | tail -20      # "USB HID device" / "input: ... keyboard"
```

No driver installation is needed — Ubuntu includes the standard USB HID keyboard driver.

## 2. Test scanning

1. Open any text editor (for example `gedit`, or a browser address bar).
2. Click into the text area so it has keyboard focus.
3. Scan a barcode. The decoded data appears at the cursor. By default a **Carriage Return (Enter)** is appended, so the cursor moves to the next line after each scan.

That is the whole normal workflow — no software to run.

## 3. Reading the data in your own program

Because the scanner behaves as a keyboard, any application that accepts keyboard input receives the barcode automatically. For example, in a terminal:

```bash
cat                    # scan a code -> its text is typed into the terminal
```

In a GUI application, put the focus in an input field and scan. In a text/UI framework, listen for normal keyboard events (the barcode arrives as a burst of key presses followed by Enter).

## 4. Optional: change settings with configuration barcodes

Scanner settings are changed by scanning configuration barcodes from the scanner's configuration sheet — not by software. Common ones:

- **Restore Factory Settings** — returns the scanner to the default USB HID mode (useful if it was previously switched to another interface).
- **Suffix type** — choose what is appended after each scan: none, CR, CR+LF, LF, `&`, `|`, comma, and combinations with CR. Default is CR (Enter).
- **Scan mode** — auto-sense (default), continuous, or trigger-button.
- **Case / prefix** — uppercase conversion or a custom prefix if needed by your application.

If the scanner does not type anything, scan **Restore Factory Settings** first to return it to the default HID mode.

## 5. Optional: serial / USB-COM mode

Only if your application needs programmatic control over a serial port, switch the scanner to USB-COM or RS232 by scanning the corresponding configuration barcode. In that mode you open a COM port and use the DLL/API described in the Windows development guide (`serial-port-development-guide.md`); under USB-COM no baud-rate setup is needed, while RS232 requires matching serial parameters on both sides.

## 6. Troubleshooting

| Symptom | Check |
|---|---|
| Device not listed by `lsusb` | Re-plug cable; try another USB port; check the cable |
| Scanning does nothing | Make sure a text field has keyboard focus; scan Restore Factory Settings to return to HID mode |
| Data goes to the wrong window | Click the intended input field first — HID output follows the keyboard focus |
| No line break after a scan | Suffix is set to "none"; scan the CR / CR+LF suffix configuration barcode |
| Characters garbled / wrong case | Check the scanner's keyboard layout / case configuration barcode |
| Need programmatic port access | Switch to USB-COM or RS232 mode and use the serial-port development guide |
