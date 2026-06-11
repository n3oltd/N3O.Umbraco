# v17 Migration Audit — 2026-06-10 (session 14)

*Source: read-only audit workflow (17 agents, 1781 tool calls) — verified every known remaining task
against the actual code on `v17-Talha` + the Umbraco 17.3.5 source, and swept for new issues. This file
is the **current source of truth** for remaining migration work; the older trackers
(`MIGRATION_BLOCKERS.md`, `REVIEW_FINDINGS.md`, `TECH_DEBT_AND_MODERNIZATION.md`) predate it and are
partly stale — corrections noted at the bottom.*

## ✅ Verified DONE (previously tracked as open)
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
1. **Block List/Grid Data Export crashes / silently drops content** *(NEW — found by this audit)* — `src/N3O.Umbraco.Extensions/Content/ContentHelper.cs:208` reads `element["udi"]` (v17 `BlockItemData.Udi` is `[Obsolete][JsonIgnore]`; v17 JSON uses `"key"`), and `:194-201` guards on `block is JArray` but v17 `contentData` is a **flat** `JArray` of `JObject` → the branch never matches, so every Block editor's content is skipped/crashes on Export. **Fix (one change):** use `Guid.Parse((string)element["key"])` (null-guard legacy content) **and** process each `JObject` directly instead of the nested-`JArray` branch.
2. **Engage TelethonOnAir segment rule — client-side registration placeholder (BLOCKER-04)** — `…Cloud.Platforms.Marketing.StaticAssets/wwwroot/App_Plugins/telethon-on-air-rule/segment-rule-telethon-on-air.js` exports the descriptor but has no Engage v17 registration call. Server side done; wire the descriptor + Lit editor/display into Engage 17.2.2's client-side segment-rule API.

## 🟠 HIGH
- **PlatformsPreview tab → server NRE:** `platforms-preview-app.tsx:56-64` passes literal `'undefined'` as the content-type alias (v17 `documentType` is `{unique,collection,icon}`, no alias). Pass `contentTypeUnique` + resolve alias server-side.
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
- **BLOCKER-10 #3:** Platforms-Preview `workspaceView` shows on ALL doc types (only `WorkspaceAlias` condition). Add a composition-gating workspace-condition (Offering doc types only).
- Cropper/Uploader depend on jQuery/Formstone (Uploader CDN-fetches jQuery; Cropper no-ops without it). Product decision: native vs bundle-jQuery-locally.
- `Blazor.BackOffice` TS loader uses global `$` → ReferenceError (no global jQuery in v17). Use native DOM (bundle with the RCL conversion).
- Scheduler/WelcomeDashboard ship v13 `lang/*.xml` (ignored by v14+). Add a `lang/*.js` + `localization` extension entry; delete the XML.
- `NotificationsComposer` registers async handlers via factory lambdas, bypassing Umbraco's dedup guard. Use `AddNotificationAsyncHandler<…>` or add a `Contains()` guard.
- 3 RCL-packed projects (`Scheduler.StaticAssets`, `Cells`, `Uploader`) are `IsPackable=true` with **no `<Version>`** → pack at default 1.0.0. Add `<Version>17.0.0</Version>` + Assembly/File version.
- `N3O.Umbraco.Bundling` — orphaned non-functional stub (IBundler throws; tag-helpers crash if rendered; **zero consumers**). Decide delete-vs-replace (delete = lowest risk).
- `Locator.All()` now `DescendantsOrSelf()` (was `Descendants()`) → root nodes included (silent behavior change). Confirm intent.

## ⚪ LOW
- BLOCKER-10(b): Export/Import **tabs** visible to all users (API is group-gated; UI isn't). Optional client-side condition.
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
