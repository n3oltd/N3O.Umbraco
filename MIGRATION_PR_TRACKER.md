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

> **Branch convention (2026-06-13):** each domain project and its `.StaticAssets` backoffice sibling now live on **one combined branch** named after the domain project (e.g. `v17-N3O.Umbraco.Data` carries both `N3O.Umbraco.Data` and `N3O.Umbraco.Data.StaticAssets`). The former separate `*.StaticAssets` branches were deleted. **Keep this for all future per-project branches.** These branches contain **only the two project directories** — no solution-wide files. The npm-workspace unification (root `package.json` glob, `package-lock.json`, `tsconfig.base.json`, `build/vite-config`, `Directory.Build.targets`) + the `ClientApp→Apps` rename is inherently solution-wide and is **deferred to the solution-wide PR** (it lives on `v17-Talha`, commit `e89ee591e`); it can't be sliced per-project because it depends on every app's migration being present. So a `.StaticAssets` slice still uses its pre-restructure `ClientApp` layout until that solution-wide change lands.

| Project (domain + .StaticAssets) | Branch | Tip | Notes |
|---|---|---|---|
| **ReactRuntime (auth + visibility)** | `v17-N3O.Umbraco.ReactRuntime` | `74a2a8ef8` | **Foundation — must merge first.** Shared `@n3o/auth-fetch` (`createAuthFetch` + `UmbAuthFetchMixin`) + `N3O.Condition.WorkspaceVisibility` condition added to the Cms ReactRuntime (importmap + condition manifest). _(Still predates the workspace restructure — will need the same re-cut treatment.)_ |
| **Data** (+ Data.StaticAssets) | `v17-N3O.Umbraco.Data` | `8b008592c` | Combined. v17 migration; **runtime-verified import flow** (queue → Hangfire → content created); `EnsureDataTypeExistsAsync` sets `EditorUiAlias`; import status-row DB connection isolated (DL-05); Export/Import visibility controllers + manifests restored. StaticAssets: authed Export/Import + ImportDataEditor via shared `@n3o/auth-fetch`; native HTML controls (upstream uui FormControlMixin/React bug); "View queue" → v17 route. |
| **Cloud.Platforms** (+ Cloud.Platforms.StaticAssets + Marketing.StaticAssets) | `v17-N3O.Umbraco.Cloud.Platforms` | `83b9a8b86` | Combined. v17 migration; Preview visibility endpoint restored. **BLOCKER-04 resolved:** TelethonOnAir registered via `engageSegmentRule` extension type (Engage v17 API confirmed from `manifests-personalization.js`; verified live in private manifest + Extension Insights). Preview `contentTypeAlias` undefined fix (route now accepts GUID, resolves alias server-side). `CampaignSending`/`OfferingSending` comment-updated (embed codes work; URL display + tab visibility deferred). Auth0 `UserSaving` + `ExceptionMiddleware` null guards. Marketing.StaticAssets telethon-on-air-rule App_Plugins added to this branch. |
| **Blocks** | `v17-N3O.Umbraco.Blocks` | `736ec5463` | v17 migration; dead `BlockItemDataExtensions` removed. _(Predates the workspace restructure — will need the same re-cut treatment.)_ |

> **Merge order:** `v17-N3O.Umbraco.ReactRuntime` → `v17` first; then `v17-N3O.Umbraco.Data` / `v17-N3O.Umbraco.Cloud.Platforms` (they `import '@n3o/auth-fetch'` + reference the condition, which only exist once the runtime lands).

---

## ✅ Resumed — Auth + visibility runtime (shipped to branches 2026-06-12)

- **Shared backoffice runtime + content-app visibility gating** — `@n3o/auth-fetch` (`createAuthFetch` + `UmbAuthFetchMixin`) and the configurable `N3O.Condition.WorkspaceVisibility` condition, restoring the v13 `IContentAppFactory` per-node/per-user gating for Export / Import / Preview. Previously deferred from all per-project slices; now restored as the sequenced branch set above (`v17-N3O.Umbraco.ReactRuntime` foundation + the four consumers). No PRs raised yet; ReactRuntime merges first.

## ⏳ In progress — remaining known issues (Data / Data.StaticAssets)

- ✅ **TextResourceEditor** — native-control rework done (2026-06-15, session 17): `<uui-input>` → native `<input>` (React `onChange`), the last plugin hit by the upstream uui FormControlMixin+React bug. On `v17-Talha` (not yet sliced into a per-project branch).
- Block List lookup schema (`PropertyType.Nested`) needs real-content validation.
- `ImportsMigrationsComponent` plan name not namespaced (rename has migration-rerun risk — needs a decision).
- Export memory (`ProcessExportHandler` buffers whole file) — no safe in-scope fix; needs a multi-project streaming change.
- ✅ **CS0618 `IDataTypeService.GetDataType(int)`** — replaced with `GetAsync(propertyType.DataTypeKey)` (2026-06-14).
- ✅ **CS0618 `IAuditService.GetLogs(int)`** — replaced with `GetItemsByEntityAsync(...).Items.FirstOrDefault()` (2026-06-14).
- `EPPlus 8.6.0` (tracker previously said 8.5.4) commercial-license review. **⚠️ Compliance:** `DataComposer.cs:28` currently asserts `SetNonCommercialOrganization("N3O")`, which does NOT cover for-profit company use — non-compliant as-is. Footprint is tiny (1 project, 4 files, export-only). Fix = buy a commercial license (one-line) **or** swap to ClosedXML/MIT (~1–5 days). See [`BACKLOG_SCOPING.md`](BACKLOG_SCOPING.md) §6.

## ⏳ In progress — remaining known issues (Cloud.Platforms)

- ✅ **Campaign/Offering URL display** — resolved 2026-06-14 (session 16). New `N3O.WorkspaceView.PlatformsUrls` workspace view: `PlatformsBackOfficeController.GetContentUrls` endpoint + `platforms-urls.ts`/`platforms-urls-app.tsx`; shows staging + production URLs for campaigns and offerings using `ContentLocatorExtensions` + `UrlSettingsContent`. Build 0 errors; frontend builds 0 errors.
- ✅ **BLOCKER-04: TelethonOnAir segment rule client-side registration** — resolved 2026-06-14. `engageSegmentRule` extension type confirmed from `Umbraco.Engage.StaticAssets` 17.2.2; registered in `umbraco-package.json`. Verified live in private manifest + Extension Insights with Engage running.
- ✅ **Cleanup** — resolved 2026-06-14 (session 16). Deleted `PlatformsContentAppsComposer.cs` + `PlatformsPreviewApp.cs` stubs; removed stale `TODO Migration Review` from `LinkExtensions.cs`; removed `SubscriptionFile.cs` TODO comment.
- Crowdfunding tab visibility for unpublished campaigns — needs a workspace condition (deferred).

## 🔄 In progress — pushed to a branch, no PR yet (small-change projects, session 16)

| Branch | Tip | Projects | Notes |
|---|---|---|---|
| **`v17-small-projects`** | `6d899eaeb` | Authentication + Auth0, Storage + Storage.Azure, Blog, Events, Vacancies, Email, Validation, Cdn.Cloudflare, Sync.Extensions, Bundling, Marketing, Markup.Markdown | 14 projects · 33 files · each ≤3 non-csproj changes |

## ⏳ Not started — not yet sliced into per-project PRs

- **All other framework projects** (e.g. Giving.*, Crowdfunding, Forms, Maps.Google, UIBuilder, Webhooks, Blazor.BackOffice, Scheduler, Plugins, Search, etc.) — still only in `v17-Talha`, not yet cut into per-project branches/PRs.
- **DemoSite** (Web + Core, the test harness) and **solution-wide files** (`Directory.Build.props/targets`, `*.sln`, `NuGet.Config`, root `.gitignore`, `.github/` CI, root migration `*.md` docs) — deferred to a single PR **at the end**, per the agreed workflow.
- **Backoffice npm-workspace unification + `ClientApp→Apps` rename** — solution-wide (root `package.json` glob + `package-lock.json` + `tsconfig.base.json` + `build/vite-config` + `Directory.Build.targets` + the 12 folder renames). Lives on `v17-Talha` (`e89ee591e`); part of the final solution-wide PR. Can't be sliced per-project (depends on all app migrations being present), so per-project `.StaticAssets` slices keep their `ClientApp` layout until this lands.

## ⏳ Backlog — remaining migration work (pending / not yet scheduled)

> **Scoped (2026-06-15):** every item below was researched for achievability — scope, verdicts, file refs, and cited licensing sources are in [`BACKLOG_SCOPING.md`](BACKLOG_SCOPING.md). Corrections from that research are folded into the items below.

- **N3O Image Cropper → Umbraco Image Cropper + Uploader** — replace the custom Cropper/Uploader plugins with Umbraco's built-in image cropper + upload editors.
- **Perplex Content Blocks** — finish / validate the Perplex ContentBlocks migration (Perplex rc.3; the `ContentHelper` Perplex-block parser).
- **JS calls + authentication header** — generalize the authenticated bearer-token fetch across all backoffice JS calls / plugins (ties into the deferred `@n3o/auth-fetch` shared runtime).
- **`.targets` in build** — remove the remaining per-project `build/*.targets`. `Blazor.BackOffice` was the last RCL-conversion leftover and is now converted (2026-06-15, session 17: `build/*.targets` deleted, assets → `wwwroot/App_Plugins/`). Only the intentional **wrapper** StaticAssets pass-throughs (`Forms`/`Maps.Google`/`Marketing`/`UIBuilder`/`Workflows`) remain to review.
- ✅ **Rename `ClientApp` → `Apps` across all backoffice projects (done 2026-06-13).** The root `src/package.json` npm-workspaces list is now a glob (`"**/Apps/**"`, `"N3O.Umbraco.Cms/N3O.Umbraco.ReactRuntime"`, `"!**/bin/**"`, `"!**/obj/**"`) so new frontend projects are auto-discovered — the glob keys off a folder named **`Apps`**. The 12 leaf frontend folders were `git mv`'d `ClientApp`→`Apps` (Blazor.BackOffice, Blocks, Cloud.Platforms, Data, Cells, Cropper, EditorJs, SerpEditor, TextResourceEditor, Uploader, WelcomeDashboard, Scheduler `.StaticAssets`). The **only** functional reference was the shared `BuildClientApp` target in repo-root `Directory.Build.targets` (`Exists(...\ClientApp\package.json)` → `...\Apps\...`); the per-project `.csproj` files and vite/tsconfig use relative paths (same depth) so needed no change, and `N3O.Umbraco.Cms` already used `Apps/*`. `src/package-lock.json` regenerated via `npm install`; `npm query .workspace` lists **14** (12 apps + DynamicListViews + ReactRuntime); `dotnet build N3O.Umbraco.sln` → **0 errors**. Committed on `v17-Talha` (`e89ee591e`); ships in the solution-wide PR (not the per-project branches — see the deferred list above).
- **Deprecated-dependency audit** — Newtonsoft `Json.NET` and other deprecated packages/mechanisms — find and replace. **`build.targets` sub-item is ✅ done** (zero hand-authored `build/*.targets` remain). Newtonsoft is the big one: 🔴 ~3–5 eng-weeks (301 files; `IJsonProvider` is Newtonsoft-typed at its interface; NSwag-generated `N3O.Umbraco.Clients`; Umbraco 17 itself keeps Newtonsoft transitively). See [`BACKLOG_SCOPING.md`](BACKLOG_SCOPING.md) §1.
- **Move remaining UIs to React** — migrate the remaining Lit / legacy backoffice UIs to React.
- **Package upgrades — Mediator (evaluating **Wolverine**).** *Not actually blocked today:* the pinned `MediatR 12.5.0` is the last MIT/Apache version (only a v13+ upgrade triggers the commercial license). Coupling is ~5 files behind the N3O `IMediator` abstraction; Wolverine core is MIT (but a distributed bus, overkill). ⚠️ Not urgent — proactive-upgrade timing is the decision. See [`BACKLOG_SCOPING.md`](BACKLOG_SCOPING.md) §3.
- **Lifetime-scope hacks** — fix the workarounds introduced for DI lifetime scopes (around `HttpContextAccessor` / `IUmbracoContextAccessor`).
- **Cloud project de-duplication** — remove duplication in the `Cloud.Platforms` project.
- **`checkout` / `accounts` etc. deletion in the `v17` branch** — *to discuss* (whether these projects should be dropped in v17).

---

*Generated 2026-06-11. Per-project PRs target `v17`; `v17` itself = a clean `origin/main` base accumulating the migration PRs.*
