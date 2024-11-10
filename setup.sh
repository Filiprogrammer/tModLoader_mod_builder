#!/bin/sh

set -e
set -x

apt update
apt install git wget libicu-dev
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 6.0.427
export PATH=$PATH:$HOME/.dotnet
export DOTNET_ROOT=$HOME/.dotnet
export DOTNET_CLI_TELEMETRY_OPTOUT=1
dotnet --version
