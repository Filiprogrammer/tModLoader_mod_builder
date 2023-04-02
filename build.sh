#!/bin/sh

set -x

cd "$(dirname "$(readlink -f "$0")")"

MODDING_DIR="$(pwd)"

cd mods/BossCursor
dotnet build

cd ${MODDING_DIR}/tModPackager
dotnet run -- "${MODDING_DIR}/mods/BossCursor" "${MODDING_DIR}/BossCursor.tmod"
