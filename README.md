# Thunderstore Mod Template

A batteries-included template for building [BepInEx](https://github.com/BepInEx/BepInEx) mods for Steam games and publishing them to [Thunderstore](https://thunderstore.io).

## What's included

- `src/` — C# source for the BepInEx plugin (`Plugin.cs`, `Patches/`)
- `thunderstore/` — `manifest.json`, `README.md`, `CHANGELOG.md`, and the slot for `icon.png` that ship inside the package
- MSBuild targets for **Build**, **Pack** (zip a Thunderstore-ready archive), and **Deploy** (copy the DLL into your local game install)
- `scripts/publish.ps1` / `scripts/publish.sh` — one-shot build + pack + upload via [`tcli`](https://github.com/thunderstore-io/thunderstore-cli)
- `scripts/rename.ps1` — rebrand the template in one command
- BepInEx + HarmonyX + UnityEngine.Modules pulled from NuGet (no manual DLL hunting)

## Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- The Steam game installed locally (only required if you reference its assemblies or use the `Deploy` target)
- A [Thunderstore account](https://thunderstore.io/settings/account/) and an API token for publishing

## Quick start

1. **Rename the template:**
   ```pwsh
   pwsh ./scripts/rename.ps1 -ModName CoolMod -Author Alice
   ```
2. **(Optional) Point at your game install** so game assemblies resolve and `Deploy` works. Copy `Directory.Build.user.props.example` to `Directory.Build.user.props` and set `<GameDir>`. Also set `<GameName>` in `Directory.Build.props` (e.g. `Lethal Company`) so the `*_Data/Managed` path resolves.
3. **Drop a 256x256 `icon.png`** into `thunderstore/`.
4. **Edit `thunderstore/manifest.json`** and `thunderstore.toml` — set `dependencies` (e.g. the BepInEx pack ID for your game's community) and the `community` slug.
5. **Build:**
   ```pwsh
   dotnet build src/MyMod.csproj
   ```

## Common commands

| Goal | Command |
|------|---------|
| Compile the mod | `dotnet build src/MyMod.csproj` |
| Build a Thunderstore zip in `dist/` | `dotnet build src/MyMod.csproj -t:Pack` |
| Copy DLL into the game's `BepInEx/plugins` | `dotnet build src/MyMod.csproj -t:Deploy` |
| Build + pack only (no upload) | `pwsh ./scripts/publish.ps1 -PackOnly` |
| Build + pack + publish to Thunderstore | `pwsh ./scripts/publish.ps1 -Token $env:THUNDERSTORE_TOKEN` |
| Same on macOS / Linux | `./scripts/publish.sh` |

The Pack target produces `dist/<Author>-<ModName>-<Version>.zip` with this Thunderstore-compliant layout:

```
manifest.json
icon.png
README.md
CHANGELOG.md
plugins/
  MyMod/
    MyMod.dll
```

## Versioning

Bump `<ModVersion>` in `Directory.Build.props` **and** `version_number` in `thunderstore/manifest.json` (and `versionNumber` in `thunderstore.toml`) before publishing. Thunderstore rejects re-uploads of an existing version.

## Publishing

1. Get a token at https://thunderstore.io/settings/account/ → "Service accounts".
2. Set it: `setx THUNDERSTORE_TOKEN "tss_xxx"` (Windows) or `export THUNDERSTORE_TOKEN=tss_xxx`.
3. Set `community` in `thunderstore.toml` to your game's slug (e.g. `lethal-company`).
4. Run `pwsh ./scripts/publish.ps1`.

## Project layout

```
.
├── Directory.Build.props          # shared MSBuild props (mod name, version, paths)
├── NuGet.config                   # adds the BepInEx NuGet feed
├── MyMod.sln
├── src/
│   ├── MyMod.csproj               # plugin project + Pack/Deploy targets
│   ├── Plugin.cs                  # BepInEx entry point
│   └── Patches/                   # Harmony patches go here
├── thunderstore/
│   ├── manifest.json
│   ├── README.md
│   ├── CHANGELOG.md
│   └── icon.png                   # add your own
├── thunderstore.toml              # tcli config
└── scripts/
    ├── publish.ps1
    ├── publish.sh
    └── rename.ps1
```
