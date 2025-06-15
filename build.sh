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

SDK_VERSION="9.0"
DAYS_OLD=28

if [ -n "$(find dotnet-install.sh -mtime +${DAYS_OLD})" ]; then
    echo "dotnet-install.sh is older than ${DAYS_OLD} days, removing it"
    # Remove the old dotnet-install.sh script
    rm dotnet-install.sh
fi

# Check if dotnet-install.sh exists, if not download it
if [ ! -f dotnet-install.sh ]; then
    wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
    # Ensure the timestamp is up to date
    touch dotnet-install.sh
    # Make the script executable
    chmod +x ./dotnet-install.sh

    # Install .NET SDK
    echo "Installing .NET SDK ${SDK_VERSION}"
    ./dotnet-install.sh --channel STS --version ${SDK_VERSION}
    dotnet --version
    dotnet --list-runtimes
    dotnet --list-sdks
fi

function apt_install() {
    if [ ! -f "apt_installed_$1.txt" ]; then
        echo "Install $1"
        sudo apt install "$1" -y
        date > "apt_installed_$1.txt"
    fi
}

function brew_install() {
    if [ ! -f "brew_installed_$1.txt" ]; then
        echo "Install $1"
        brew install "$1" -y
        date > "brew_installed_$1.txt"
    fi
}

PLATFORM=$(uname)
PLATFORM_LC=${PLATFORM,,}

if [ "${PLATFORM_LC}" == "darwin" ]; then
    if [ ! -f "brew_installed.txt" ]; then
        echo "Install Homebrew"

        /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

        date > "brew_installed.txt"
    fi

    #brew_install git
else
    echo "Install tools for ${PLATFORM}"

    apt_install git
fi

if [ -d .git ]; then
    echo "Update git repository"

    if [ ! -d Charon ]; then
        echo "Clone Charon submodule"
        git clone git@github.com:montague247/Charon.git Charon
        git submodule init
    fi

    git pull --recurse-submodules --jobs=10
    git -C Charon pull
fi

#src/update_dependencies.sh

function build_release() {
    echo "Build release for runtime $1 @ ${PLATFORM}"

    dotnet build src/NetSentinel/NetSentinel.csproj -c Release -o "Release/$1" -r "$1" --self-contained true
}

CURRENT_BUILD_REV=$(git rev-parse HEAD)

if [ -f "last_build_rev.txt" ]; then
    LAST_BUILD_REV=$(cat last_build_rev.txt)
else
    LAST_BUILD_REV=""
fi

if [ "$CURRENT_BUILD_REV" != "$LAST_BUILD_REV" ]; then
    MACHINE_HARDWARE_NAME=$(uname -m)

    if [ "${MACHINE_HARDWARE_NAME}" == "aarch64" ]; then
        MACHINE_HARDWARE_NAME=arm64
    fi

    build_release "${PLATFORM_LC}-${MACHINE_HARDWARE_NAME}"

    echo "$CURRENT_BUILD_REV" > last_build_rev.txt
fi
