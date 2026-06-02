# N3O.Umbraco: Umbraco 13 → 17 Migration Plan

*Last updated: 2026-06-02 (session 5) — uSync Publisher reimplemented, 14-agent branch review (`REVIEW_FINDINGS.md`), review-found defects fixed, NestedContent→BlockList code replacement done*

---

## Executive Summary

N3O.Umbraco is a **shared Umbraco package framework** (120 projects) consumed by multiple client sites. The migration target is **Umbraco 17.3.5 on .NET 10**.

**Current state:** The solution **builds with 0 errors** and **the app starts successfully (HTTP 200 confirmed)**. All compile-time API breakages and the blocking runtime startup crashes have been resolved. **The AngularJS → Bellissima frontend migration (BLOCKER-07) is now largely done** — all 16 plugin areas migrated to `umbraco-package.json` + Lit (15 done, telethon blocked); dashboards + Data property editors verified live in the running backoffice; a critical property-editor alias bug was found and fixed.

**Session 5 update:** uSync Publisher `SyncContentHandler` reimplemented on the v17 `PublisherProcessor`/`Jumoo.Processing` API (RR-01/BLOCKER-05 — *resolved in code, needs remote-server E2E*). A **14-agent branch deliberation review** (verdict: make-sense-with-fixes) ran over the whole `v17-Talha` diff — full tracker `REVIEW_FINDINGS.md`. Four review-found defects were fixed (CRITICAL duplicate `Umbraco.BlockList` converter → deleted; NC migration empty-path JSON + non-transactional steps; `UrlInfo.AsUrl` arg order), and the **NestedContent→BlockList code replacement** was completed (lookup re-key, `[Obsolete(error:true)]` redirects, a live `DonationItemReceiver` caller fixed). New production blockers surfaced: **BLOCKER-10 access-control regressions** and **BLOCKER-11 `EnsureDataTypeExists` dup-data-types** (see `MIGRATION_BLOCKERS.md`). Remaining: **live-render testing (needs content fixtures), the deferred review blockers, public-API obsolete/guide work, and per-site data migration (+ real-data dry-run of the NC→BlockList value transform)**. See `BELLISSIMA_MIGRATION_LOG.md` and `REVIEW_FINDINGS.md`.

---

## Migration Progress

| Area | Status | Notes |
|---|---|---|
| Target framework (all 120 projects) | **Done** | All on `net10.0` |
| Umbraco core packages | **Done** | `Umbraco.Cms` 17.3.5 everywhere |
| Solution compiles (0 errors) | **Done** | Verified full solution build |
| Startup pipeline | **Done** | `AddBackOffice/AddWebsite/AddDeliveryApi/AddComposers/AddContentment` |
| `UmbracoApiController` → `ControllerBase` | **Done** | Both base classes + all downstream controllers |
| `SectionRequirement` → `RequireAuthenticatedUser` | **Done** | SchedulerComposer |
| `IPublishedCache` abstraction (Locator) | **Done** | Split into typed `IPublishedContentCache`/`IPublishedMediaCache` |
| `IPublishedSnapshotAccessor` removal | **Done** | All files updated |
| `IPublishedCache.GetAtRoot()` | **Done** | `IDocumentNavigationQueryService.TryGetRootKeys` |
| Block List API (`BlockGridValue`, `BlockItemData.Values`) | **Done** | BlockValueExtensions, BlockItemDataExtensions |
| Block preview controller | **Done** | Cache lookups, language service, controller base |
| `ILocalizationService` → `ILanguageService` | **Done** | Block preview, localization accessor, cloud |
| Auth0 BackOffice security namespace | **Done** | `Umbraco.Cms.Api.Management.Security`; `SchemeForBackOffice` now static |
| Auth0 removed options (`Icon`, `AutoRedirectLoginToExternalProvider`) | **Done** | Both removed |
| URL providers (Blog, Events, Vacancies) | **Done** | `NewDefaultUrlProvider`, abstract `Alias`, virtual `GetPreviewUrlAsync` |
| `Link.Content` → `Link.Url` | **Done** | |
| `DataEditorAttribute` 1-arg constructor | **Done** | All custom data editors |
| `ConfigurationEditor` `ioHelper`-only constructor | **Done** | |
| `ContentTypeSort.Id` → `.Key` | **Done** | |
| `DataEditor.Name` → `.Alias` | **Done** | |
| `DateTimeConfiguration.Format` removal | **Done** | Separate DateOnly/DateTime aliases |
| `Aliases.TinyMce` → `Aliases.RichText` | **Done** | |
| `MultiNodePicker.StartNodeId` → `Guid?` | **Done** | `.ToId()` removed |
| Nested Content extension stubs (throw `NotSupportedException`) | **Done** | Guard rails in place |
| `PropertyBuilder.Nested.cs` stubs | **Done** | Throws `NotSupportedException` |
| DynamicListViews → Bellissima | **Done** | `umbraco-package.json` + Lit `workspaceView` + `condition` |
| **Contentment** → `Umbraco.Community.Contentment 6.1.4` | **Done** | Package renamed; `AddContentment()` restored; API updated |
| **Konstrukt → Umbraco.UIBuilder 17.2.0** | **Done** | All value mappers, configurator, composer, data builders ported |
| Engage infrastructure namespaces | **Confirmed unchanged** | `Umbraco.Engage.Infrastructure.*` unchanged in v17.2.2 |
| Perplex.ContentBlocks → v4.0.0-rc.3 | **Done** | `ElementTypeKey`, `BlockNameTemplate`, `ContentBlocksValue` API updated |
| uSync → 17.3.6 | **Done** | |
| Umbraco.Workflow → 17.0.2 | **Done** | |
| Azure Blob Storage → 17.0.0 | **Done** | |
| GMaps → 17.0.0 | **Done** | |
| Umbraco.Forms → 17.0.x | **Done** | Aligned to 17.0.1 (Forms + StaticAssets bumped to match Forms.Core; verified on nuget.org) (2026-06-02) |
| Umbraco.Engage → 17.2.2 | **Done** | Package upgraded; namespaces confirmed valid |
| `Directory.Build.targets` runtime stripping | **Done** | Strips `Umbraco.Web.BackOffice.dll` (v13) from output |
| `SaveAndPublish` → `Save` + `Publish` | **Done** | |
| Marketing `UmbracoEngageSegmentsDataSource` | **Done** | `ContentmentConfigurationField` return type fix |
| Contentment `IDataPickerSource` v6 API | **Done** | `GetItems()`, `SearchAsync` → `PagedViewModel<T>`, `Fields` type |
| **App starts + HTTP 200** | **Done** | Verified: Umbraco boots, Hangfire starts, OpenIddict running |
| `NumberFormatter` eager DB hit at startup | **Done** | `NumberFormat` property made lazy (`??=`); DB only hit on first format call |
| `DateTimeFormatter` eager DB hit at startup | **Done** | `DateFormat`/`TimeFormat`/`Timezone` properties made lazy (`??=`) |
| `UIBuilderComposer` double-registration crash | **Done** | Removed manual configurator loop; UIBuilder auto-discovers `IConfigurator` |
| `KonstruktConfigurator.GetContentSection` | **Done** | `ConditionalWeakTable` ensures `WithSection("content")` called once per builder |
| `launchSettings.json` `dotnetRunMessages` type | **Done** | Fixed `"true"` string → `true` boolean |
| **Assembly `<Version>` tags** | **Done (placeholder, 2026-06-02)** | Bulk-set `13.0.0`→`17.0.0` across 114 csproj; re-stamp CalVer before NuGet publish |
| **All App_Plugins AngularJS → Bellissima** | **Largely done (2026-06-02)** | 15/16 areas migrated to `umbraco-package.json` + Lit (telethon blocked on RR-02). Dashboards + Data property editors verified live. Property-editor UI alias must = backend `[DataEditor]` alias. Detail: `BELLISSIMA_MIGRATION_LOG.md` |
| Content app registration (Import/Export/Preview) | **Done (migrated)** | Now `workspaceView` extensions (Data.Import, Data.Export, Cloud.Platforms.Preview); not yet live-rendered (need a content node) |
| Dashboard registration (Scheduler, Welcome) | **Done + verified live** | `dashboard` extensions; both render in the backoffice (Welcome=Content, Scheduler=Settings/Hangfire iframe) |
| Property editor UI alias = backend `[DataEditor]` alias | **Done** | Critical fix: data types store `editorUiAlias`; all 8 custom editors re-aliased to backend alias (TextResourceEditor → `N3O.Umbraco.TemplateTextEditor`) |
| Cropper / Uploader native-picker migration | **PENDING DECISION** | Ported 1:1 (cropperjs/Formstone need global jQuery); may switch to Umbraco native media/image picker — awaiting Talha. Header comment added in `cropper.js`/`uploader.js` |
| Campaign/Offering backoffice notifications | **Not started** | `SendingContentNotification` stubs; Bellissima workspace views needed |
| TelethonOnAir Cockpit factory | **Done (2026-06-02)** | `TelethonOnAirCockpitSegmentRuleFactory` implemented + registrations re-enabled; v17.2.2 API verified by reflection. AngularJS telethon UI still blocked. |
| `GetNestedPropertySchemaHandler` | **Done — removed (2026-06-02)** | Was dead (zero callers); handler + query deleted. `NestedSchemaRes`/mapping kept (still live). |
| Content data migration (Nested Content → Block List) | **Not started (per-site run)** | Migration class written + now registered; still needs per-site backup/dry-run/run |
| Nested Content DB migration registration | **Done (2026-06-02)** | `N3ONestedContentMigrationPlan` added; auto-discovered via `IDiscoverable`. Runs on startup; no-ops without NC data types. |
| uSync Publisher v17 (`SyncContentHandler`) | **Blocked** | `IPublisherStateService` removed; Jumoo API not yet documented |
| Perplex stable release | **Blocked** | On v4.0.0-rc.3; waiting for stable v4 |

---

## AngularJS → Bellissima Migration (Largest Remaining Work)

All `package.manifest` files and AngularJS controllers must be replaced. In Umbraco 14+ (Bellissima), the backoffice uses Lit-based web components registered via `umbraco-package.json`. AngularJS APIs (`angular.module`, `$scope`, `$http`, `editorState`, `assetsService`) are completely gone.

### Extension type mapping

| Old AngularJS pattern | Bellissima replacement |
|---|---|
| Property editor controller + `package.manifest` `propertyEditors` | `propertyEditorUi` extension in `umbraco-package.json` + Lit `UmbPropertyEditorUiElement` |
| Content App (`IContentAppFactory` + controller) | `contentApp` extension in `umbraco-package.json` + Lit web component |
| Dashboard (`IDashboard` + controller + view) | `dashboard` extension in `umbraco-package.json` + Lit web component |
| Section/Tree registration | `section`, `menuItem`, `treeItem` extensions |
| Workspace view | `workspaceView` extension (replaces `SendingContentNotification` tab injection) |
| Notification/tab injection (`SendingContentNotification`) | `workspaceView` or `workspaceAction` extension |
| `assetsService.loadJs/Css` | Bundle dependencies into the Lit component or import as ES module |

### Files to migrate (per project area)

| Project / Plugin | Files | Extension type | Notes |
|---|---|---|---|
| `Blocks.StaticAssets` / `N3O.Umbraco.Blocks.Preview` | `block-preview.controller.js`, `bind-compile.directive.js`, `block-preview.html`, `package.manifest` | `blockEditorCustomView` | Uses `editorState`, `$sce`, `umbRequestHelper`; backend endpoint unchanged |
| `Cloud.Platforms.StaticAssets` / `N3O.Umbraco.Cloud.Platforms.Preview` | `N3O.Umbraco.Cloud.Platforms.Preview.Controller.js`, `package.manifest` | `contentApp` | |
| `Cloud.Platforms.Marketing.StaticAssets` / `telethon-on-air-rule` | `segment-rule-telethon-on-air.js`, `-editor.js`, `-display.js`, `package.manifest` | Engage segment rule UI | **Blocked on Engage BLOCKER-04 (cockpit factory empty)** |
| `Data.StaticAssets` / `N3O.Umbraco.Data.Export` | Controller.js + `package.manifest` | `contentApp` | Uses `editorState`, `assetsService` |
| `Data.StaticAssets` / `N3O.Umbraco.Data.Import` | Controller.js + `package.manifest` | `contentApp` | |
| `Data.StaticAssets` / `N3O.Umbraco.Data.ImportDataEditor` | Controller.js + `package.manifest` | `propertyEditorUi` | |
| `Data.StaticAssets` / `N3O.Umbraco.Data.ImportNoticesViewer` | Controller.js + `package.manifest` | `propertyEditorUi` | |
| `Plugins/Cells` / `N3O.Umbraco.Cells` | Controller.js + `package.manifest` | `propertyEditorUi` | Wraps Handsontable; load via ES import |
| `Plugins/Cropper` / `N3O.Umbraco.Cropper` | Controller.js + `package.manifest` | `propertyEditorUi` | Uses Formstone; reuse as ES module |
| `Plugins/EditorJs` / `N3O.Umbraco.EditorJs` | Controller.js + `package.manifest` | `propertyEditorUi` | Uses editorjs bundle; load via ES import |
| `Plugins/SerpEditor` / `N3O.Umbraco.SerpEditor` | Controller.js + `package.manifest` | `propertyEditorUi` | |
| `Plugins/TextResourceEditor` | Controller.js + `package.manifest` | `propertyEditorUi` | |
| `Plugins/Uploader` / `N3O.Umbraco.Uploader` | Controller.js + `package.manifest` | `propertyEditorUi` or `dashboard` | |
| `Plugins/WelcomeDashboard` | Controller.js (empty controller) + `package.manifest` | `dashboard` | |
| `Scheduler.StaticAssets` / `N3O.Umbraco.Scheduler` | `N3O.Umbraco.Scheduler.html` (iframe) + _(no JS controller)_ | `dashboard` | Wrap Hangfire iframe in Lit component |
| `Blazor.BackOffice` | `package.manifest` only (JS is non-AngularJS Blazor loader) | `script` extension | Low risk; JS reusable; just update manifest format |

**Already migrated:** `N3O.Umbraco.DynamicListViews` — fully Bellissima with `umbraco-package.json`, `LitElement`, `UmbElementMixin`.

---

## Remaining Runtime Issues

### RR-01 — ✅ DONE (2026-06-02) — `SyncContentHandler` (uSync Publisher v17)
- `src/Sync/N3O.Umbraco.Sync.Extensions/Handlers/SyncContentHandler.cs` + `SyncExtensionsComposer.cs`
- Reimplemented against the real uSync.Publisher v17 API (discovered by reflection — Jumoo publishes no docs for it). Injects `PublisherProcessor` (wraps `Jumoo.Processing.Core.Pipelines.IPipelineService`), calls `Process(PublisherActionRequest, PublisherProcessingOptions)` → `SyncPublishResponse`, same Document-UDI push w/ published deps, throws on `!Success`. Build 0 errors; boots clean. See BLOCKER-05 for the API map + the one runtime caveat (multi-step pipeline completion needs an end-to-end test with a remote uSync server).

### RR-02 — ✅ DONE (2026-06-02) — `PlatformsMarketingComposer` + `TelethonOnAirCockpitSegmentRuleFactory` (Engage)
*Implemented: factory written against verified v17.2.2 API (`.Rules` sub-namespace; `out CockpitSegmentRule?`), both DI registrations re-enabled, build 0 errors. AngularJS telethon UI still blocked (BLOCKER-04 client side).*
- `src/Cloud/N3O.Umbraco.Cloud.Platforms.Marketing/PlatformsMarketingComposer.cs` — both `ICockpitSegmentRuleFactory` and `ISegmentRuleFactory` registrations are commented out.
- `src/Cloud/N3O.Umbraco.Cloud.Platforms.Marketing/Services/Campaigns/TelethonOnAirCockpitSegmentRuleFactory.cs` — empty file.
- **Fix:** Implement `TelethonOnAirCockpitSegmentRuleFactory` against `ICockpitSegmentRuleFactory` (namespace: `Umbraco.Engage.Web.Cockpit.Segments`); update `TryCreate` to the v17 signature (nullable out param); re-enable both DI registrations in the Composer.
- **Note:** `ISegmentRuleFactory`, `ISegmentRule`, `BaseSegmentRule`, `ISegmentRepository`, `IPersonalizationProfile` namespaces are all **unchanged** in Engage 17.2.2.

### RR-03 — ✅ DONE (2026-06-02, removed) — `GetNestedPropertySchemaHandler` throws `NotImplementedException`
*Verified dead (zero callers solution-wide); handler + `GetNestedPropertySchemaQuery` deleted. `NestedSchemaRes`/`NestedSchemaResMapping` kept (still live via `NestedValueResMapping`).*
- `src/Data/N3O.Umbraco.Data/Handlers/Content/GetNestedPropertySchemaHandler.cs` line 13.
- **Fix:** Either implement for Block List schema export, or remove the handler and its associated query/response types if no longer needed.

### RR-04 — Content Apps not registered (Import, Export, Platforms Preview)
- C# stubs exist (namespace-only files with TODO comments). No `umbraco-package.json` created.
- **Fix:** Create `umbraco-package.json` `contentApp` extension entry + Lit web component for each. The backend API endpoints in their controllers are functional.

### RR-05 — Campaign/Offering backoffice notifications absent
- `src/Cloud/N3O.Umbraco.Cloud.Platforms/Notifications/Campaigns/CampaignSending.cs` and `OfferingSending.cs` — stub files.
- **Fix:** Implement as Bellissima `workspaceView` or `workspaceAction` extensions in `umbraco-package.json`.

### RR-06 — Dashboards not registered (Scheduler, WelcomeDashboard)
- Both `SchedulerDashboard.cs` and `WelcomeDashboard.cs` are namespace-only stubs.
- **Fix:** Create `umbraco-package.json` `dashboard` extension + Lit component for each.

### RR-07 — ✅ DONE (2026-06-02) — `GetPreviewUrlAsync` returns null
*Base `UrlProvider.GetPreviewUrlAsync` now mirrors core `NewDefaultUrlProvider` using `this.Alias` (key-based preview); covers Blog/Events/Vacancies, no per-subclass overrides needed.*
- `src/N3O.Umbraco.Extensions/UrlProviders/UrlProvider.cs` base implementation returns `null`.
- **Fix:** Implement per subclass or provide a base implementation deriving from `GetUrl`.

### RR-08 — ✅ DONE (2026-06-02) — Data controllers unauthenticated (pre-existing, security risk)
*Added `[Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]` to ContentController/ContentTypesController/DataTypesController (base `ApiController` kept; route stays `/umbraco/api/...`). Unauthenticated callers now get 401.*
- `ContentController.cs`, `ContentTypesController.cs`, `DataTypesController.cs` — all have `// TODO Add authentication`.
- **Fix:** Add `[Authorize]` attribute or equivalent. Pre-existing, but must resolve before any internet-facing deployment.

### RR-09 — ✅ ALREADY DONE — `SaveAndPublish` bare exception (pre-existing)
*Current code throws `InvalidOperationException` surfacing `EventMessages` + `StatusType` (`Result`) for both save and publish failures. No action needed.*
- `src/N3O.Umbraco.Extensions/Extensions/ContentServiceExtensions.cs` — throws bare `Exception` discarding `PublishResult` detail.
- **Fix:** Surface `EventMessages`/`StatusType` in the exception.

### RR-10 — ⚠️ DECISION NEEDED (2026-06-02 analysis) — `Bundling` service is entirely non-functional
*Investigated: `N3O.Umbraco.Bundling` is fully orphaned — zero consumers anywhere (no ProjectReference/PackageReference, no `IAssetBundle` impls, no `@addTagHelper`). Only artifact: two inert `<n3o-css-bundle/>`/`<n3o-js-bundle/>` literals in DemoSite `Layout.cshtml` (lines 30, 52) that don't bind (tag helpers unregistered). Recommend **delete the project** (+ its 2 sln entries/GUIDs `{C52E624B-...}`,`{9D94C90F-...}` + the 2 cshtml lines), OR build a Vite/ESM replacement. Not actioned — awaiting decision.*
- `src/Bundling/N3O.Umbraco.Bundling/Services/Bundler.cs` — all methods throw `NotSupportedException("Smidge bundling was removed in Umbraco 14")`.
- `src/Bundling/N3O.Umbraco.Bundling/AssetBundle.I.cs` — stub interface.
- **Fix:** If bundling is still needed by any consumer, implement via Vite/esbuild/native ES modules. If no consumers remain, delete both files.

---

## Package Status

| Package | Version | Status | Action |
|---|---|---|---|
| `Umbraco.Cms` | 17.3.5 | OK | — |
| `Umbraco.Community.Contentment` | 6.1.4 | **Done** | Was BLOCKER-01; resolved with package rename |
| `Umbraco.UIBuilder` | 17.2.0 | **Done** | Was BLOCKER-03 (Konstrukt); port complete |
| `Umbraco.Engage` | 17.2.2 | **Done (package)** | Namespaces confirmed valid; Cockpit factory needs implementing |
| `Umbraco.Forms` | 17.0.0 / 17.0.1 | OK (minor skew) | Align both projects to same patch version |
| `uSync.Complete` | 17.3.6 | OK | — |
| `Umbraco.Workflow` | 17.0.2 | OK | — |
| `Umbraco.StorageProviders.AzureBlob` | 17.0.0 | OK | — |
| `Our.Umbraco.GMaps` | 17.0.0 | OK | API changes unverified — test in backoffice |
| `Perplex.ContentBlocks` | 4.0.0-rc.3 | **Pre-release** | Upgrade to stable v4.x when released |
| `Umbraco.Code` | 2.4.0 | **Unconfirmed** | Verify v17 compat; drop if only design-time |
| `Diplo.GodMode` | 17.0.0 | OK | — |
| `uSync.Publisher` | n/a | **Blocked** | `IPublisherStateService` removed; v17 API not documented |
| All `<Version>` tags | 13.0.0 | **Must fix** | Update to `17.x.x.x` before any NuGet publish |

---

## Content Data Migration

Per consuming site — code-only package migration is insufficient.

**CM-01 — Nested Content → Block List (Critical, blocking for live data)**
- Migration class: `src/N3O.Umbraco.Extensions/Migrations/NestedContentToBlockListMigration.cs`
- **Still needed:** Register in an `AsyncPackageMigrationBase` / `IMigrationPlan`. Example:
```csharp
public class N3ONestedContentMigrationPlan : PackageMigrationPlan {
    public N3ONestedContentMigrationPlan() : base("N3O.Umbraco.NestedToBlockList") { }
    protected override void DefinePlan() {
        To<NestedContentToBlockListMigration>("2026-NestedContent-v1");
    }
}
```
- Checklist per site: back up DB → verify Block List data types exist → dry-run on copy → run → regenerate uSync XML.

**CM-02 — uSync XML for Nested Content data types**
- Regenerate after running CM-01: `uSync export` → commit to source control → verify clean import.

**CM-03 — Engage analytics/personalisation data**
- Follow Umbraco Engage v13→v17 upgrade guide; run Engage DB migrations in order.

**CM-04 — Block Editor v15 format + UTC dates (auto)**
- v15 auto-migrates Block Editor data; v17 auto-migrates dates to UTC. Both run on first startup — schedule a maintenance window and back up first.

**CM-05 — RTE: TinyMCE → TipTap (review)**
- v16 auto-migrates data types; review RTE-dependent Razor/email/feed output.

---

## Recommended Work Order

**Phase 1 — Bellissima frontend (largest remaining effort, can parallelise)**
1. Migrate all 15 AngularJS plugin areas to Lit web components + `umbraco-package.json` (see AngularJS table above).
2. Register content apps (Import, Export, Platforms Preview) via `umbraco-package.json`.
3. Register dashboards (Scheduler, WelcomeDashboard) via `umbraco-package.json`.
4. Implement Campaign/Offering workspace views via `umbraco-package.json`.

**Phase 2 — Runtime correctness**
5. RR-02: Implement `TelethonOnAirCockpitSegmentRuleFactory` + re-enable Composer registrations.
6. RR-03: Implement or remove `GetNestedPropertySchemaHandler`.
7. RR-07: Implement `GetPreviewUrlAsync` in URL provider subclasses.
8. RR-09: Fix `SaveAndPublish` exception handling.
9. RR-08: Add authentication to Data API controllers.
10. RR-10: Evaluate Bundling — implement or delete.

**Phase 3 — Package cleanup**
11. Align `Umbraco.Forms` versions (17.0.0 → 17.0.1 everywhere).
12. Upgrade `Perplex.ContentBlocks` to stable v4.x when released.
13. Verify `Umbraco.Code` 2.4.0 / `Our.Umbraco.GMaps` 17.0.0 in backoffice.
14. Bump all `<Version>` tags from `13.0.0` to `17.x.x.x` before any NuGet publish.

**Phase 4 — Content data migration (per site)**
15. CM-01: Register + run `NestedContentToBlockListMigration`.
16. CM-02: Regenerate uSync XML post-migration.
17. CM-03 / CM-04 / CM-05: Engage data, Block format, UTC dates, TipTap review.

**Phase 5 — Blocked items (when unblocked)**
18. RR-01: Implement `SyncContentHandler` against new uSync.Publisher v17 API (Jumoo.Processing) when documented.

---

## Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Bellissima frontend rewrite scope underestimated | High | High | 15+ AngularJS controllers; each needs Lit + TypeScript; start early with the highest-traffic plugins (Data Import/Export, Block Preview) |
| uSync Publisher v17 API never documented / breaking | Medium | High | Monitor Jumoo; isolate `SyncContentHandler` behind a feature flag so the rest of the site works without it |
| Perplex ContentBlocks v4 stable not released | Low | Medium | v4.0.0-rc.3 is functional; assess whether RC is acceptable for production |
| Engage Cockpit factory implementation harder than expected | Low | Medium | `ICockpitSegmentRuleFactory` and `IPersonalizationProfile` are confirmed in Engage 17.2.2; spike the implementation first |
| GMaps v17.0.0 API changes break map editors | Medium | Medium | Test map property editor in backoffice before shipping |
| Nested Content content-migration data loss | Medium | High | Idempotent migration + dry-run on DB copy + before/after content review |
| Umbraco.Code 2.4.0 not v17-compatible | Low–Med | Medium | Verify; if design-time only, commit generated code and drop reference |
| Assembly version `13.0.0` causing consumer confusion | High | Low | Global find/replace in csproj before any package publish |

---

## Definition of Done

Migration is complete when:

- [x] All 120 projects target `net10.0`
- [x] Solution builds with 0 errors
- [x] No removed C# APIs used (UmbracoApiController, IPublishedCache, IPublishedSnapshotAccessor, NestedContentConfiguration, abstract BlockValue, BlockItemData.PropertyValues, etc.)
- [x] All third-party packages on v17-compatible versions (Contentment 6.1.4, UIBuilder 17.2.0, Engage 17.2.2, Forms 17.x, uSync 17.3.6, Workflow 17.0.2, AzureBlob 17.0.0, GMaps 17.0.0, Perplex 4.x)
- [x] All AngularJS `package.manifest` + controllers replaced with `umbraco-package.json` + Lit web components *(15/16; telethon blocked on BLOCKER-04)*
- [~] Content apps (Import, Export, Platforms Preview) registered as `workspaceView` — **migrated; live-render test pending (needs content node)**
- [x] Dashboards (Scheduler, WelcomeDashboard) registered and **verified rendering** in backoffice
- [~] Property editors render live in a content workspace — **pending content/data-type fixtures** (Cells, Cropper, EditorJs, SerpEditor, TextResourceEditor, Uploader; Data editors' data types resolve)
- [ ] **Decision:** Cropper/Uploader — keep bundled cropperjs/Formstone (needs jQuery) or switch to Umbraco native media/image picker
- [ ] Campaign/Offering workspace views implemented
- [ ] `TelethonOnAirCockpitSegmentRuleFactory` implemented and registered
- [ ] `GetNestedPropertySchemaHandler` implemented or removed
- [ ] `GetPreviewUrlAsync` implemented for all URL provider subclasses
- [ ] Data controllers authenticated
- [ ] Bundling service implemented or removed
- [ ] `SaveAndPublish` has production-grade error handling
- [ ] All `<Version>` tags updated from `13.0.0` to `17.x`
- [ ] `uSync.Publisher` `SyncContentHandler` reimplemented (when Jumoo docs available)
- [ ] `Perplex.ContentBlocks` on stable v4.x
- [ ] Nested Content → Block List data migration registered, tested, run per site
- [ ] uSync XML regenerated post-migration
- [ ] Engage data migration run
- [ ] Auto-migrations (Block format, UTC, TipTap) validated on a DB copy
- [ ] Manual backoffice regression testing: block preview, content apps, dashboards, import/export, segment rules, map editor, Forms, Workflow, uSync round-trip
- [ ] At least one downstream client site builds and runs end-to-end

---

*Effort note: The Bellissima frontend rewrite (Phase 1) is the largest remaining effort — 15 plugin areas, each requiring a new Lit web component. Treat it as a dedicated frontend sprint.*
