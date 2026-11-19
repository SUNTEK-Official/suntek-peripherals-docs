# Suntek MP80PC Thermal Receipt Printer · Ubuntu Setup & Usage Guide

## 1. Overview

| Item | Details |
|------|---------|
| Printer | Suntek MP80PC, 80mm thermal receipt printer, **ESC/POS** command set |
| Connection | USB printer-class interface (e.g., kernel node `/dev/usb/lp1`) |
| Tested Environment | Ubuntu 24.04 LTS, CUPS 2.4.7 (example print server IP: `192.0.2.10`) |
| CUPS queue | `suntek-pos80` (system default, raw queue) |
| Print device | `usbraw:/dev/usb/lp1` (custom backend) |

> **Note:** the serial path is **unsupported** on this unit. The CH340 port (`/dev/ttyUSB0`) on this board is **not wired to the printer** — no baud rate produces any output. Use USB only.

## 2. Architecture & Setup Summary

Run [`./setup-pos80.sh`](./setup-pos80.sh) (as root) to apply the recommended CUPS configuration automatically:

- Sets the raw queue `suntek-pos80` as the system default printer.
- Installs a custom CUPS backend `/usr/lib/cups/backend/usbraw` that writes CUPS job data directly to the detected `/dev/usb/lpN` node.
- Avoids mismatched queues (for example a `POS-80` queue with an unrelated printer PPD) that cause wrong output or USB resets.
- Persists across both CUPS and machine restarts.

### Why a custom backend is needed (background)

CUPS' stock `usb://` backend probes the device with an IEEE-1284 Device-ID request on open. This printer's firmware cannot handle that probe: its USB controller resets and re-enumerates repeatedly (kernel log shows `usblp` removed/re-added every ~5 seconds), and jobs hang at *"Waiting for printer to become available"* forever.

The `usbraw` backend skips the probe entirely — it only `dd`s the job bytes into `/dev/usb/lp1`. It also correctly reads the job data from the CUPS spool file (argv[6]); CUPS does **not** feed data to backends on stdin for a local spooled job.

Backend runtime log: `/var/log/usbraw.log` (one line per job: byte count + device write result).

## 3. Everyday printing

### 3.1 From the command line

```bash
# Print an existing ticket file (plain text or raw ESC/POS bytes)
lp -d suntek-pos80 -o raw ticket.bin

# Always keep -o raw, otherwise CUPS rasterizes it as a document
echo "Hello" | lp -d suntek-pos80 -o raw

# With ESC/POS init (\x1b\x40 = ESC @)
printf '\033@Hello\n\n\n' > /tmp/ticket.bin
lp -d suntek-pos80 -o raw /tmp/ticket.bin
```

### 3.2 Non-ASCII text & encoding

The printer's built-in font uses **GB18030 / GBK**. UTF-8 bytes sent directly print as garbage or `?`. Transcode before submitting:

```bash
iconv -f UTF-8 -t GB18030 ticket.txt | lp -d suntek-pos80 -o raw
```

In code, encode the string as GB18030 (or GBK) before handing it to the queue.

### 3.3 A reliable one-shot command

```bash
{
  printf '\033@'                               # ESC @ init
  printf 'TEST LINE\n'
  printf 'Non-ASCII text test\n' | iconv -f UTF-8 -t GB18030
  printf '\n\n\n'                              # feed paper
} > /tmp/ticket.bin
lp -d suntek-pos80 -o raw /tmp/ticket.bin
```

## 4. Printing from applications

Submit a *raw* job to the CUPS queue `suntek-pos80`. No printer SDK is required.

Python example:

```python
import subprocess

def print_ticket(text: str) -> None:
    data = b"\x1b\x40" + text.encode("gb18030") + b"\n\n\n"
    subprocess.run(
        ["lp", "-d", "suntek-pos80", "-o", "raw"],
        input=data, check=True,
    )
```

Node.js example:

```js
const { execFile } = require("child_process");

function printTicket(text) {
  const data = Buffer.concat([
    Buffer.from([0x1b, 0x40]),                    // ESC @
    Buffer.from(text, "gb18030"),
    Buffer.from("\n\n\n"),
  ]);
  return new Promise((resolve, reject) => {
    const child = execFile("lp", ["-d", "suntek-pos80", "-o", "raw"], (e) =>
      e ? reject(e) : resolve()
    );
    child.stdin.end(data);
  });
}
```

Key points:

- **Always pass `-o raw`.** Without it CUPS treats binary data as a document and mangles it.
- Encode non-ASCII text as **GB18030/GBK**.
- Receipt width, cutter, black-mark detection etc. are controlled by the **ESC/POS commands** in the data, not by a driver.

## 5. Troubleshooting

| Symptom | Likely cause | Fix |
|---------|--------------|-----|
| Garbage / blank output | `-o raw` omitted, or text not encoded as GB18030 | Follow section 3/4 |
| Job hangs (usblp keeps re-enumerating in dmesg) | A `usb://` queue is being used | Remove that queue; use `suntek-pos80` (usbraw backend) only |
| Nothing prints at all | Printer off / no paper / cable | Verify the hardware path: `printf '\033@TEST\n\n' > /dev/usb/lp1` — paper out proves the link is fine |
| Wondering whether data reached the device | Backend log | `cat /var/log/usbraw.log` — look for `payload=xx bytes -> device` and `DD_RC=0` |

Useful diagnostics (run as root):

```bash
lpstat -a                 # queue status
lpstat -v suntek-pos80         # what the queue points to
ls -l /dev/usb/lp1        # is the USB printing node present
lsusb                     # expect 0483:5743 Suntek USB Printer
cat /var/log/usbraw.log   # custom backend log
tail -f /var/log/cups/error_log   # CUPS daemon log
dmesg | grep -i usblp     # detect the USB reset loop
```

## 6. Do's and Don'ts

1. **Don't** create or use a `usb://Printer/...` queue — it fires the Device-ID probe and resets the printer.
2. **Don't** rely on serial `/dev/ttyUSB0` — it is not connected to the printer.
3. **Don't** use the Windows XP-series TSPL label-printer driver package for this model; the Suntek MP80PC is an ESC/POS receipt printer and does not use TSPL.
4. Keep the default queue `suntek-pos80`; submit jobs from the service account as shown above.
5. **Don't** delete `/usr/lib/cups/backend/usbraw` or strip its 755 permissions — the queue stops working without it.

## 7. Deploying / migrating to another Ubuntu machine

### 7.1 Plugging the printer directly into another Ubuntu (raw method)

**No "driver" installation is required** — these ESC/POS receipt printers have no official Linux driver package. But the target machine must recreate two things:

1. The `usbraw` backend (otherwise CUPS' stock `usb://` backend fires the IEEE-1284 probe, the firmware resets the USB port, and jobs hang);
2. The raw queue `suntek-pos80`.

Run the following script as **root** (dependencies are only CUPS and gzip, both stock):

```bash
# 1) Create the custom backend: dump CUPS jobs straight into /dev/usb/lp1
cat > /usr/lib/cups/backend/usbraw <<'EOF'
#!/bin/bash
exec >> /var/log/usbraw.log 2>&1
f="/tmp/usbraw_src_$$.raw"
if [ -n "$6" ] && [ "$6" != "-" ] && [ -e "$6" ]; then SRC="$6"; else SRC="$f"; cat > "$f"; fi
magic=$(head -c2 "$SRC" | od -An -tx1 | tr -d ' \n')
tmp="/tmp/usbraw_out_$$.raw"
if [ "$magic" = "1f8b" ]; then gzip -dc "$SRC" > "$tmp"; else cat "$SRC" > "$tmp"; fi
dd if="$tmp" of=/dev/usb/lp1 status=none
rc=$?; rm -f "$f" "$tmp"; exit "$rc"
EOF
chmod 755 /usr/lib/cups/backend/usbraw

# 2) Create the raw queue and set it as default
lpadmin -p suntek-pos80 -E -v usbraw:/dev/usb/lp1 -m raw
lpadmin -d suntek-pos80
```

Confirm the USB printing node exists first:

```bash
lsusb               # expect 0483:5743 Suntek USB Printer
ls -l /dev/usb/lp1  # present if the usblp driver is loaded
```

### 7.2 Printing from other machines over the network (shared print server)

**Client machines need zero configuration and zero drivers** — no backend required. Steps:

1. Turn on sharing on the Ubuntu print server (e.g., `192.0.2.10`):
   ```bash
   cupsctl --share-printers --remote-any
   ```
2. On each client, add a *network printer* with the address:
   ```text
   ipp://192.0.2.10:631/printers/suntek-pos80
   ```
   Pick **Generic / Raw** (or CUPS' "everywhere") as the driver; client-side printing commands and GB18030 handling are identical to sections 3 and 4.

> **Note:** this Suntek MP80PC only has USB + serial (and the serial port does not work), so it has **no network port of its own**. Other machines cannot reach the printer directly — network printing must go through the Ubuntu print server (e.g., `192.0.2.10`).
