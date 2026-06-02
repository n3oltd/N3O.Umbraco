# Migration Blockers

*Last updated: 2026-06-01*

Items that require external action, design decisions, or significant implementation effort before full production readiness.

---

## RESOLVED

### ~~BLOCKER-01: Contentment~~ — RESOLVED
- **Old state:** No Umbraco 17 release; `Our.Umbraco.Community.Contentment.Core 4.7.0` crashed TypeFinder via `Umbraco.Web.BackOffice v13` IL reference.
- **Resolution:** Package renamed to `Umbraco.Community.Contentment`. Upgraded to **6.1.4** (targets Umbraco 17, net10.0). `.AddContentment()` restored. Contentment API updated: `ContentmentConfigurationField` (in `Umbraco.Cms.Core.PropertyEditors`), `PagedViewModel<T>` (in `Umbraco.Cms.Api.Common.ViewModels.Pagination`), `GetItems()` sync method added.
- **MSBuild strip target** in `Directory.Build.targets` confirmed no longer needed for Contentment; kept as safety net only.

### ~~BLOCKER-03: Konstrukt → Umbraco.UIBuilder~~ — RESOLVED
- **Old state:** `Konstrukt.Startup 1.6.7` renamed to `Umbraco.UIBuilder` in v14+; full API port needed.
- **Resolution:** All Konstrukt → UIBuilder API ports complete:
  - `KonstruktConfigurator` → uses `UIBuilderConfigBuilder`, `WithSectionConfigBuilder`
  - `UIBuilderComposer` → `builder.AddUIBuilder(cfg => { ... })` with auto-discovery of `IConfigurator`
  - `KonstruktValueMapper` → `ValueMapper` (`Umbraco.UIBuilder.Mapping`)
  - `KonstruktDataViewsBuilder<T>` → `DataViewsBuilder<T>`; `KonstruktDataViewSummary` → `DataViewSummary`
  - `KonstruktEntitySaved/SavingNotification` → `EntitySaved/SavingNotification`
  - Data UIBuilder `ImportsConfigurator` fully ported.

---

## ACTIVE BLOCKERS

### BLOCKER-02: Perplex.ContentBlocks — Pre-release RC

**Status:** Pre-release — v4.0.0-rc.3 in use  
**Package:** `Perplex.ContentBlocks 4.0.0-rc.3`  
**Projects:** `N3O.Umbraco.Blocks.Perplex`, `N3O.Umbraco.Data.PerplexBlocks`

#### Current state
The v4.0.0-rc.3 package has been integrated and the solution compiles. API changes adopted:
- `PerplexBlockDefinition` implements `ElementTypeKey` (returns `Id`) and `BlockNameTemplate` (returns `null`)
- `ContentBlocksModelValue` → `ContentBlocksValue`; namespace `Perplex.ContentBlocks.PropertyEditor.Value`
- `DataType.Configuration` property removed; `CreateDataTypes` method removed from `PerplexBlockTypesService`
- Dead code: `GetOrCreateDataTypeContainer` private method in `PerplexBlockTypesService` is now unreachable — **safe to delete**.

#### Issue
RC releases should not be used in production. A stable v4.x release is needed.

**Note:** Data types that were previously created by code (via `CreateDataTypes`) must now be managed via the Umbraco backoffice or uSync. Communicate this to all site teams.

#### Action required
1. Monitor https://github.com/PerplexDigital/Perplex.ContentBlocks for stable v4 release
2. When available: upgrade from rc.3 to stable; verify API compatibility
3. Delete dead `GetOrCreateDataTypeContainer` method from `PerplexBlockTypesService.cs`
4. Document for site teams: Perplex data types are no longer auto-created; manage via backoffice/uSync

---

### BLOCKER-04: Engage Cockpit Segment Rule Factory (Partially resolved)

**Status:** Engage package upgraded; namespaces confirmed valid; Cockpit factory implementation missing  
**Package:** `Umbraco.Engage 17.2.2` (installed)  
**Projects:** `N3O.Umbraco.Cloud.Platforms.Marketing`

#### Current state (what IS working)
The Engage package upgrade to v17.2.2 is complete. DLL inspection confirms all three namespaces used by the codebase are **unchanged** in v17.2.2:
- `Umbraco.Engage.Infrastructure.Personalization.Segments` → `ISegmentRepository`, `Segment` ✅
- `Umbraco.Engage.Infrastructure.Personalization.Segments.Rules` → `ISegmentRuleFactory`, `ISegmentRule`, `BaseSegmentRule`, `SegmentRuleValidationMode` ✅
- `Umbraco.Engage.Infrastructure.Personalization.PersonalizationProfile` → `IPersonalizationProfile` ✅

`UmbracoEngageSegmentsDataSource.cs` (Marketing project) builds and runs correctly.

#### What is broken
`PlatformsMarketingComposer.cs` has both factory registrations commented out:
```csharp
// TODO (BLOCKER-04): Umbraco.Engage namespaces changed in v17
// builder.WithCollectionBuilder<SegmentRuleCollectionBuilder>().Add<TelethonOnAirSegmentRuleFactory>();
// builder.WithCollectionBuilder<CockpitSegmentRuleCollectionBuilder>().Add<TelethonOnAirCockpitSegmentRuleFactory>();
```

`TelethonOnAirCockpitSegmentRuleFactory.cs` is **an empty file** — the class body needs implementing.

The Cockpit factory interface in v17.2.2 is at: `Umbraco.Engage.Web.Cockpit.Segments.ICockpitSegmentRuleFactory`

#### Action required
1. Read `ICockpitSegmentRuleFactory` interface definition (in `Umbraco.Engage.Web.Cockpit.Segments` namespace)
2. Implement `TelethonOnAirCockpitSegmentRuleFactory` — specifically update `TryCreate` to the v17 signature (nullable out parameter)
3. Re-enable both `Add<>()` registrations in `PlatformsMarketingComposer.cs`
4. Rewrite the 3 AngularJS segment rule editor JS files (`segment-rule-telethon-on-air*.js`) as Bellissima web components

#### AngularJS UI still blocked
The three JS files in `App_Plugins/telethon-on-air-rule/` are AngularJS and will not run in Umbraco 17 backoffice. These depend on `umsSegmentRuleRepository` (an Engage Angular service). Check what the Engage v17.2.2 client-side API provides for custom segment rule UIs.

---

### BLOCKER-05: uSync Publisher — `SyncContentHandler`

**Status:** Blocked on Jumoo — v17 API not publicly documented  
**Package:** uSync.Publisher (separate from `uSync.Complete 17.3.6`)  
**File:** `src/Sync/N3O.Umbraco.Sync.Extensions/Handlers/SyncContentHandler.cs`

#### What is broken
`IPublisherStateService` was removed in uSync.Publisher v17. The `SyncContentHandler.Handle()` method currently throws `NotSupportedException` at runtime. Any attempt to sync content via uSync Publisher will crash.

```csharp
public void Handle(ContentSavedNotification notification) {
    throw new NotSupportedException("IPublisherStateService removed in uSync.Publisher v17. Needs reimplementation against Jumoo.Processing v17 API.");
}
```

#### Action required
1. Monitor https://jumoo.co.uk and https://github.com/PerplexDigital for uSync Publisher v17 documentation
2. When available: reimplement `SyncContentHandler` against the `Jumoo.Processing` v17 API
3. Until then: consider feature-flagging the handler so it is only registered in environments where Publisher is not needed

---

### BLOCKER-06: Nested Content Database Migration

**Status:** Migration code written; not registered; not run on any site  
**File:** `src/N3O.Umbraco.Extensions/Migrations/NestedContentToBlockListMigration.cs`

#### What is needed
For every existing client site, Nested Content property values must be converted to Block List JSON before the site can render content correctly on v17.

#### Registration (not yet done)
```csharp
public class N3ONestedContentMigrationPlan : PackageMigrationPlan {
    public N3ONestedContentMigrationPlan() : base("N3O.Umbraco.NestedToBlockList") { }
    protected override void DefinePlan() {
        To<NestedContentToBlockListMigration>("2026-NestedContent-v1");
    }
}
```
This plan needs to be registered in a Composer via `builder.PackageMigrationPlans().Add<N3ONestedContentMigrationPlan>()`.

#### Pre-migration checklist per site
- [ ] Back up the database
- [ ] Verify all Nested Content data types have a corresponding Block List data type configured with the same element types
- [ ] Run migration on a database copy first
- [ ] Verify rendered output before and after in staging
- [ ] Regenerate uSync XML after migration (run `uSync export`, commit the result)

---

### BLOCKER-07: Bellissima Frontend — All AngularJS Plugins

**Status:** Largely DONE (2026-06-02) — 15 of 16 plugin areas migrated to `umbraco-package.json` + Lit web components; only `telethon-on-air-rule` remains blocked (depends on BLOCKER-04). Build 0 errors; app boots clean; all assets served HTTP 200; WelcomeDashboard + Scheduler dashboards render live; both Data property-editor data types resolve. See `BELLISSIMA_MIGRATION_LOG.md` for the per-plugin table, the systematic alias fix, and the remaining live-render checklist (property editors / workspace views / Blocks.Preview need content fixtures to render — demo DB has no content). Migration guide: `BELLISSIMA_MIGRATION_GUIDE.md`.

**Critical fix discovered during testing:** each custom `propertyEditorUi.alias` (and `propertyEditorSchemaAlias`) must equal the backend `[DataEditor]` alias (NOT a new `N3O.PropertyEditorUi.*`), else existing data types show "Property Editor UI not found". Applied to all custom editors (TextResourceEditor's backend alias is `N3O.Umbraco.TemplateTextEditor`).

**Original impact:** All custom property editors, content apps, and dashboards were invisible or non-functional in the Umbraco 17 backoffice.

#### Summary
Umbraco 14+ (Bellissima) replaced the entire AngularJS backoffice with Lit-based web components. None of the N3O custom property editors, content apps, or dashboards currently work.

#### Plugin areas and effort

| Plugin | Extension type | Complexity | AngularJS APIs used |
|---|---|---|---|
| Block Preview | `blockEditorCustomView` | High | `$scope`, `$sce`, `$timeout`, `editorState`, `umbRequestHelper` |
| Data Export | `contentApp` | Medium | `$scope`, `editorState`, `assetsService` |
| Data Import | `contentApp` | Medium | `$scope`, `editorState`, `assetsService` |
| Data ImportDataEditor | `propertyEditorUi` | Medium | `$scope`, `assetsService` |
| Data ImportNoticesViewer | `propertyEditorUi` | Low | Simple display |
| Platforms Preview | `contentApp` | Medium | `$scope`, `editorState` |
| Cells (Handsontable) | `propertyEditorUi` | High | `$scope`, `assetsService`, Handsontable lib |
| Cropper | `propertyEditorUi` | High | `$scope`, `assetsService`, `$timeout`, Formstone |
| EditorJs | `propertyEditorUi` | High | `$scope`, `assetsService`, EditorJs bundle |
| SerpEditor | `propertyEditorUi` | Medium | `$scope`, `assetsService`, `editorState` |
| TextResourceEditor | `propertyEditorUi` | Low | Simple text editor |
| Uploader | `propertyEditorUi`/`dashboard` | Medium | `$scope`, `assetsService`, Formstone |
| WelcomeDashboard | `dashboard` | Low | Empty controller — almost no logic |
| Scheduler Dashboard | `dashboard` | Low | HTML iframe wrapping Hangfire UI |
| Blazor BackOffice | `script` | Low | Non-AngularJS; just needs manifest format update |
| TelethonOnAir segment rule | Engage extension | High | Engage-specific Angular services — also blocked by BLOCKER-04 |

#### Approach
For each plugin:
1. Create a Lit `customElement` class (TypeScript recommended)
2. Replace `$scope` state with Lit reactive properties (`@property()`, `@state()`)
3. Replace `assetsService.loadJs/Css` with ES module `import` or bundled dependencies
4. Replace `editorState` access with Umbraco's `UmbDocumentWorkspaceContext`
5. Register in `umbraco-package.json` with the correct extension type alias
6. Delete the old `.js` controller, `.html` view, and `package.manifest`

#### Resources
- Umbraco backoffice package docs: https://docs.umbraco.com/umbraco-cms/extending/backoffice-setup
- Umbraco UI Library (Lit components): https://uui.umbraco.com
- Extension type reference: https://docs.umbraco.com/umbraco-cms/extending/backoffice-setup/extension-types

---

### BLOCKER-08: Umbraco Forms Subscription License

**Status:** Procurement action required  
**Package:** `Umbraco.Forms 17.0.x`

From Umbraco v17, the perpetual license model for Umbraco Forms is no longer valid. A subscription license is required per site.

#### Action required
1. Contact Umbraco sales: https://umbraco.com/products/umbraco-forms/
2. Obtain a subscription license per client site using Forms
3. Configure the license key in `appsettings.json` before deploying v17

---

### BLOCKER-09: Assembly Version Tags (`13.0.0`)

**Status:** Must fix before any NuGet publish  
**Scope:** All 120 `.csproj` files

Every project has `<Version>13.0.0</Version>`, `<AssemblyVersion>13.0.0</AssemblyVersion>`, `<FileVersion>13.0.0</FileVersion>`. These are CalVer-style and need updating to `17.x.x.x` before publishing any packages.

#### Action required
```powershell
# Bulk update all csproj version tags
Get-ChildItem "D:\AI Migration Test\N3O.Umbraco\src" -Recurse -Filter "*.csproj" |
    ForEach-Object {
        $content = Get-Content $_.FullName -Raw
        $updated = $content -replace '<Version>13\.0\.0</Version>', '<Version>17.0.0</Version>'
        $updated = $updated -replace '<AssemblyVersion>13\.0\.0</AssemblyVersion>', '<AssemblyVersion>17.0.0</AssemblyVersion>'
        $updated = $updated -replace '<FileVersion>13\.0\.0</FileVersion>', '<FileVersion>17.0.0</FileVersion>'
        if ($updated -ne $content) { Set-Content $_.FullName $updated }
    }
```
Use CalVer format `YYYY.M.D.Build` for the actual release version.

---

## Summary Table

| # | Blocker | Status | Urgency |
|---|---|---|---|
| ~~01~~ | ~~Contentment~~ | **RESOLVED** (6.1.4) | — |
| 02 | Perplex.ContentBlocks RC | Functional RC; waiting for stable | Medium |
| ~~03~~ | ~~Konstrukt → UIBuilder~~ | **RESOLVED** (port complete) | — |
| 04 | Engage Cockpit factory missing | Implementation needed | High |
| 05 | uSync Publisher v17 | Blocked on Jumoo docs | High |
| 06 | Nested Content DB migration | Code written; needs registration + per-site run | Critical |
| 07 | Bellissima frontend (all AngularJS) | **Largely done** (15/16 migrated; telethon blocked on 04; live-render fixtures pending) | High |
| 08 | Forms subscription license | Procurement | High |
| 09 | Assembly version `13.0.0` | Must fix before publish | Medium |
