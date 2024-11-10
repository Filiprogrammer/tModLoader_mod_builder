#!/bin/sh

set -x

cd "$(dirname "$(readlink -f "$0")")"

MODDING_DIR="$(pwd)"

cd mods/BossCursor
export LD_LIBRARY_PATH="${MODDING_DIR}/Libraries/Native/Linux/"
dotnet build

cd ${MODDING_DIR}/tModPackager
dotnet run -- "${MODDING_DIR}/mods/BossCursor" "${MODDING_DIR}/BossCursor.tmod"
