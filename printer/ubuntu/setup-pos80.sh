#!/usr/bin/env bash
# =====================================================================
# setup-pos80.sh — Suntek MP80PC thermal receipt printer · Ubuntu one-click setup
# =====================================================================
# Purpose:  On any Ubuntu 24.04 machine, configure a connected Suntek MP80PC (ESC/POS)
#           as a CUPS raw queue suntek-pos80 — no "driver" installation required.
# Usage:    sudo bash setup-pos80.sh          (connect the printer via USB first)
# Requires: stock CUPS + gzip; everything else is included in this script.
# Why a custom backend (usbraw): CUPS' stock usb:// backend sends an IEEE-1284
#           Device-ID probe to the printer every time; this printer's firmware
#           cannot handle it and the USB port resets repeatedly (the kernel log
#           shows usblp re-enumerating every 5 seconds), so jobs hang forever.
#           The usbraw backend skips the probe and writes straight to /dev/usb/lpN.
# =====================================================================
set -u

LOG=/var/log/usbraw.log
BACKEND=/usr/lib/cups/backend/usbraw
VID=0483
PID=5743

say()  { printf '\033[1;32m%s\033[0m\n' "$*"; }
warn() { printf '\033[1;33mWARN: %s\033[0m\n' "$*"; }

# ---- 0. root required ----
if [ "$(id -u)" -ne 0 ]; then
  echo "ERROR: run as root —  sudo bash $0"
  exit 1
fi

# ---- 1. make sure CUPS is installed and running ----
if ! command -v cupsd >/dev/null 2>&1; then
  say "CUPS not detected, installing (network required)..."
  export DEBIAN_FRONTEND=noninteractive
  apt-get update -qq
  apt-get install -y -qq cups || {
    echo "ERROR: CUPS installation failed (no network or repo unavailable). Install cups manually and re-run."
    exit 1
  }
fi
systemctl enable --now cups >/dev/null 2>&1

# ---- 2. load usblp and wait for the printer to appear ----
modprobe usblp 2>/dev/null || true
for i in $(seq 1 10); do
  lsusb -d "${VID}:${PID}" >/dev/null 2>&1 && break
  warn "Printer not seen (0483:5743), waiting attempt ${i} ..."
  sleep 1
done
lsusb -d "${VID}:${PID}" >/dev/null 2>&1 || {
  echo "ERROR: printer not detected. Confirm the printer is powered on and USB is connected, then re-run."
  exit 1
}

# ---- 3. find the print node (prefer VID/PID match; fall back to the first) ----
DEV=""
for n in /dev/usb/lp*; do
  [ -e "$n" ] || continue
  if udevadm info -q property -n "$n" 2>/dev/null \
    | grep -qE "^(ID_VENDOR_ID=${VID}|ID_MODEL_ID=${PID})$"; then
    DEV="$n"; break
  fi
done
if [ -z "$DEV" ]; then
  warn "No VID/PID match; falling back to the first usblp node"
  for n in /dev/usb/lp*; do [ -e "$n" ] && { DEV="$n"; break; }; done
fi
[ -z "$DEV" ] && { echo "ERROR: no /dev/usb/lp* found. Try modprobe usblp or a different USB port."; exit 1; }
say "Print node: $DEV"

# ---- 4. install the custom backend ----
cat > "$BACKEND" <<'EOF'
#!/usr/bin/env bash
# usbraw: CUPS job data -> write directly to the USB printer-class device, no IEEE-1284 probe
exec >> /var/log/usbraw.log 2>&1
dev="/dev/usb/lp1"
if [ -n "${1:-}" ]; then dev="${1#usbraw:/}"; fi
f="/tmp/usbraw_src_$$.raw"
if [ -n "${6:-}" ] && [ "$6" != "-" ] && [ -e "$6" ]; then SRC="$6"; else SRC="$f"; cat > "$f"; fi
magic=$(head -c2 "$SRC" | od -An -tx1 | tr -d ' \n')
tmp="/tmp/usbraw_out_$$.raw"
if [ "$magic" = "1f8b" ]; then gzip -dc "$SRC" > "$tmp"; else cat "$SRC" > "$tmp"; fi
dd if="$tmp" of="$dev" status=none 2>/dev/null
rc=$?; rm -f "$f" "$tmp"; exit "$rc"
EOF
chmod 755 "$BACKEND"
: > "$LOG"; chmod 666 "$LOG" 2>/dev/null || true

# ---- 5. restart CUPS and create the raw queue ----
systemctl restart cups >/dev/null 2>&1
lpadmin -p suntek-pos80 -E -v "usbraw:$DEV" -m raw 2>/dev/null
lpadmin -d suntek-pos80 2>/dev/null
cupsenable suntek-pos80

# ---- 6. verification ----
echo
say "==== Configuration result ===="
lpstat -a | grep -i suntek-pos80 || { echo "ERROR: queue does not exist."; exit 1; }
lpstat -v suntek-pos80

# ---- 7. print a test ticket automatically ----
{
  printf '\033@'                                   # ESC @ init
  printf 'suntek-pos80 CONFIG OK\n'
  printf 'Node: %s\n' "$DEV"
  printf 'Non-ASCII print test\n' | iconv -f UTF-8 -t GB18030 2>/dev/null || printf 'ascii test\n'
  printf '\n\n'                                    # feed paper
} > /tmp/pos80_test.bin
lp -d suntek-pos80 -o raw /tmp/pos80_test.bin >/dev/null 2>&1
echo
echo "Confirm the test ticket printed (contents: suntek-pos80 CONFIG OK + Non-ASCII print test)"
echo

# ---- 8. hints ----
say "Done. Common commands (encode non-ASCII text to GB18030 first):"
echo "    lp -d suntek-pos80 -o raw ticket.txt"
echo "    iconv -f UTF-8 -t GB18030 ticket.txt | lp -d suntek-pos80 -o raw"
echo
warn "Do not create or use a usb://Printer/... queue (it triggers the Device-ID probe and resets the printer)."
warn "Backend log: $LOG ; see README.md in this folder for troubleshooting."
