#!/usr/bin/env bash

if [ ! -f dotnet-install.sh ]; then
    wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
    chmod +x ./dotnet-install.sh

    ./dotnet-install.sh --version latest
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

if [ "$(uname)" == "Darwin" ]; then
    if [ ! -f "brew_installed.txt" ]; then
        echo "Install Homebrew"
        /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
        date > "brew_installed.txt"
    fi

    #brew_install git
    #brew_install rrdtool
    #brew_install nmap
else
    echo "Install tools for $(uname)"

    apt_install git
    apt_install rrdtool
    apt_install nmap
fi

if [ -d .git ]; then
    echo "Update git repository"
    git pull
fi

./update_dependencies.sh

function build_release() {
    echo "Build release for runtime $1 @ $(uname)"
    dotnet build NetSentinel/NetSentinel.csproj -c Release -o "Release/$1" -r "$1" --self-contained true
}

CURRENT_BUILD_REV=$(git rev-parse HEAD)

if [ -f "last_build_rev.txt" ]; then
    LAST_BUILD_REV=$(cat last_build_rev.txt)
else
    LAST_BUILD_REV=""
fi

if [ "$CURRENT_BUILD_REV" != "$LAST_BUILD_REV" ]; then
    echo "Update last build revision"

    build_release linux-arm64
    build_release linux-x64
    build_release win-arm64
    build_release win-x64

    echo "$CURRENT_BUILD_REV" > last_build_rev.txt
fi
