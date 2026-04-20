#!/usr/bin/env bash
#  _   _      _    _____            _   _            _ 
# | \ | |    | |  / ____|          | | (_)          | |
# |  \| | ___| |_| (___   ___ _ __ | |_ _ _ __   ___| |
# | . ` |/ _ \ __|\___ \ / _ \ '_ \| __| | '_ \ / _ \ |
# | |\  |  __/ |_ ____) |  __/ | | | |_| | | | |  __/ |
# |_| \_|\___|\__|_____/ \___|_| |_|\__|_|_| |_|\___|_|
#
# Copyright (c) Dirk Helbig. All rights reserved.
#

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u
# By default cmd1 | cmd2 returns exit code of cmd2 regardless of cmd1 success
# This is causing it to fail
set -o pipefail

Charon/update_dev.sh

echo "Updating NetSentinel and dependencies..."

src/update_dependenies.sh

echo "Finished updating NetSentinel and dependencies."
