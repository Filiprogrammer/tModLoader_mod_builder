# tModLoader mod builder

This is a quickly thrown together project that makes it easy to build a tModLoader mod on Linux without having Terraria installed. It also works inside a Docker container.

Usage:

```console
. setup.sh
./build.sh
```

This generates a BossCursor.tmod file. The BossCursor mod is just used as an example here. Other mods can also easily be built. Place the source repository of the mod into the mods directory. Edit the build.sh file accordingly. And run build.sh.
