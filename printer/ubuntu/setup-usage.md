# Using the setup script on another device

Run on the target device:

```bash
sudo bash /media/<your-USB-drive>/setup-pos80.sh
```

Connect the printer via USB first. One command does the whole job, fully automatically:

1. Check / install CUPS (runs `apt install cups` if missing)
2. Detect the printer (only accepts 0483:5743)
3. Auto-locate the `/dev/usb/lpN` node (not hard-coded to lp1)
4. Install the `usbraw` backend (skips CUPS' IEEE-1284 probe) — adapts to the node path automatically
5. Create the `suntek-pos80` raw queue and set it as default
6. Print a test ticket automatically to verify, then show print-command hints

## Key features

- Self-contained: a single file, zero extra dependencies (CUPS + gzip are stock)
- Idempotent: safe to run repeatedly on the same machine
- Prints a test ticket automatically: paper out means setup succeeded, no manual check needed
- Troubleshooting log: `/var/log/usbraw.log`

## Notes

- Requires root (`sudo bash`). On a FAT32 USB drive just run the script with bash — no need to set the executable bit.
- Supports only this printer model (VID/PID hard-coded to 0483:5743) to avoid misconfiguring other USB devices.
- If the target is Ubuntu Server without CUPS, the script installs it over the network — internet access is required.
