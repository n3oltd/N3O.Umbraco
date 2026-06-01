# Migration Blockers — Items Without a v17 Alternative

These items cannot be fully resolved until external dependencies release v17-compatible versions, or until a replacement strategy is decided. Each section documents the situation, what currently works, and what action is needed.

---

## BLOCKER-01: Our.Umbraco.Community.Contentment

**Status:** Blocked on upstream release  
**Current version:** 4.7.0 (Umbraco 13 era)  
**Package:** `Our.Umbraco.Community.Contentment.Core`  
**Projects affected:** `N3O.Umbraco.Cms`, `N3O.Umbraco.Extensions`

### What is broken
No Umbraco 17-compatible release of Contentment exists. The v4.7.0 Core package transitively pulls in `Umbraco.Cms.Web.BackOffice 13.0.0`, which conflicts with the v17 management API and causes CS0121 ambiguity errors on `AddBackOffice`, `UseBackOffice`, `UseBackOfficeEndpoints`.

### Current workaround
An MSBuild target in `N3O.Umbraco.Cms.csproj` strips the conflicting v13 BackOffice assembly from the compiler reference path at build time:
```xml
<Target Name="ExcludeLegacyUmbracoBackOffice" AfterTargets="ResolveAssemblyReferences">
    <ItemGroup>
        <ReferencePath Remove="@(ReferencePath)"
                       Condition="$([System.String]::Copy('%(Identity)').ToLower().Contains('umbraco.cms.web.backoffice'))" />
        ...
    </ItemGroup>
</Target>
```
This allows the solution to **compile**, but Contentment-backed editors (`DataList`, `DataPicker`, etc.) will **not function at runtime** until a v17-compatible release is used.

### What is still used (audit before v17 release)
Search the codebase for all content types and property types using these editor aliases:
- `Umbraco.Community.Contentment.DataList`
- `Umbraco.Community.Contentment.DataPicker`
- `Umbraco.Community.Contentment.ContentBlocks` (if used)

Files currently referencing Contentment:
- `src/N3O.Umbraco.Extensions/Types/ContentmentDataSource.I.cs`
- `src/N3O.Umbraco.Extensions/Lookups/LookupsDataSource.cs`
- `src/N3O.Umbraco.Extensions/Extensions/ContentHelperExtensions.DataList.cs`
- `src/N3O.Umbraco.Extensions/Extensions/ContentHelperExtensions.DataPicker.cs`
- `src/N3O.Umbraco.Extensions/Extensions/PropertyTypeExtensions.cs` (IsDataList, IsDataPicker)

### Action required
1. Monitor https://github.com/leekelleher/umbraco-contentment for a v17 release
2. When available: remove the `ExcludeLegacyUmbracoBackOffice` MSBuild target, restore `.AddContentment()` in `CmsStartup.cs`, test all Contentment-backed editors
3. If no release appears: evaluate native replacements (Umbraco's built-in dropdown, Radio Button List, etc.) for each Contentment editor in use

---

## BLOCKER-02: Perplex.ContentBlocks

**Status:** RC available (v4.0.0-rc.2), stable not released  
**Current version:** 3.0.1 (Umbraco 13 era)  
**Package:** `Perplex.ContentBlocks`  
**Projects affected:** `N3O.Umbraco.Blocks.Perplex`, `N3O.Umbraco.Data.PerplexBlocks`

### What is broken
Perplex.ContentBlocks v3 targets Umbraco 13 and uses Nested Content APIs internally. These are removed in v17. Additionally, `NestedContentConfiguration` usages in `PerplexBlockTypesService.cs` (lines 141–157) reference removed types.

v4.0.0-rc.2 reportedly supports Umbraco 17, but as a release candidate it is not production-ready.

### Decision required
Choose one of:
1. **Wait for stable v4**: use `Perplex.ContentBlocks` 4.x stable when released. Update `PerplexBlockTypesService.cs` to use `BlockListConfiguration` and the v4 API.
2. **Replace with native Block Grid**: rebuild the Perplex module using Umbraco's Block Grid editor. This is significant work but removes the third-party dependency.
3. **Exclude from build**: comment out `N3O.Umbraco.Blocks.Perplex` and `N3O.Umbraco.Data.PerplexBlocks` from the solution temporarily until resolved.

### Current state in code
- `src/Blocks/N3O.Umbraco.Blocks.Perplex/Services/PerplexBlockTypesService.cs:141–157` — uses `NestedContentConfiguration.ContentTypes` (removed); also `CreateDataTypes` builds Nested Content data types programmatically
- `src/Data/N3O.Umbraco.Data.PerplexBlocks/` — data export/import for Perplex blocks

### Action required
1. Evaluate v4.0.0-rc.2 on a test environment
2. Decide strategy above by sprint planning before Phase 4
3. Track: https://github.com/PerplexDigital/Perplex.ContentBlocks

---

## BLOCKER-03: Konstrukt → Umbraco.UIBuilder

**Status:** Replacement available but requires code port  
**Current version:** `Konstrukt.Startup` / `Konstrukt.Web.UI` 1.6.7  
**Replacement:** `Umbraco.UIBuilder` 17.2.0  
**Projects affected:** `N3O.Umbraco.UIBuilder`, `N3O.Umbraco.UIBuilder.StaticAssets`

### What is broken
Konstrukt was rebranded as Umbraco UI Builder from v14 onwards. The package IDs have changed and the API surface has changed significantly (it is now a Bellissima/Lit-based backoffice integration, not Angular). Simply swapping the package reference is not sufficient.

### csproj changes needed (already updated)
- `Konstrukt.Startup` 1.6.7 → `Umbraco.UIBuilder` 17.2.0
- `Konstrukt.Web.UI` 1.6.7 → `Umbraco.UIBuilder` 17.2.0

### Code changes needed (not yet done)
Audit all files in `src/UIBuilder/N3O.Umbraco.UIBuilder/` for Konstrukt namespaces and API calls:
- `using Konstrukt.*` → `using Umbraco.UIBuilder.*`
- `IKonstruktConfigBuilder` → `IUiBuilderConfigurationBuilder` (or equivalent)
- Dashboard/section registration API differs
- Tree/collection/action registration API differs

### Action required
1. Read the Umbraco UI Builder v17 documentation: https://docs.umbraco.com/umbraco-ui-builder
2. Map each Konstrukt API call to its UIBuilder equivalent
3. Rewrite `N3O.Umbraco.UIBuilder` registration and configuration code
4. Test the backoffice UI sections that depend on this

---

## BLOCKER-04: Umbraco.Engage (formerly uMarketingSuite)

**Status:** Available (v17.2.2) but namespace/API changes required  
**Current version:** `Umbraco.Engage.Core` 13.8.0  
**Projects affected:** `N3O.Umbraco.Marketing`, `N3O.Umbraco.Marketing.StaticAssets`, `N3O.Umbraco.Cloud.Engage`

### What is broken
Namespaces changed significantly between v13 and v17. Current code imports:
- `Umbraco.Engage.Infrastructure.Personalization.Segments.*`
- `Umbraco.Engage.Infrastructure.Personalization.PersonalizationProfile.*`
- `Umbraco.Engage.Web.Cockpit.Segments.*`

These need to be mapped to the v17 equivalents.

### csproj changes needed (already updated)
- `Umbraco.Engage.Core` 13.8.0 → `Umbraco.Engage` 17.2.2
- `Umbraco.Engage.StaticAssets` / `Umbraco.Engage.Forms.StaticAssets` — verify if these are now bundled into `Umbraco.Engage`; remove if so

### Code changes needed (not yet done)
Files to update:
- `src/Marketing/N3O.Umbraco.Marketing/UmbracoEngageSegmentsDataSource.cs`
- `src/Cloud/N3O.Umbraco.Cloud.Platforms.Marketing/Services/Campaigns/TelethonOnAirSegmentRule.cs`
- `src/Cloud/N3O.Umbraco.Cloud.Platforms.Marketing/Services/Campaigns/TelethonOnAirSegmentRuleFactory.cs`
- `src/Cloud/N3O.Umbraco.Cloud.Platforms.Marketing/Services/Campaigns/TelethonOnAirCockpitSegmentRuleFactory.cs`
- `src/Cloud/N3O.Umbraco.Cloud.Platforms.Marketing/PlatformsMarketingComposer.cs`

### Action required
1. Follow the official Engage upgrade guide: https://docs.umbraco.com/umbraco-engage
2. Run Engage's database migrations in order (v13 → v14 → ... → v17)
3. Map old namespaces to new ones using the Engage v17 API reference
4. Test personalisation rules and segments in backoffice

---

## BLOCKER-05: Umbraco.Code 2.4.0

**Status:** Unconfirmed — no known v17 release  
**Projects affected:** `N3O.Umbraco.Extensions`

### What is broken
`Umbraco.Code` is used for generating strongly-typed model code from content types. v2.4.0 was built for Umbraco 13. No v17-compatible release has been identified.

### Action required
1. Verify what `Umbraco.Code` is used for in this project (check usages in `N3O.Umbraco.Extensions`)
2. If only used at design time for code generation: check if generated code is already committed to the repo; if so, remove the dependency
3. If used at runtime: identify the alternative (Umbraco's built-in ModelsBuilder, or a newer generator)
4. If no v17 release: remove the package reference and commit the generated code statically

---

## BLOCKER-06: Nested Content → Block List (Content Data Migration)

**Status:** Migration code written; database action required per site  
**File:** `src/N3O.Umbraco.Extensions/Migrations/NestedContentToBlockListMigration.cs`

### What needs to happen
For every existing client site database, Nested Content property values must be converted to Block List JSON format before the site can run on v17. The migration class handles this but must be:

1. **Registered** as part of an `IMigrationPlan` in the relevant package composer
2. **Run** on each site database during the upgrade process

### Example registration
```csharp
public class MyMigrationPlan : PackageMigrationPlan {
    public MyMigrationPlan() : base("N3O.Umbraco.NestedToBlockList") { }

    protected override void DefinePlan() {
        To<NestedContentToBlockListMigration>("2026-NestedToBlockList-v1");
    }
}
```

### Pre-migration checklist per site
- [ ] Back up the database
- [ ] Verify all Nested Content data types have a corresponding Block List data type configured with the same element types
- [ ] Run on a database copy first
- [ ] Verify rendered output before and after in a staging environment
- [ ] Update uSync XML files alongside (see BLOCKER-07)

---

## BLOCKER-07: uSync XML for Nested Content Data Types

**Status:** Must be done manually per site alongside CM-01  
**Packages affected:** `uSync.Complete` (now at 17.3.6)

### What is broken
uSync stores content type and data type definitions as XML files on disk. Any XML referencing `Umbraco.NestedContent` as editor alias or its config format must be updated to `Umbraco.BlockList` with Block List config after the database migration.

### Action required
After running `NestedContentToBlockListMigration` on a database:
1. Run `uSync export` to regenerate all XML files from the migrated database
2. Commit the regenerated XML to source control
3. Verify re-import on a clean database works

---

## BLOCKER-08: Umbraco Forms Subscription License

**Status:** Procurement action required  
**Current version:** 13.9.6 → upgrading to 17.0.1  
**Projects affected:** `N3O.Umbraco.Forms`, `N3O.Umbraco.Forms.StaticAssets`

From Umbraco v17, the perpetual license model for Umbraco Forms is no longer valid. A subscription license is required. This affects every client site using Forms.

### Action required
1. Contact Umbraco sales to obtain subscription licenses
2. Configure license keys per site before deploying v17
3. See: https://umbraco.com/products/umbraco-forms/

---

## BLOCKER-09: Our.Umbraco.GMaps — API changes in v17

**Status:** v17 package available (17.0.0) but API changes unverified  
**Current version:** `Our.Umbraco.GMaps.Core` 3.0.5  
**Replacement:** `Our.Umbraco.GMaps` 17.0.0  
**Projects affected:** `N3O.Umbraco.Maps.Google`, `N3O.Umbraco.Maps.Google.StaticAssets`

### Action required
1. Install `Our.Umbraco.GMaps` 17.0.0
2. Review breaking changes between v3 and v17
3. Update any namespace or API calls in `src/Maps/N3O.Umbraco.Maps.Google/`
4. Test map property editors in backoffice

---

## Summary Table

| # | Blocker | External Dependency | Action | Urgency |
|---|---------|-------------------|--------|---------|
| 01 | Contentment | No v17 release | Monitor upstream | Medium |
| 02 | Perplex.ContentBlocks | RC only (v4.0.0-rc.2) | Decide strategy | High |
| 03 | Konstrukt → UIBuilder | Available (v17.2.0) | Code port required | High |
| 04 | Umbraco.Engage | Available (v17.2.2) | Namespace updates required | High |
| 05 | Umbraco.Code | Unconfirmed | Investigate/remove | Medium |
| 06 | Nested Content DB migration | N/A (code written) | Register + run per site | Critical |
| 07 | uSync XML for NestedContent | N/A | Regenerate post-migration | Critical |
| 08 | Forms subscription license | Procurement | Contact Umbraco sales | High |
| 09 | GMaps v17 API changes | Available (v17.0.0) | API audit required | Low |
