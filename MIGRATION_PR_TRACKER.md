# N3O.Umbraco → Umbraco 17.3.5 / .NET 10 — PR Tracker

> Status tracker for the GitHub migration issue. **"Done" = the work is in a branch that has an *active PR*.** Everything else is **in progress** — including work that's already pushed to a branch but does **not** yet have a PR open.
>
> The migration is shipped as **one PR per project** (base = `v17`), the same way `N3O.Umbraco.Extensions` was. The integration branch `v17-Talha` (and its backup) is **not** a tracked PR — it's the working branch the per-project slices are cut from. Repo: `github.com/n3oltd/umbraco-extensions`.

Legend: ✅ Done (in an active PR) · 🔄 In progress (pushed, no PR yet) · ⏳ Not started / deferred

---

## ✅ Done — in an active PR

| Workstream | Branch | PR | Base | Contains |
|---|---|---|---|---|
| **Extensions + CMS** | `v17-N3O.Umbraco.Extensions` | **#850** | `v17` | Core framework migration (Extensions + Cms); the shared **React** runtime (`N3O.Umbraco.ReactRuntime`: react / react-dom / jsx-runtime); v17 **Block List/Grid data export** fix (`ContentHelper`); DynamicListViews + its `{contentId:guid}` visibility route. |
| **csproj normalization** | `v17-csproj-normalization` | **#852** | `main` | net10 / v17 `.csproj` normalization across 70 leaf projects (cross-cutting chore, separate from the per-project v17 PRs). |

**Resumed (2026-06-12):** the shared `@n3o/auth-fetch` runtime + `N3O.Condition.WorkspaceVisibility` condition + adoption README were deferred out of `#850`; they have now been shipped as a sequenced set of branches (see "Auth + visibility runtime" below). The ReactRuntime branch must merge into `v17` **before** the four consumer branches.

---

## 🔄 In progress — pushed to a branch, **no PR open yet** (ready to raise)

Each is built, committed, and pushed; just needs a PR opened against `v17` (compare URL: `…/compare/v17...<branch>`).

| Project | Branch | Tip | Notes |
|---|---|---|---|
| **ReactRuntime (auth + visibility)** | `v17-N3O.Umbraco.ReactRuntime` | `74a2a8ef8` | **Foundation — must merge first.** Shared `@n3o/auth-fetch` (`createAuthFetch` + `UmbAuthFetchMixin`) + `N3O.Condition.WorkspaceVisibility` condition added to the Cms ReactRuntime (importmap + condition manifest). |
| **Data** | `v17-N3O.Umbraco.Data` | `1b5e1bf7e` | v17 migration; **runtime-verified import flow** (queue → Hangfire → content created); `EnsureDataTypeExistsAsync` sets `EditorUiAlias`; import status-row DB connection isolated from the content scope (DL-05). Export/Import visibility controllers restored. |
| **Data.StaticAssets** | `v17-N3O.Umbraco.Data.StaticAssets` | `da5b34167` | Authed Export/Import + ImportDataEditor now via shared `@n3o/auth-fetch`; native HTML controls (avoids the upstream uui FormControlMixin/React bug); import "View queue" link points at the v17 route. Export/Import visibility-condition manifests restored. |
| **Cloud.Platforms** | `v17-N3O.Umbraco.Cloud.Platforms` | `e9768e4bf` | v17 migration. Preview visibility endpoint (PlatformsPreviewController + PreviewApiName doc) restored. |
| **Cloud.Platforms.StaticAssets** | `v17-N3O.Umbraco.Cloud.Platforms.StaticAssets` | `1d45c50b2` | RCL packaging; Preview workspace view. Preview visibility condition restored. |
| **Blocks** | `v17-N3O.Umbraco.Blocks` | `736ec5463` | v17 migration; dead `BlockItemDataExtensions` removed. |

> **Merge order:** `v17-N3O.Umbraco.ReactRuntime` → `v17` first; then Data / Data.StaticAssets / Cloud.Platforms / Cloud.Platforms.StaticAssets (they `import '@n3o/auth-fetch'` + reference the condition, which only exist once the runtime lands).

---

## ✅ Resumed — Auth + visibility runtime (shipped to branches 2026-06-12)

- **Shared backoffice runtime + content-app visibility gating** — `@n3o/auth-fetch` (`createAuthFetch` + `UmbAuthFetchMixin`) and the configurable `N3O.Condition.WorkspaceVisibility` condition, restoring the v13 `IContentAppFactory` per-node/per-user gating for Export / Import / Preview. Previously deferred from all per-project slices; now restored as the sequenced branch set above (`v17-N3O.Umbraco.ReactRuntime` foundation + the four consumers). No PRs raised yet; ReactRuntime merges first.

## ⏳ In progress — remaining known issues (Data / Data.StaticAssets)

- **TextResourceEditor** still renders `<uui-input>` in React (same upstream uui FormControlMixin bug) — needs the native-control rework.
- Block List lookup schema (`PropertyType.Nested`) needs real-content validation.
- `ImportsMigrationsComponent` plan name not namespaced (rename has migration-rerun risk — needs a decision).
- Export memory (`ProcessExportHandler` buffers whole file) — no safe in-scope fix; needs a multi-project streaming change.
- CS0618 deprecations (`IDataTypeService.GetDataType(int)` U18, `IAuditService.GetLogs(int)` U19) — deliberate TODOs.
- `EPPlus 8.5.4` commercial-license review.

## ⏳ Not started — not yet sliced into per-project PRs

- **All other framework projects** (e.g. Giving.*, Crowdfunding, Marketing/Engage, Forms, Maps.Google, UIBuilder, Webhooks, Authentication/Auth0, Blazor.BackOffice, etc.) — still only in `v17-Talha`, not yet cut into per-project branches/PRs.
- **DemoSite** (Web + Core, the test harness) and **solution-wide files** (`Directory.Build.props/targets`, `*.sln`, `NuGet.Config`, root `.gitignore`, `.github/` CI, root migration `*.md` docs) — deferred to a single PR **at the end**, per the agreed workflow.

## ⏳ Backlog — remaining migration work (pending / not yet scheduled)

- **N3O Image Cropper → Umbraco Image Cropper + Uploader** — replace the custom Cropper/Uploader plugins with Umbraco's built-in image cropper + upload editors.
- **Perplex Content Blocks** — finish / validate the Perplex ContentBlocks migration (Perplex rc.3; the `ContentHelper` Perplex-block parser).
- **JS calls + authentication header** — generalize the authenticated bearer-token fetch across all backoffice JS calls / plugins (ties into the deferred `@n3o/auth-fetch` shared runtime).
- **`.targets` in build** — remove the remaining per-project `build/*.targets` (RCL-conversion leftovers / wrapper StaticAssets).
- ✅ **Rename `ClientApp` → `Apps` across all backoffice projects (done 2026-06-13).** The root `src/package.json` npm-workspaces list is now a glob (`"**/Apps/**"`, `"N3O.Umbraco.Cms/N3O.Umbraco.ReactRuntime"`, `"!**/bin/**"`, `"!**/obj/**"`) so new frontend projects are auto-discovered — the glob keys off a folder named **`Apps`**. The 12 leaf frontend folders were `git mv`'d `ClientApp`→`Apps` (Blazor.BackOffice, Blocks, Cloud.Platforms, Data, Cells, Cropper, EditorJs, SerpEditor, TextResourceEditor, Uploader, WelcomeDashboard, Scheduler `.StaticAssets`). The **only** functional reference was the shared `BuildClientApp` target in repo-root `Directory.Build.targets` (`Exists(...\ClientApp\package.json)` → `...\Apps\...`); the per-project `.csproj` files and vite/tsconfig use relative paths (same depth) so needed no change, and `N3O.Umbraco.Cms` already used `Apps/*`. `src/package-lock.json` regenerated via `npm install`; `npm query .workspace` lists **14** (12 apps + DynamicListViews + ReactRuntime); `dotnet build N3O.Umbraco.sln` → **0 errors**. Uncommitted on `v17-Talha`. (NB: `git status` cross-matches some near-identical `tsconfig.json` rename pairs across projects — cosmetic only; the committed tree is correct.)
- **Deprecated-dependency audit** — Newtonsoft `Json.NET`, `build.targets`, and other deprecated packages/mechanisms — find and replace.
- **Move remaining UIs to React** — migrate the remaining Lit / legacy backoffice UIs to React.
- **Package upgrades blocked by licenses** — Mediator (evaluating **Wolverine**) not upgraded due to licensing.
- **Lifetime-scope hacks** — fix the workarounds introduced for DI lifetime scopes (around `HttpContextAccessor` / `IUmbracoContextAccessor`).
- **Cloud project de-duplication** — remove duplication in the `Cloud.Platforms` project.
- **`checkout` / `accounts` etc. deletion in the `v17` branch** — *to discuss* (whether these projects should be dropped in v17).

---

*Generated 2026-06-11. Per-project PRs target `v17`; `v17` itself = a clean `origin/main` base accumulating the migration PRs.*
