# Packaging Research — Per-plugin `build/*.targets` vs. the RCL Standard

> **Question investigated:** *"Do we need a `.targets` file in a `build/` folder for each plugin? How does
> Umbraco do it? Is it the standard way?"*
>
> **Method:** 6 Sonnet web/Context7 research agents → Opus synthesis (earlier session), plus a full
> repo enumeration of every `.targets` file (session-10 build-packaging agent). **Documentation only —
> nothing changed.** Companions: [`NOT_REQUIRED_TO_RUN.md`](NOT_REQUIRED_TO_RUN.md) ·
> [`TECH_DEBT_AND_MODERNIZATION.md`](TECH_DEBT_AND_MODERNIZATION.md).

---

## Conclusion (short)

**No — a per-plugin copy-`.targets` file is not required and is not the modern standard.** It is the
pre-.NET-6 / Umbraco v8–v13 asset-delivery pattern. The de-facto and officially-documented standard for
Umbraco 14–17 packages is a **Razor Class Library** (`Microsoft.NET.Sdk.Razor`) that serves its
`App_Plugins` assets through the ASP.NET Core **static web assets** pipeline, with:

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <StaticWebAssetBasePath>App_Plugins/N3O.Umbraco.SerpEditor</StaticWebAssetBasePath>
  </PropertyGroup>
</Project>
```

Assets live under `wwwroot/`; the SDK packs and delivers them to consumers automatically. There is **no
hand-written `<Copy>` target, no `buildTransitive/*.targets`, and no consumer-side `<Import>`.** Umbraco's
own first-party packages (e.g. `Umbraco.Forms.StaticAssets`) use this RCL model.

---

## Why the current `.targets` pattern is legacy

The repo ships **14** per-plugin `build/<id>.targets` files, each containing a `<Copy>` task that runs on the
**consuming** project's build to copy `App_Plugins/**` out of the NuGet package cache into the consumer's
`App_Plugins` folder:

| # | Project (under `src/`) |
|---|---|
| 1 | `Blazor/N3O.Umbraco.Blazor.BackOffice` |
| 2 | `Blocks/N3O.Umbraco.Blocks.StaticAssets` |
| 3 | `Cloud/N3O.Umbraco.Cloud.Platforms.Marketing.StaticAssets` |
| 4 | `Cloud/N3O.Umbraco.Cloud.Platforms.StaticAssets` |
| 5 | `Data/N3O.Umbraco.Data.StaticAssets` |
| 6 | `N3O.Umbraco.Cms` |
| 7 | `Plugins/Cells/N3O.Umbraco.Cells.StaticAssets` |
| 8 | `Plugins/Cropper/N3O.Umbraco.Cropper.StaticAssets` |
| 9 | `Plugins/EditorJs/N3O.Umbraco.EditorJs.StaticAssets` |
| 10 | `Plugins/SerpEditor/N3O.Umbraco.SerpEditor.StaticAssets` |
| 11 | `Plugins/TextResourceEditor/N3O.Umbraco.TextResourceEditor.StaticAssets` |
| 12 | `Plugins/Uploader/N3O.Umbraco.Uploader.StaticAssets` |
| 13 | `Plugins/WelcomeDashboard/N3O.Umbraco.WelcomeDashboard.StaticAssets` |
| 14 | `Scheduler/N3O.Umbraco.Scheduler.StaticAssets` |

Problems with this mechanism:

- **It pre-dates the SDK feature that replaces it.** Static Web Assets (.NET 6+) does delivery + publish +
  content-hashing + cleanup natively; the manual `<Copy>` does none of that and isn't incrementally cached.
- **Dual delivery / redundancy.** Each `.StaticAssets` csproj *also* packs `App_Plugins/**` as `<Content>`
  (`ContentTargetFolders=.`). So assets are shipped twice — once as NuGet content, once copied by the
  target. The content path is effectively dead weight under the targets approach.
- **It doesn't work transitively for ProjectReferences.** `DemoSite.Web.csproj` has to manually `<Import>`
  each of 8 `.targets` files (a `ProjectReference` doesn't trigger `buildTransitive`). Every new plugin needs
  two edits (reference + import). Under RCL this is automatic.
- **No isolation / no cleanup.** Stale files left in a consumer's `App_Plugins` after a package downgrade
  aren't removed; the static-web-assets manifest handles this.

---

## Migration plan (if/when you choose to do it)

Per `.StaticAssets` project:

1. Change the SDK: `<Project Sdk="Microsoft.NET.Sdk">` → `<Project Sdk="Microsoft.NET.Sdk.Razor">`.
2. Move `App_Plugins/<name>/**` → `wwwroot/App_Plugins/<name>/**` (or set `StaticWebAssetBasePath` to
   `App_Plugins/<name>` and keep a `wwwroot` root).
3. Delete the `build/<id>.targets` file and its `<None Include="build\**" PackagePath="buildTransitive">` item.
4. Remove the redundant `<Content Include="App_Plugins\**" Pack="true" ...>` items (the RCL packs `wwwroot`).
5. Keep the `BuildClientApp` Vite target, but point its output at `wwwroot/App_Plugins/<name>/`.
6. In `DemoSite.Web.csproj`, delete the 8 manual `<Import>` lines — RCL assets flow via `ProjectReference`.

**Verification:** `dotnet build DemoSite.Web` and confirm `bin/.../wwwroot/App_Plugins/<name>/*.js` is present
and served at `/App_Plugins/<name>/...`; `dotnet pack` and inspect the `.nupkg` (`staticwebassets` manifest +
`wwwroot` content, no `buildTransitive`).

---

## Gotchas

- **`StaticWebAssetBasePath` casing must exactly match** the `App_Plugins/<name>` Umbraco expects
  (Umbraco scans App_Plugins 2 levels deep). Get it wrong and the plugin silently doesn't load.
- **Production runtime mode disables `UseStaticWebAssets()`.** Umbraco's package discovery uses a composite
  file provider (WebRoot + ContentRoot); confirm assets resolve in `Production` runtime mode, not just dev —
  this is the single most important thing to test, because it's the historical reason teams kept the copy
  pattern. (If RCL assets don't surface in Production for a given host config, that's the deciding factor.)
- **`umbraco-package.json` / import-map** (the React runtime in `N3O.Umbraco.Cms`) must live under the same
  `wwwroot/App_Plugins/<name>/` so its served path is unchanged.
- Mixed approach is fine during transition — RCL and copy-targets projects can coexist while you migrate
  one plugin at a time (start with `SerpEditor.StaticAssets`, the simplest, as a proof).

---

## Recommendation

Treat this as a **deferred, opt-in modernization**, not a migration blocker — the current `.targets` mechanism
**works** and is load-bearing for NuGet consumers today. When you do it:

1. **Prove it on one plugin** (`SerpEditor.StaticAssets`) end-to-end including a **Production-runtime-mode**
   check before converting the rest.
2. Pair it with the `Directory.Build.props` / Central Package Management work (TECH_DEBT D-02) and the
   shared `BuildClientApp` target extraction (D-04) — they touch the same files.
3. Net result across all 14 projects: delete 14 `.targets` files + 8 `DemoSite` `<Import>` lines + the
   redundant `<Content>` packing, and gain incremental builds, content-hashing, and automatic
   ProjectReference flow.
