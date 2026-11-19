# LED Control Board · Ubuntu Serial Port Guide

On Ubuntu, controlling the LED board is simple: get the serial port settings right, then send the commands in **hex**. No SDK is needed — any serial terminal or a one-line shell command works (`minicom`, `screen`, `printf`, or Python).

## 1. Find the serial device

Plug in the USB-to-serial adapter and identify the device node:

```bash
dmesg | tail -20          # look for "ttyUSB" / "ttyACM"
ls -l /dev/ttyUSB* /dev/ttyACM* 2>/dev/null
```

Typically it is `/dev/ttyUSB0` (or `/dev/ttyACM0`). If your user cannot open the port, add it to the `dialout` group and log in again:

```bash
sudo usermod -aG dialout $USER
```

## 2. Serial port settings

Open the port with these exact parameters:

```text
Baud rate:    115200
Data bits:    8
Parity:       None
Stop bits:    1
Flow control: None
```

That is 115200 8N1. With `stty`:

```bash
stty -F /dev/ttyUSB0 115200 cs8 -parenb -cstopb -ixon -ixoff -crtscts raw -echo
```

Wiring: TX → P3.0/RXD, RX → P3.1/TXD, and tie GND to P9 GND (common ground is mandatory). The MCU is powered from 3.3V (pin 2); the WS2812B strip uses its own 5V supply, with all grounds shared.

## 3. Sending commands — IMPORTANT

- Send raw bytes in **hex** — **no spaces, no carriage return, no line feed**. A single frame is one continuous hex string.
- Disable any "add newline on send" option in your terminal (`minicom`: `Ctrl-A O` → Serial port setup → turn off line-wrap/CR-NL translation; `screen`: do not press Enter after typing).

The most reliable way from the shell is `printf` (bash understands `\xNN` escapes):

```bash
printf '\xAA\x55\x01\x01\x04\xFF\x00\x00\xFF\x8B\x85' > /dev/ttyUSB0
```

Or with Python (clearest, no escaping surprises):

```bash
python3 -c "open('/dev/ttyUSB0','wb').write(bytes.fromhex('AA55010104FF0000FF8B85'))"
```

To read the reply in parallel, open a second terminal:

```bash
cat /dev/ttyUSB0 | xxd -p        # shows hex bytes as they arrive
```

A query frame only reads information and does not light the LEDs. To light them, send a `SET_STATIC` (`CMD=01`) or `SET_EFFECT` (`CMD=02`) frame.

## 4. Verified working commands (fixed color)

These frames have been tested and confirmed working. Send in hex:

```text
Red 100%:    AA55010104FF0000FF8B85
Green 100%:  AA5501010400FF00FF8BA1
Blue 100%:   AA550101040000FFFFFA61
White 100%:  AA55010104FFFFFFFFFA45
White 50%:   AA55010104FFFFFF80BBA5
Orange 78%:  AA55010104FF8000C8CBBB
```

Quick send helper — paste a frame and it writes raw bytes to the port:

```bash
send_led() { python3 -c "import sys; open('/dev/ttyUSB0','wb').write(bytes.fromhex(sys.argv[1]))" "$1"; }
send_led AA55010104FFFFFFFFFA45      # White 100%
```

The payload is `R G B BRIGHTNESS` (brightness `00`–`FF`). The firmware converts the protocol's `R G B` to the WS2812B's `G R B` order internally — do not swap the channels yourself. `SET_STATIC` switches to fixed-color mode and turns the display on; brightness `00` means off.

Expected successful reply to a `SET_STATIC` frame:

```text
Send:  AA55010104FFFFFFFFFA45
Reply: AA55018101005060
```

## 5. Other useful commands

```text
Set LED count to 11:        AA550104020B00BE00
Set LED count to 60:        AA550104023C00A830
Rainbow flow (mode 2):      AA5501020702000000FF80000B74
Cyan trail (mode 3):        AA550102070300FFFFC84000DE7A
Rainbow back-and-forth (4): AA5501020704000000FF8002ECB5
Power off:                  AA5501070100B189
Power on:                   AA55010701017049
Save as power-on default:   AA55010A0026A0
Read info (GET_INFO):       AA550109002650
```

Send the actual number of LEDs installed in the product; the firmware default is 11.

## 6. Frame format (reference)

```text
AA 55 VER CMD LEN PAYLOAD CRC16_L CRC16_H
```

- Header `AA 55`, `VER = 01`.
- CRC16 (Modbus, polynomial `0xA001`) is computed over `VER + CMD + LEN + PAYLOAD`; send the low byte first, then the high byte.
- Reply CMD = request CMD + `0x80`; a successful reply has a one-byte payload `00`.
- If you only use the verified frames in this guide, you do not need to calculate CRC yourself.

## 7. Troubleshooting

| Symptom | Check |
|---|---|
| `/dev/ttyUSB0` not found | Re-plug adapter; `dmesg | tail`; try `/dev/ttyACM0` |
| Permission denied | Add user to `dialout` group, then log out and back in |
| No reply / no light | Correct device node; 115200 8N1; GND connected; MCU has 3.3V |
| Garbage reply | Port in `raw` mode; no CR/NL translation; read with `xxd -p` |
| Command ignored | No spaces / CR / LF appended to the hex string |
| Wrong colors | Do not swap R/G/B — the firmware handles GRB order |
