# LED Control Board · Windows Serial Port Guide

On Windows, controlling the LED board is simple: get the serial port settings right, then send the commands in **hex**. No SDK is needed — any serial terminal works (SSCOM, PuTTY, Tera Term, etc.).

## 1. Serial port settings

Connect the board to the PC (USB-to-serial adapter), find its COM port in Device Manager, and open it with these exact parameters:

```text
Baud rate:    115200
Data bits:    8
Parity:       None
Stop bits:    1
Flow control: None
```

That is 115200 8N1. Wire TX → P3.0/RXD, RX → P3.1/TXD, and tie GND to P9 GND (common ground is mandatory). The MCU is powered from 3.3V (pin 2); the WS2812B strip uses its own 5V supply, with all grounds shared.

## 2. Sending commands — IMPORTANT

- Set the serial terminal to **HEX send** (hexadecimal transmit) and HEX display.
- Send raw bytes only — **no spaces, no carriage return, no line feed**. A single frame is one continuous hex string.
- For example, type `AA55010104FF0000FF8B85` and click Send; do not add a trailing newline.

A query frame only reads information and does not light the LEDs. To light them, send a `SET_STATIC` (`CMD=01`) or `SET_EFFECT` (`CMD=02`) frame.

## 3. Verified working commands (fixed color)

These frames have been tested and confirmed working. Send in hex:

```text
Red 100%:    AA55010104FF0000FF8B85
Green 100%:  AA5501010400FF00FF8BA1
Blue 100%:   AA550101040000FFFFFA61
White 100%:  AA55010104FFFFFFFFFA45
White 50%:   AA55010104FFFFFF80BBA5
Orange 78%:  AA55010104FF8000C8CBBB
```

The payload is `R G B BRIGHTNESS` (brightness `00`–`FF`). The firmware converts the protocol's `R G B` to the WS2812B's `G R B` order internally — do not swap the channels yourself. `SET_STATIC` switches to fixed-color mode and turns the display on; brightness `00` means off.

Expected successful reply to a `SET_STATIC` frame:

```text
Send:  AA55010104FFFFFFFFFA45
Reply: AA55018101005060
```

## 4. Other useful commands

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

## 5. Frame format (reference)

```text
AA 55 VER CMD LEN PAYLOAD CRC16_L CRC16_H
```

- Header `AA 55`, `VER = 01`.
- CRC16 (Modbus, polynomial `0xA001`) is computed over `VER + CMD + LEN + PAYLOAD`; send the low byte first, then the high byte.
- Reply CMD = request CMD + `0x80`; a successful reply has a one-byte payload `00`.
- If you only use the verified frames in this guide, you do not need to calculate CRC yourself.

## 6. Troubleshooting

| Symptom | Check |
|---|---|
| No reply / no light | Correct COM port; 115200 8N1; GND connected; MCU has 3.3V |
| Garbage reply | HEX send and HEX display both enabled |
| Command ignored | No spaces / CR / LF appended to the hex string |
| Wrong colors | Do not swap R/G/B — the firmware handles GRB order |
