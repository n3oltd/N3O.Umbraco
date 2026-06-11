# v17 Migration Audit — 2026-06-10 (session 14)

*Source: read-only audit workflow (17 agents, 1781 tool calls) — verified every known remaining task
against the actual code on `v17-Talha` + the Umbraco 17.3.5 source, and swept for new issues. This file
is the **current source of truth** for remaining migration work; the older trackers
(`MIGRATION_BLOCKERS.md`, `REVIEW_FINDINGS.md`, `TECH_DEBT_AND_MODERNIZATION.md`) predate it and are
partly stale — corrections noted at the bottom.*

## ✅ Verified DONE (previously tracked as open)

> **Update 2026-06-11 (post-audit session):** the NEW BLOCKER #1 (Block List/Grid Data Export crash) is
> now FIXED + runtime-verified; the Data Export/Import backoffice 401/empty-list problem and the upstream
> uui form-control + React console error were diagnosed and resolved; and the lost `IContentAppFactory`
> per-node/per-user content-app gating (REVIEW_FINDINGS BLOCKER-5) was RESTORED for Export/Import/Preview.
> See the "Resolved 2026-06-11" section directly below the BLOCKERS list.

- **Content Apps → Bellissima:** `ExportApp.cs`, `ImportApp.cs`, `PlatformsPreviewApp.cs` are dead comment-only stubs (no `IContentApp(Factory)`); `PlatformsContentAppsComposer.Compose()` empty. Data **Export**/**Import** re-registered as v17 `workspaceView`s (`N3O.WorkspaceView.DataExport`/`DataImport`) with built JS + TS source.
- **Dashboards:** `WelcomeDashboard.cs` / `SchedulerDashboard.cs` `IDashboard` bodies stripped; both registered via `umbraco-package.json` (`N3O.Dashboard.Welcome`/`Scheduler`); both projects are RCL.
- **BLOCKER-10(a) Hangfire auth:** resolved — `SchedulerComposer.cs:91-106` chains the custom Hangfire policy + `AuthorizationPolicies.SectionAccessSettings`.
- **BLOCKER-05 uSync Publisher:** `PublisherActionRequest(requestId, requestId.ToString())` matches uSync 17.3.6; `SyncItem.Name` set; `PublisherProcessor` registered. (Single-call completion is by-design fire-and-forget — documented caveat, not a defect.)
- **BLOCKER-06 NC→BlockList (code):** on-startup migration fully removed (`N3ONestedContentMigrationPlan.cs` + `NestedContentToBlockListMigration.cs` deleted, commit `635b54329`); moved to an external CLI. *(Operational validation still pending — see High below.)*
- **BLOCKER-11 DataComposer:** `EnsureDataTypeExists` uses key-based lookup (`UmbracoId.Generate` + `GetAsync(key)`); obsolete by-alias lookup gone.
- **Packages stable (BLOCKER-02 partial aside):** GMaps 17.0.0, Workflow 17.0.2, Umbraco.Code 2.4.0, Engage 17.2.2 are stable v17 releases.
- **Housekeeping:** `package.manifest.bak` removed (commit `203a1199b`).
- **RCL rollout / packaging:** the 12 `*.StaticAssets` + `N3O.Umbraco.Cms` converted to RCL; runtime Razor compilation in `CmsStartup`; csproj normalized to net10/v17. (This session.)

## 🔴 BLOCKERS (remaining)
1. ✅ **RESOLVED (2026-06-11) — Block List/Grid Data Export crashes / silently drops content** *(was NEW — found by this audit)* — `ContentHelper.GetContentPropertiesForBlockListOrGrid` now iterates the v17 flat `contentData` array directly; new `GetBlockElementKey` reads the element `"key"` (legacy `udi` fallback) and `GetBlockElementValuesByAlias` reads the v17 `"values"` array. **Verified end-to-end on the test site via the Management API AND the backoffice UI** (CSV contained the block's inner value; export completed with no crash). *(Original finding: `ContentHelper.cs:208` read obsolete `element["udi"]`, and `:194-201` guarded on `block is JArray` which never matched v17's flat `contentData`.)*
2. **Engage TelethonOnAir segment rule — client-side registration placeholder (BLOCKER-04)** — `…Cloud.Platforms.Marketing.StaticAssets/wwwroot/App_Plugins/telethon-on-air-rule/segment-rule-telethon-on-air.js` exports the descriptor but has no Engage v17 registration call. Server side done; wire the descriptor + Lit editor/display into Engage 17.2.2's client-side segment-rule API.

## ✅ Resolved 2026-06-11 (post-audit session)
- **Data Export/Import backoffice UI returned 401 / empty lists — FIXED + verified.** In v17 the backoffice uses an OAuth bearer token (not cookies), so the custom `[Authorize]` API controllers 401'd a token-less `fetch`. A shared authenticated-fetch runtime was added — **`@n3o/auth-fetch`** (built in `N3O.Umbraco.Cms/N3O.Umbraco.ReactRuntime`, exposed via its import map) providing `createAuthFetch(config)` + a Lit `UmbAuthFetchMixin`. The Data Export/Import shells consume it; all Export/Import endpoints now return 200 and the dropdowns/lists populate (full export verified end-to-end in the browser). Adoption documented in the ReactRuntime README.
- **uui form-control + React console error ("createElement … must not have attributes") — diagnosed (UPSTREAM bug) + worked around + verified.** Root cause is an upstream `@umbraco-ui/uui` `FormControlMixin` bug (constructor sets the `pristine` attribute; regression in uui 1.8.0, unmerged upstream fix PR umbraco/Umbraco.UI#1242): when React creates a uui form-control (`uui-select`/`uui-input`/`uui-checkbox`/`uui-radio`/`uui-toggle`/`uui-button`) it throws AND the element fails to mount. **Resolution:** the Data Export/Import React UIs were reworked to use native HTML controls inside the non-form-control uui wrappers (`uui-box`, `umb-property-layout`, `uui-loader-bar`), which fixed the broken controls and eliminated the console error (verified 0 console errors); the two views were also compacted to horizontal property layouts. A repo-wide audit found only **ONE other affected plugin still to fix — TextResourceEditor** (`Plugins/TextResourceEditor/.../text-resource-editor-app.tsx` renders `<uui-input>` in React) — **NOT yet reworked (open item, see HIGH/MEDIUM)**; all other React plugins are safe.
- **REVIEW_FINDINGS BLOCKER-5 — lost `IContentAppFactory` per-node/per-user content-app gating — RESTORED + verified** for all three affected apps. v14+ removed `IContentAppFactory`; content apps became manifest `workspaceView` extensions and lost server-side gating. A shared, configurable condition **`N3O.Condition.WorkspaceVisibility`** (in `N3O.Umbraco.ReactRuntime`, registered once) reads the document key and calls an authed `GET {endpoint}/{key}` returning `{permitted}`. Per app:
  - **Export:** `ExportVisibilityController` (N3O.Umbraco.Data) — Admin OR ExportUsers group + `IExportContentFilter.IsFilter`/`AllowExports`.
  - **Import:** `ImportVisibilityController` (N3O.Umbraco.Data) — Admin OR ImportUsers + `IImportContentFilter`.
  - **Preview:** `PlatformsPreviewController` (N3O.Umbraco.Cloud.Platforms) — content type composes `PlatformsConstants.Offerings.CompositionAlias`.

  Each manifest references the shared condition with its endpoint. **Builds 0 errors** (TestSite + Cloud.Platforms). **Export/Import verified end-to-end in the browser** (condition fires the authed call; tabs gate correctly); **Preview is compile-verified only** (the test site doesn't reference Cloud.Platforms — runtime verification pending). NOTE: this also closes the display-only **BLOCKER-10 #3** below (Platforms-Preview now content-type-gated). The Hangfire dashboard auth part of BLOCKER-5/BLOCKER-10(a) is separate and already resolved.

## 🟠 HIGH
- **PlatformsPreview tab → server NRE:** `platforms-preview-app.tsx:56-64` passes literal `'undefined'` as the content-type alias (v17 `documentType` is `{unique,collection,icon}`, no alias). Pass `contentTypeUnique` + resolve alias server-side.
- **TextResourceEditor still renders `<uui-input>` in React** *(NEW — found 2026-06-11)* — the only remaining plugin hit by the upstream uui `FormControlMixin`+React bug (see "Resolved 2026-06-11"): the control fails to mount and throws the "createElement … must not have attributes" console error. Rework to a native HTML control inside non-form-control uui wrappers (same pattern applied to Data Export/Import). `Plugins/TextResourceEditor/.../text-resource-editor-app.tsx`.
- **EditorJs image picker inserts blank images:** `editor-js-app.tsx` reads rich-object fields off the v17 media-picker result, which is now an array of GUID strings. Fetch media via management API to populate url/name/dims.
- **Missing `propertyEditorSchema` manifests** for **Uploader / Cropper / Cells** → their config fields are unconfigurable in the v17 backoffice. Add a `propertyEditorSchema` per plugin wiring each `[ConfigurationField]` to a `Umb.PropertyEditorUi.*`.
- **CampaignSending handlers not reimplemented (BLOCKER-10 #1/#2):** embed-code injection + hide-crowdfunding-tab are comment-only stubs. Implement a workspace-view + a published-state workspace-condition (pattern: DynamicListViews).
- **`Blazor.BackOffice` not converted to RCL** — still legacy `build/*.targets` + root `App_Plugins` dual-delivery → latent `NETSDK1152` publish blocker for consumers. Convert per the RCL recipe (also fixes its jQuery loader).
- **NC→BlockList transform never validated** against real v17 Block List output (DemoSite has 0 NC data types). Operational: dry-run the external CLI on a legacy DB copy before any live run.
- **`BlockItemDataExtensions.FormatBlockData`** writes to obsolete `RawPropertyValues` (no-op) + has a ContentPicker duplicate-key crash — **and has zero callers**. Delete the method (closes this + the ContentPicker nit + a CS0618).

## 🟡 MEDIUM
- `GetAwaiter().GetResult()` over async `ILanguageService` in localization accessors (deadlock risk) — add async overloads / `Task.Run` escape.
- `PublishedContentTypeCacheExtensions.Get` dereferences nullable `contentTypeService.Get(alias)` → NRE on unknown/renamed alias. Add null-guard.
- `ContentBuilderExtensions.Nested()` not `[Obsolete]` — silently writes unrenderable NC JSON. Mark `[Obsolete(error:true)]`.
- Block-preview `blockEditorCustomView` missing `forBlockEditor` → permanent spinner on block editors. Add `"forBlockEditor":"block-grid"`.
- ✅ **RESOLVED (2026-06-11) — BLOCKER-10 #3:** Platforms-Preview `workspaceView` is now composition-gated via the shared `N3O.Condition.WorkspaceVisibility` condition + `PlatformsPreviewController` (Offering compositions only). Compile-verified; runtime verification of Preview still pending (test site doesn't reference Cloud.Platforms). See "Resolved 2026-06-11" above.
- Cropper/Uploader depend on jQuery/Formstone (Uploader CDN-fetches jQuery; Cropper no-ops without it). Product decision: native vs bundle-jQuery-locally.
- `Blazor.BackOffice` TS loader uses global `$` → ReferenceError (no global jQuery in v17). Use native DOM (bundle with the RCL conversion).
- Scheduler/WelcomeDashboard ship v13 `lang/*.xml` (ignored by v14+). Add a `lang/*.js` + `localization` extension entry; delete the XML.
- `NotificationsComposer` registers async handlers via factory lambdas, bypassing Umbraco's dedup guard. Use `AddNotificationAsyncHandler<…>` or add a `Contains()` guard.
- 3 RCL-packed projects (`Scheduler.StaticAssets`, `Cells`, `Uploader`) are `IsPackable=true` with **no `<Version>`** → pack at default 1.0.0. Add `<Version>17.0.0</Version>` + Assembly/File version.
- `N3O.Umbraco.Bundling` — orphaned non-functional stub (IBundler throws; tag-helpers crash if rendered; **zero consumers**). Decide delete-vs-replace (delete = lowest risk).
- `Locator.All()` now `DescendantsOrSelf()` (was `Descendants()`) → root nodes included (silent behavior change). Confirm intent.

## ⚪ LOW
- ✅ **RESOLVED (2026-06-11) — BLOCKER-10(b):** Export/Import tabs now gate client-side via the shared `N3O.Condition.WorkspaceVisibility` condition (Export → `ExportVisibilityController` Admin/ExportUsers + filter; Import → `ImportVisibilityController` Admin/ImportUsers + filter). Verified in the browser. (API-boundary group-gating was already in place.) See "Resolved 2026-06-11" above.
- `Directory.Build.targets` `ExcludeLegacyUmbracoBackOffice` strip is now stale (Contentment moved to `Umbraco.Community.Contentment 6.1.4`) — comment/remove.
- 3 (not 5) tagged CS0618 sites remain: `BlockItemDataExtensions.cs:47` (closed by the FormatBlockData delete), `ContentMetadataConverter.LatestState.cs:23` (`IAuditService.GetLogs(int)`), `ContentTypeExtensions.cs:41` (`IDataTypeService.GetDataType(int)`). All harmless on v17 (removal v18/19).
- Redundant `EnsureUmbracoContext()` wrappers around singleton-cache access (v9-era) in several startup paths — remove.
- `LocalizationComposer` uses `AddSingleton` not `TryAddSingleton` → orphaned accessor when Cloud replaces it. Use `TryAddSingleton`.
- `BlocksComposer.AddRazorRuntimeCompilation()` duplicates the `CmsStartup` framework call (ASPDEPR003) — remove the duplicate.
- Housekeeping: delete `src/Sync/InspectUsync/` (orphaned debug project); delete empty `App_Plugins/` root dirs in Uploader/Scheduler StaticAssets; fix EditorJs manifest `$schema` (broken relative path → schemastore URL).
- 5 wrapper projects (`Forms`, `Maps.Google`, `Marketing`, `UIBuilder`, `Workflows` `.StaticAssets`) are intentional `Microsoft.NET.Sdk` pass-throughs (no assets) — correct, optionally rename/README to avoid confusion.
- Umbraco Forms + Engage v17 **license keys** must be provisioned per environment (not a code blocker).

## Doc corrections (apply to the older trackers)
- `REVIEW_FINDINGS.md` "5 CS0618" → **3**.
- `TECH_DEBT_AND_MODERNIZATION.md`: the `MembersAccessControl.cs:39,55` `GetDataType(int)` entry and the `GetPagedChildren(int)` row are **stale** (code already uses the v17 overloads) — remove.
