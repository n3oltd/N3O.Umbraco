# N3O.Umbraco: Umbraco 13 → 17 Migration Plan

## Executive Summary

N3O.Umbraco is a **shared Umbraco package framework** (120 projects, not a single website) consumed by multiple client sites. The migration target is **Umbraco 17.3.5 on .NET 10**, up from Umbraco 13.14.0 on .NET 8. Work is in progress on branch `v17-Talha` (single commit `369ec7d`, 24 files changed).

**What is done:** The two foundation projects (`N3O.Umbraco.Cms`, `N3O.Umbraco.Extensions`) are on `net10.0` and reference Umbraco 17.3.5. The startup pipeline, content-type factory pattern, navigation-service root lookups, Block List property builder, URL provider interface, MNTP value converter, antiforgery header names, and the `SaveAndPublish` split have been migrated. A Contentment workaround and a new Bellissima-style DynamicListViews extension exist.

**What remains (the bulk of the work):**
- **117 of 120 projects still target `net8.0`** — the single largest outstanding item.
- **~10 compile-blocking C# API breakages** outside the two migrated projects (removed `UmbracoApiController`, `SurfaceController` constructor, `NestedContentConfiguration`, `BlockValue`/`BlockItemData`, `IPublishedCache.GetAtRoot`/`GetContentType`/`GetByContentType`, the `IPublishedCache` abstraction in `Locator`, `SectionRequirement` namespace).
- **9 third-party package blockers** (Contentment, Perplex, Konstrukt, Engage, Forms, uSync, Workflow, Azure Blob storage, GMaps; plus `Umbraco.Code` to verify).
- **Runtime breakages** (`ILocalizationService`, Nested Content stubs that throw, preview-URL stub, list-view UX regression).
- **Content data migration** (Nested Content JSON → Block List, uSync XML, Contentment, Engage).

This document is the execution reference for completing the migration. **Nothing in the current branch state compiles solution-wide today** — only the two foundation projects are internally consistent.

---

## Migration Progress

| Area | Status | Notes |
|---|---|---|
| Target framework (foundation projects) | Done | `N3O.Umbraco.Cms`, `N3O.Umbraco.Extensions` on `net10.0` |
| Target framework (all other projects) | Not Started | 117 projects still `net8.0` |
| Umbraco core packages | Done | `Umbraco.Cms*` 17.3.5; `Diplo.GodMode` 17.0.0 |
| Startup pipeline (`CmsStartup.cs`) | Done | `AddBackOffice/AddWebsite/AddDeliveryApi/AddComposers`; `UseInstallerEndpoints` removed |
| Content-type lookup (factory pattern) | Done | `ContentHelper`, `ContentJsonConverter`, `ContentTypesDataSource` migrated |
| Root content/media lookup (navigation svc) | Partial | `Locator.Content/Media` done; `PublishedContentParser` still uses `GetAtRoot()` |
| `IPublishedCache` abstraction in `Locator` base | Not Started | `Locator.cs` still uses removed common-type `IPublishedCache` |
| Block List property builder | Done | `PropertyBuilder.BlockList.cs` rewritten to typed v17 API |
| Block preview controller | Not Started | Multiple intersecting breaks (`BlockValue`, cache lookups, `ILocalizationService`, snapshot accessor) |
| `BlockItemData` / `BlockValue` extensions | Not Started | `PropertyValues`→`Values`; abstract `BlockValue` |
| Nested Content (editor + data + converters) | Partial | Extension methods stubbed to throw; converters/configs/builders NOT migrated; callers still call stubs |
| API controllers (`UmbracoApiController`) | Not Started | 2 base classes + 6 downstream controllers |
| `SurfaceController` constructor | Not Started | `AuthenticationController` |
| `ILocalizationService` → `ILanguageService` | Not Started | 4 files |
| Antiforgery | Partial | Header/cookie names fixed; `OurBackofficeAntiforgery` class should be removed |
| URL provider | Partial | Interface implemented; `GetPreviewUrlAsync` is a null stub |
| DynamicListViews backoffice | Partial | New API controller + App_Plugins JS added; tree `isContainer` UX not replicated |
| `SaveAndPublish` helper | Partial | Split correct; error handling is `/*TODO*/` placeholder |
| Third-party Umbraco packages | Not Started | 9 packages on v13/blocked |
| Content data migration | Not Started | Nested Content, uSync XML, Contentment, Engage |
| ModelsBuilder mode (`InMemoryAuto`) | Unconfirmed | Verify whether `Umbraco.Cms.DevelopmentMode.Backoffice` is needed |

---

## Complete Removed Features Reference

| Feature | Removed In | Replacement | Complexity | Status in Branch |
|---|---|---|---|---|
| Nested Content editor (`Umbraco.NestedContent`) | v14 | Block List / Block Grid | High | NOT addressed (10+ files); ext methods stubbed only |
| `NestedContentConfiguration` type | v14 | `BlockListConfiguration` | High | NOT addressed (4 files) |
| Grid Layout (`Umbraco.Grid`) | v14 | Block Grid | High | N/A — not used |
| Legacy Media Picker (`Umbraco.MediaPicker2`) | v14 | Media Picker v3 | Medium | Done — already `MediaPicker3` |
| `UmbracoApiController` | v15 (obsolete v14) | `ControllerBase` | Low | NOT addressed (2 bases + 6 downstream) |
| `UmbracoAuthorizedApiController` / `…JsonController` | v14 | `ManagementApiControllerBase` | Low–Med | N/A — custom wrapper used instead |
| `SurfaceController` ctor (4 infra params) | v14 | ctor takes `IUmbracoContextAccessor`, `IPublishedUrlProvider` | Low | NOT addressed (1 file) |
| `IPublishedSnapshot` | v15 (obsolete v14) | inject typed caches | Low | NOT addressed (via snapshot accessor) |
| `IPublishedSnapshotAccessor` | v15 (obsolete v14) | `IPublishedContentCache`/`IPublishedMediaCache` | Low–Med | Partial — 2 files + generated models remain |
| `IPublishedCache.GetContentType` / `GetByContentType` | v14/v15 | `IContentTypeService.Get` + `IPublishedContentTypeFactory` | Med | NOT addressed (BlockPreview controller) |
| `IPublishedCache.GetAtRoot()` | v15 | `IDocumentNavigationQueryService.TryGetRootKeys` + `GetById` | Low | Partial — `PublishedContentParser` remains |
| `IPublishedCache` as common base type | v14/v15 | typed caches | Med | NOT addressed (`Locator` hierarchy) |
| `ILocalizationService` | deprecated v14 | `ILanguageService` (async) | Med | NOT addressed (4 files) |
| `SectionRequirement` namespace | v14 | `Umbraco.Cms.Web.Common.Authorization` | Low | NOT addressed (1 file) |
| `BlockValue` as concrete/bindable type | v15 | `BlockListValue` / `BlockGridValue` | Med | NOT addressed (2 files) |
| `BlockItemData.PropertyValues` | v15 | `.Values` (+ verify `RawPropertyValues`) | Low | NOT addressed (2 files) |
| `IContentService.GetAll(Guid[])` | v15 | `GetMultiple(Guid[])` | Low | N/A — only no-arg `GetAll()` used |
| `IContentService.SaveAndPublish` | v14 | `Save` + `Publish` | Low | Done (error handling stubbed) |
| Macros / `IMacroService` / `IMacroRenderer` | v14 | Partial Views / RTE Blocks | High | N/A — not used |
| XPath traversal (`ContentAtXPath`) | v14 | `IContentLastChanceFinder` / typed services | Med | N/A — not used |
| Synchronous `PackageMigrationBase` | v17 | `AsyncPackageMigrationBase` | Low | N/A in repo; affects v15-compiled package deps |
| `InMemoryAuto` ModelsBuilder in core | v17 | `Umbraco.Cms.DevelopmentMode.Backoffice` pkg | Low | Unconfirmed — verify settings |
| Smidge bundling | v14 | ES Modules / Vite | Low | N/A — not used |
| `package.manifest` (server JSON) | v14 | `umbraco-package.json` (client) | — | At least one migrated (DynamicListViews) |
| XML backoffice lang files | v14 | JS module `"localization"` extension | Low | Unconfirmed |
| TinyMCE built-in RTE | v16 | TipTap (TinyMCE via 3rd-party pkg) | Low–Med | C# unaffected; Razor/template review needed |
| `UseInstallerEndpoints()` | v14 | removed from pipeline | Low | Done — not present |
| All system dates → UTC | v17 | auto DB migration; treat dates as UTC | Low | Unconfirmed — audit scheduled-publish/date logic |
| Block Editor data format (Block Level Variations) | v15 | new `BlockListValue`/`BlockGridValue` JSON | Med | Programmatic write path partly done (BlockList builder) |
| Swashbuckle v10 (breaking) | v17 | v10 API | Low–Med | Verify `NSwag`/OpenAPI usage |
| NPoco v6 (breaking) | v16 | recompile / update direct usage | Low–Med | Verify no direct NPoco usage |
| Umbraco Forms perpetual license | v17 | subscription license required | — | Procurement action |

---

## Completed Work (v17-Talha branch)

Already implemented and verified in the branch:

1. **Target framework** upgraded to `net10.0` in `src/N3O.Umbraco.Cms/N3O.Umbraco.Cms.csproj` and `src/N3O.Umbraco.Extensions/N3O.Umbraco.Extensions.csproj`; Umbraco refs at 17.3.5.
2. **Contentment workaround** — full package replaced with `Contentment.Core` only; `ExcludeLegacyUmbracoBackOffice` MSBuild target strips the transitive `Umbraco.Cms.Web.BackOffice` 13.x assembly (both csproj files, with TODO blocker comments).
3. **BackOffice package** removed as standalone reference in `N3O.Umbraco.Extensions.csproj` (folded into `Umbraco.Cms` in v14+).
4. **`UseInstallerEndpoints()` removed** from `src/N3O.Umbraco.Cms/CmsStartup.cs`; startup pipeline matches the v17 pattern.
5. **`.AddContentment()` commented out** in `CmsStartup.cs` with TODO.
6. **Antiforgery names fixed** — `AntiforgeryComposer.cs` `HeaderName` → `"X-UMB-XSRF-TOKEN"`; `OurBackofficeAntiforgery.cs` cookie → `WebConstants.Web.CsrfValidationCookieName`.
7. **Content-type factory pattern** replacing `IPublishedCache.GetContentType` in `ContentHelper.cs`, `ContentJsonConverter.cs`, `ContentTypesDataSource.cs` (via `IContentTypeService.Get` + `IPublishedContentTypeFactory.CreateContentType`).
8. **Root-key navigation** — `Locator.cs` adds abstract `GetRootKeys()`; `Locator.Content.cs` uses `IDocumentNavigationQueryService`, `Locator.Media.cs` uses `IMediaNavigationQueryService`; bug fix to include the root node itself before `Descendants()`.
9. **Block List builder rewrite** — `PropertyBuilder.BlockList.cs` now uses `BlockListLayoutItem`, `BlockItemData`, `BlockPropertyValue`, `BlockListValue`, and `Dictionary<string, IEnumerable<IBlockLayoutItem>>`.
10. **Nested Content extension stubs** — 6 overloads in `ContentHelperExtensions.Nested.cs` throw `NotSupportedException`; `PublishedPropertyTypeExtensions.cs` had `GetNestedContentType` removed.
11. **DynamicListViews rewrite** — `ContentSending.cs` and `NodesRendering.cs` deleted; new `DynamicListViewApiController.cs`; `NotificationHandlerSkipper.cs` updated; App_Plugins JS (`dynamic-list-view.js`, `dynamic-list-view-condition.js`, `umbraco-package.json`) added with a copy target.
12. **URL provider** — `UrlProvider.cs` uses `NewDefaultUrlProvider`, `UrlInfo.AsUrl`, `UrlInfo.Url?.ToString()`, adds abstract `Alias` and virtual `GetPreviewUrlAsync` (null stub).
13. **Staging middleware** — `StagingMiddleware.cs` uses `IContentLocator` lazy + `using` on `EnsureUmbracoContext()`.
14. **MNTP value converter** — `StronglyTypedMultiNodeTreePickerValueConverter.cs` ctor takes `IPublishedContentCache`, `IPublishedMediaCache`, `IPublishedMemberCache` (snapshot accessor removed).
15. **Build targets** — `N3O.Umbraco.Cms.targets` copies `App_Plugins/**` to `wwwroot/App_Plugins/`.
16. **Added** `Microsoft.AspNetCore.Mvc.NewtonsoftJson` 10.0.7 to `N3O.Umbraco.Extensions.csproj`.
17. **`SaveAndPublish` helper** — `ContentServiceExtensions.cs` (lines 70–86) splits `Save` + `Publish(content, ["*"])` (error handling still `/*TODO*/`).
18. **appsettings schema** — `appsettings-schema.Umbraco.Cms.json` updated to the v17 schema.

---

## Remaining Compile-Time Issues

> Effort key: Small = <½ day; Medium = ½–2 days; Large = multi-day / cross-cutting.

**CE-01 — `UmbracoApiController` removed (Blocking, all controllers)**
- `src/N3O.Umbraco.Extensions/Hosting/Controllers/BackofficeAuthorizedApiController.cs:22` and `…/ApiController.cs:19` inherit `UmbracoApiController` (`Umbraco.Cms.Web.Common.Controllers`), gone in v15.
- Fix: change both bases to `: ControllerBase`; remove the now-unused `using Umbraco.Cms.Web.Common.Controllers;`. Fixes all 6 downstream controllers transitively (`BlockPreviewBackofficeController`, `TypesenseDevToolsController`, `DynamicListViewApiController`, `PlatformsDevToolsController`, `PlatformsBackOfficeController`, `CloudBackOfficeController`).
- Effort: Small.

**CE-02 — `SurfaceController` constructor signature changed (Blocking)**
- `src/Authentication/N3O.Umbraco.Authentication/Controllers/AuthenticationController.cs:17–34` injects/forwards `IUmbracoDatabaseFactory`, `ServiceContext`, `AppCaches`, `IProfilingLogger`.
- Fix: remove those 4 params from the ctor and the `base(...)` call (v17 base takes only `IUmbracoContextAccessor`, `IPublishedUrlProvider`).
- Effort: Small.

**CE-03 — `NestedContentConfiguration` type removed (Blocking)**
- `src/Blocks/N3O.Umbraco.Blocks.Perplex/Services/PerplexBlockTypesService.cs:141–157`; `src/Data/N3O.Umbraco.Data/Converters/Properties/PropertyConverter.NestedContent.cs:40,81,117,163`; `src/Data/N3O.Umbraco.Data/Extensions/ContentTypeExtensions.cs:40`; `src/Data/N3O.Umbraco.Data/Models/Content/Nested/NestedSchemaResMapping.cs:27`. Also alias constant `UmbracoPropertyEditors.Aliases.NestedContent` at `PropertyConverter.NestedContent.cs:31` and `src/Data/N3O.Umbraco.Data/Lookups/Content/PropertyType.Nested.cs:14`.
- Fix: rewrite all sites against `BlockListConfiguration` / `BlockListConfiguration.BlockConfiguration[]`; replace the alias with `Constants.PropertyEditors.Aliases.BlockList`. Entangled with CE-12 (Perplex).
- Effort: Large.

**CE-04 — `BlockValue` abstract used as parameter (Blocking)**
- `src/Blocks/N3O.Umbraco.Blocks/Controllers/BlockPreviewBackofficeController.cs:61` `[FromBody] BlockValue blockData`; `src/Blocks/N3O.Umbraco.Blocks/Extensions/BlockValueExtensions.cs:18` extension on `BlockValue`.
- Fix: use the concrete type (`BlockGridValue` or `BlockListValue` per the editor used for preview) for both the bound parameter and the extension; verify all `DeserializeAndClean` callers.
- Effort: Small.

**CE-05 — `BlockItemData.PropertyValues` renamed to `Values` (Blocking)**
- `src/Blocks/N3O.Umbraco.Blocks/Extensions/BlockItemDataExtensions.cs:18`; `src/Blocks/N3O.Umbraco.Blocks/Extensions/BlockValueExtensions.cs:93`. Verify `RawPropertyValues` (`BlockValueExtensions.cs:84,86`; `BlockItemDataExtensions.cs:46`) against v17 source before renaming.
- Fix: rename `PropertyValues` → `Values`; confirm/adjust `RawPropertyValues`.
- Effort: Small.

**CE-06 — `IPublishedCache.GetContentType` / `GetByContentType` removed (Blocking)**
- `src/Blocks/N3O.Umbraco.Blocks/Controllers/BlockPreviewBackofficeController.cs:122,124,126`.
- Fix: inject `IPublishedContentCache` directly (drop `IPublishedSnapshotAccessor`); use `IContentTypeService.Get` + `IPublishedContentTypeFactory` for the type, and navigation service + `GetById` to enumerate by type.
- Effort: Medium.

**CE-07 — `IPublishedCache.GetAtRoot()` removed (Blocking)**
- `src/Data/N3O.Umbraco.Data/Parsing/PublishedContent/PublishedContentParser.cs:18,44`.
- Fix: inject `IPublishedContentCache` directly; replace `GetAtRoot()` with `IDocumentNavigationQueryService.TryGetRootKeys` + `GetById` (mirror `Locator.Content.cs`).
- Effort: Small.

**CE-08 — `IPublishedCache` common abstraction in `Locator` (Blocking)**
- `src/N3O.Umbraco.Extensions/Content/Locator.cs:92,101` (`Func<IPublishedCache,T>`, abstract `GetCache` returning `IPublishedCache`); overrides at `Locator.Content.cs:19` and `Locator.Media.cs:20`.
- Fix: remove the common-type abstraction; split `Run<T>` into typed paths (`IPublishedContentCache` / `IPublishedMediaCache`).
- Effort: Medium.

**CE-09 — `SectionRequirement` namespace moved (Blocking)**
- `src/Scheduler/N3O.Umbraco.Scheduler/SchedulerComposer.cs:24,83`.
- Fix: `using Umbraco.Cms.Web.Common.Authorization;`.
- Effort: Small.

**CE-10 — `Umbraco.Code` 2.4.0 v17 compatibility unconfirmed (Potentially blocking)**
- `src/N3O.Umbraco.Extensions/N3O.Umbraco.Extensions.csproj`.
- Fix: confirm a v17-compatible release exists; if not, commit generated code and drop the reference, or adopt an alternative generator.
- Effort: Medium.

**CE-11 — 117 projects still target `net8.0` (Blocking)**
- All `src/**/*.csproj` except `N3O.Umbraco.Extensions` and `N3O.Umbraco.Cms`.
- Fix: set `<TargetFramework>net10.0</TargetFramework>` everywhere, then resolve per-project fallout. The mechanical edit is Small; the resulting compile fallout across 117 projects is Large.
- Effort: Large.

**CE-12 — `Perplex.ContentBlocks` 3.0.1 no confirmed v17 release (Blocking)**
- `src/Blocks/N3O.Umbraco.Blocks.Perplex` (+ `src/Data/N3O.Umbraco.Data.PerplexBlocks`).
- Fix: check for a v17 Perplex release; if none, replace the module with native Block Grid or exclude it from the build.
- Effort: Large.

**CE-13 — Konstrukt 1.6.7 renamed/replaced in v14+ (Blocking)**
- `src/UIBuilder/N3O.Umbraco.UIBuilder` and `…StaticAssets` use `Konstrukt.Startup` / `Konstrukt.Web.UI`.
- Fix: migrate to `Umbraco.UIBuilder` (verify v17 package ID); update all Konstrukt API calls to the UI Builder equivalents.
- Effort: Large.

**CE-14 — `Our.Umbraco.Community.Contentment.Core` 4.7.0 no v17 release (Blocking, acknowledged)**
- `src/N3O.Umbraco.Cms`, `src/N3O.Umbraco.Extensions`.
- Fix: monitor upstream for a v17 release; when available, remove the `ExcludeLegacyUmbracoBackOffice` workaround, restore `.AddContentment()`, and test all content using Contentment editors. Until then the MSBuild hack allows building but Contentment-backed editors will not function in v17.
- Effort: Large (blocked on upstream).

**CE-15 — `Microsoft.AspNetCore.*` / `Microsoft.Extensions.*` pinned to 8.0.x (Potentially blocking after CE-11)**
- `N3O.Umbraco.Authentication.Auth0` (OpenIdConnect 8.0.17); `N3O.Umbraco.Blazor` (Components.CustomElements 8.0.17); `N3O.Umbraco.Cloud` (Http.Polly 8.0.17); `N3O.Umbraco.Clients` (DI.Abstractions 8.0.2, Http 8.0.1).
- Fix: after each project moves to `net10.0`, remove framework-provided package pins or bump to `10.0.x`.
- Effort: Small.

---

## Remaining Runtime Issues

**RE-01 — `ILocalizationService` deprecated; async `ILanguageService` required (Runtime break, Medium)**
- `src/Blocks/N3O.Umbraco.Blocks/Controllers/BlockPreviewBackofficeController.cs:29,39,133`; `src/N3O.Umbraco.Extensions/Extensions/LocalizationServiceExtensions.cs:9,17,19`; `src/N3O.Umbraco.Extensions/Localization/LocalizationSettingsAccessor.Environment.cs:11,14`; `src/Cloud/N3O.Umbraco.Cloud/Services/PublishedLocalizationSettingsAccessor.cs:15,20`.
- Fix: replace with `ILanguageService`; `GetDefaultLanguageIsoCode()` → `(await GetDefaultLanguageAsync())?.IsoCode`; propagate async through callers.

**RE-02 — `GetNestedContents` callers throw `NotSupportedException` (Runtime break, Medium per caller)**
- `src/Cloud/N3O.Umbraco.Cloud.Platforms/Validators/DonationFormState/DonationFormStateValidator.Fund.cs:47`; `src/Giving/N3O.Umbraco.Giving.Allocations/Content/Settings/FundDonationOptionValidator.cs:47`.
- Fix: migrate both to Block List equivalents (`contentHelper.GetBlockList<T>(...)`).

**RE-03 — `ContentHelper.cs` still has Nested Content branch/methods (Silent wrong output, Small)**
- `src/N3O.Umbraco.Extensions/Content/ContentHelper.cs:86` branch; `194–251` `GetContentPropertiesForNestedContent(...)`.
- Fix: remove the `IsNestedContent()` branch and the helper methods; then remove the now-unused `PropertyTypeExtensions.IsNestedContent()` (`PropertyTypeExtensions.cs:39–41`).

**RE-04 — `PropertyBuilder.Nested.cs` writes obsolete JSON (Silent garbage data, Small + callers)**
- `src/N3O.Umbraco.Extensions/Content/Editor/PropertyBuilder.Nested.cs`.
- Fix: delete the file; migrate all callers to the migrated `PropertyBuilder.BlockList.cs`. Also remove the `.Nested()` builder hook in `ContentBuilderExtensions.cs:42–44`.

**RE-05 — DynamicListViews tree `isContainer` styling not replicated (UX regression, Medium)**
- New `src/N3O.Umbraco.Extensions/Features/DynamicListViews/DynamicListViewApiController.cs` returns only `{enabled}`; deleted `NodesRendering.cs` set `isContainer=true`.
- Fix: confirm the App_Plugins JS implements the tree/list-view presentation via the v17 extension system; add a workspace/tree condition extension if it does not.

**RE-06 — `UrlProvider.GetPreviewUrlAsync` returns null for all previews (Backoffice preview broken, Medium)**
- `src/N3O.Umbraco.Extensions/UrlProviders/UrlProvider.cs:31`.
- Fix: implement per subclass, or provide a base implementation deriving the preview URL from `GetUrl`.

**RE-07 — `SaveAndPublish` bare-exception error handling (Poor diagnostics, Small)**
- `src/N3O.Umbraco.Extensions/Extensions/ContentServiceExtensions.cs:73,80` — `throw new Exception(...)` discards `PublishResult`/`OperationResult` detail.
- Fix: surface `EventMessages`/`StatusType` in the thrown exception or log+return a typed failure.

**RE-08 — `GetNestedPropertySchemaHandler` is a `NotImplementedException` stub (Runtime break on schema export, Medium)**
- `src/Data/N3O.Umbraco.Data/Handlers/Content/GetNestedPropertySchemaHandler.cs:13`.
- Fix: implement for Block List schema export, or remove the handler + query/response types if no longer needed.

**RE-09 — System dates now UTC (Potential logic drift, audit)**
- Audit any scheduled-publish / create/update date comparisons (notably `src/Scheduler/` Hangfire logic) for local-time assumptions; the v17 auto-migration converts stored dates to UTC.

---

## Functional Gaps (NotSupportedException / NotImplementedException stubs)

**Migration-introduced gaps (must resolve for production):**

- **FG-01** — `src/N3O.Umbraco.Extensions/Extensions/ContentHelperExtensions.Nested.cs:11,17,24,29,35,42`: 6 `GetNestedContent`/`GetNestedContents` overloads throw. Prefer **removing** them (turns runtime failures into compile errors) once all in-repo callers (RE-02) and downstream consumer sites are migrated. Run a solution-wide search for remaining callers, including external client sites.
- **FG-02** — `src/Blocks/N3O.Umbraco.Blocks.Perplex/Services/PerplexBlockTypesService.cs:109–157` `CreateDataTypes`: builds Nested Content data types; rewrite to `BlockListConfiguration` (entangled with CE-12).
- **FG-03** — `src/N3O.Umbraco.Extensions/Extensions/PublishedPropertyTypeExtensions.cs`: `GetNestedContentType` removed entirely. Confirm no callers remain (in-repo and downstream); provide a Block List equivalent if any do.
- **FG-05** — `src/N3O.Umbraco.Extensions/Antiforgery/OurBackofficeAntiforgery.cs`: Angular-era CSRF workaround (issue #16107). Delete the class and its `AntiforgeryComposer` registration; let v17 handle antiforgery natively. Verify custom backoffice API CSRF still works.

**Pre-existing stubs (not migration regressions, but block their feature paths):**

| File | Line | Note |
|---|---|---|
| `src/Webhooks/N3O.Umbraco.Webhooks/Transforms/WebhookTransform.cs` | 62 | Unimplemented transform |
| `src/Validation/N3O.Umbraco.Validation/Services/ProfanityGuard/ProfanityGuard.cs` | 9 | Entire service stub |
| `src/N3O.Umbraco.Extensions/Json/Converters/SerializeToUrlJsonConverter.cs` | 27 | `WriteJson` stub |
| `src/N3O.Umbraco.Extensions/Json/Converters/PublishedElementJsonConverter.cs` | 21 | `WriteJson` stub |
| `src/N3O.Umbraco.Extensions/Json/Converters/PublishedContentJsonConverter.cs` | 32 | `WriteJson` stub |
| `src/N3O.Umbraco.Extensions/Json/Converters/ContentJsonConverter.cs` | 48 | `WriteJson` stub |
| `src/Plugins/Cropper/N3O.Umbraco.Cropper/Services/CroppedImageJsonConverter.cs` | 19 | `WriteJson` stub |
| `src/Plugins/EditorJs/N3O.Umbraco.EditorJs/DataTypes/EditorJsBlockJsonConverter.cs` | 44 | `WriteJson` stub |
| `src/N3O.Umbraco.Extensions/Hosting/Controllers/PageController.cs` | 46 | `Index()` override throws |
| `src/Data/N3O.Umbraco.Data/Parsing/Money/MoneyParser.cs` | 22 | Unimplemented parse path |
| `src/Data/N3O.Umbraco.Data/Parsing/DataTypeParser.cs` | 50,54 | Two unimplemented parse paths |
| `src/Payments/N3O.Umbraco.Payments/Services/PaymentsScopeBase.cs` | 33 | Unimplemented scope method |
| `src/Payments/N3O.Umbraco.Payments/Json/PaymentObjectJsonConverter.cs` | 50 | `WriteJson` stub |

These should be triaged after the migration compiles; many `WriteJson` stubs are intentional (read-only converters) — verify before changing.

---

## Package Compatibility Issues

| Package | Current | Projects | Issue | Resolution | Status |
|---|---|---|---|---|---|
| Our.Umbraco.Community.Contentment(.Core) | 4.7.0 | Cms, Extensions | No v17 release; pulls BackOffice 13.x | MSBuild strip workaround in place; await v17 release, then restore `.AddContentment()` | **Blocked (upstream)** — builds via workaround |
| Perplex.ContentBlocks | 3.0.1 | Blocks.Perplex, Data.PerplexBlocks | No confirmed v17 release; uses removed Nested Content APIs | Upgrade if available, else replace with native Block Grid or exclude module | **Blocked** |
| Konstrukt.Startup / Konstrukt.Web.UI | 1.6.7 | UIBuilder, UIBuilder.StaticAssets | Renamed to `Umbraco.UIBuilder` in v14+ | Replace package + migrate API to UI Builder | **Available (renamed) — needs port** |
| Umbraco.Engage.Core / .StaticAssets / .Forms.StaticAssets | 13.8.0 / 13.2.0 | Marketing, Marketing.StaticAssets, Cloud.Engage | v13; namespaces change (`Umbraco.Engage.Infrastructure.Personalization.*`, `…Web.Cockpit.*`) | Upgrade to Engage v17; update namespaces in `UmbracoEngageSegmentsDataSource.cs`, `TelethonOnAir*` types, `PlatformsMarketingComposer.cs` | **Available — needs port** |
| Umbraco.Forms(.Core/.StaticAssets) | 13.9.6 | Forms, Forms.StaticAssets, Sync.Forms | v13; also subscription-license change in v17 | Upgrade to Forms v17; procure subscription license | **Available — needs upgrade + license** |
| uSync.Complete / uSync.Forms | 13.2.4 / 13.4.6 | Sync, Sync.Forms | v13 | Upgrade to uSync v17 | **Available** |
| Umbraco.Workflow(.Core) | 13.5.4 | Workflows, Workflows.StaticAssets | v13 | Upgrade to Workflow v17 | **Available** |
| Umbraco.StorageProviders.AzureBlob(.ImageSharp) | 13.1.0 | Storage.Azure | v13 | Upgrade to v17; check setup API | **Available** |
| Our.Umbraco.GMaps(.Core) | 3.0.5 | Maps.Google(.StaticAssets) | Community pkg; v17 unconfirmed | Check for v17 release; else direct Google Maps JS integration | **Unconfirmed** |
| Umbraco.Code | 2.4.0 | Extensions | v17 compat unconfirmed | Verify v17 release or drop generator | **Unconfirmed** |
| Microsoft.AspNetCore.*/Extensions.* | 8.0.x | Auth0, Blazor, Cloud, Clients | Framework-provided on net10 | Remove pins or bump to 10.0.x after TFM bump | **Action after CE-11** |

Core Umbraco packages (`Umbraco.Cms*` 17.3.5) and `Diplo.GodMode` 17.0.0 are correct. No project directly references `Umbraco.Cms.Api.Management`. Verify direct **NPoco** and **Swashbuckle/NSwag** usage against v6 / v10 respectively.

---

## Content Data Migration

These affect databases of existing client sites; package-framework code changes alone do not migrate stored data.

**CM-01 — Nested Content property data → Block List (Large, Blocking for live data)**
- v13 stored `[{"ncContentTypeAlias":"…","key":"…","name":"…"}]`; v17 value converters expect Block List JSON (`{"layout":{"Umbraco.BlockList":[…]},"contentData":[…],"settingsData":[]}`).
- Write an `IMigrationPlan` / `AsyncPackageMigrationBase` migration: read `umbracoPropertyData` rows for properties whose editor was `Umbraco.NestedContent`, transform JSON to Block List, switch the data type's editor alias to `Umbraco.BlockList` with Block List config, and rewrite the rows. (Note: v17 requires **async** package migrations.)

**CM-02 — uSync XML referencing Nested Content (Medium, Blocking import)**
- uSync data-type/content-type XML on disk still references `Umbraco.NestedContent` and its `<Config>` format.
- Update affected uSync files to `Umbraco.BlockList` with Block List configuration; perform alongside CM-01.

**CM-03 — Contentment data types and values (Medium audit + Large if unresolved)**
- Cannot complete until the Contentment v17 blocker (CE-14/PB) is resolved. Audit and document every content type/property using Contentment editors now so migration can run immediately when the package lands.

**CM-04 — Umbraco Engage analytics/personalisation data (Medium, possible startup failure)**
- Follow the Engage v14+ upgrade guide and run its DB migrations in order when upgrading from v13.

**CM-05 — Block Editor v15 format + v17 UTC dates (auto, verify)**
- v15 auto-migrates Block Editor data for Block Level Variations; v17 auto-migrates stored dates to UTC. Both run on first startup and can be slow on large DBs — schedule a maintenance window and back up first.

**CM-06 — RTE: TinyMCE → TipTap (Low–Med, content review)**
- v16 auto-migrates TinyMCE data types to TipTap; HTML output differs. Review RTE-dependent Razor/email/feed output.

---

## Recommended Work Order

Dependency-ordered. Phases 1–4 are required to reach a building, runnable framework; 5–7 complete functional parity, content migration, and hardening.

**Phase 1 — Make the foundation projects' code correct (Small–Medium each)**
1. CE-08 `Locator` `IPublishedCache` abstraction split. (Medium)
2. CE-07 `PublishedContentParser.GetAtRoot()` → navigation service. (Small)
3. RE-03 remove `ContentHelper` Nested branch/methods + `IsNestedContent()`. (Small)
4. RE-04 delete `PropertyBuilder.Nested.cs` + remove `.Nested()` hook. (Small)
5. FG-05 delete `OurBackofficeAntiforgery` + registration. (Small)
6. RE-07 real error handling in `SaveAndPublish`. (Small)
7. RE-06 implement `GetPreviewUrlAsync` base/overrides. (Medium)

**Phase 2 — Cross-cutting compile fixes (Small each)**
8. CE-01 controller base classes → `ControllerBase`. (Small)
9. CE-09 `SectionRequirement` namespace. (Small)
10. CE-10 verify/resolve `Umbraco.Code`. (Medium)

**Phase 3 — Bump all projects to net10.0, then resolve fallout (Large)**
11. CE-11 set `net10.0` across the remaining 117 csproj. (mechanical Small; fallout Large)
12. CE-15 fix `Microsoft.AspNetCore.*`/`Extensions.*` pins. (Small)
13. CE-02 `SurfaceController` ctor. (Small)
14. CE-04 `BlockValue` → concrete type. (Small)
15. CE-05 `BlockItemData.PropertyValues` → `Values`. (Small)
16. CE-06 `BlockPreviewBackofficeController` cache lookups. (Medium)
17. RE-01 `ILocalizationService` → `ILanguageService` (4 files). (Medium)

**Phase 4 — Package blockers (parallelisable; some block compile)**
18. PB-01 Forms v17 (+ license). PB-04 uSync v17. PB-05 Workflow v17. PB-03 Azure Blob v17. (Small–Medium each)
19. PB-02 Engage v17 + namespace ports. (Large)
20. CE-13/PB-09 Konstrukt → Umbraco UI Builder. (Large)
21. PB-06 GMaps v17 or replacement. (Medium)
22. CE-12/PB-07 Perplex resolution → enables CE-03 + FG-02. (Large)
23. CE-03 NestedContentConfiguration → BlockListConfiguration in `N3O.Umbraco.Data` + Perplex. (Large)
24. CE-14/PB-08 Contentment — track upstream; restore `.AddContentment()` when available. (Large, blocked)

**Phase 5 — Functional parity & runtime correctness**
25. RE-02 migrate `GetNestedContents` callers to Block List. (Medium)
26. FG-01 remove the 6 Nested Content stubs once all callers migrated. (Small)
27. FG-03 confirm `GetNestedContentType` has no callers. (Small)
28. RE-05 DynamicListViews tree/list-view UX in App_Plugins JS. (Medium)
29. RE-08 implement or remove `GetNestedPropertySchemaHandler`. (Medium)
30. API-13 confirm ModelsBuilder mode; add `Umbraco.Cms.DevelopmentMode.Backoffice` if `InMemoryAuto`. (Small)
31. RE-09 audit date logic for UTC. (Small–Medium)

**Phase 6 — Content data migration (per consuming site)**
32. CM-01 + CM-02 Nested Content data + uSync XML. (Large)
33. CM-03 Contentment data (after CE-14). CM-04 Engage data. CM-06 RTE review.

**Phase 7 — Triage pre-existing stubs (FG-04) and the broader TODO backlog; full regression testing (TG-01…TG-10).**

---

## Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Contentment never ships a v17 release | Medium | High | Audit all Contentment usages now; plan native replacements (Data List, etc.); keep MSBuild workaround building meanwhile; isolate behind feature flags |
| Perplex.ContentBlocks unsupported on v17 | High | High | Decide early: replace with native Block Grid; CE-03/FG-02 depend on this. Budget a Large rewrite |
| Konstrukt → UI Builder API surface drift | Medium | High | Spike the UI Builder port early; UI Builder is a commercial Bellissima rewrite, not a drop-in |
| net10.0 bump surfaces large hidden compile fallout across 117 projects | High | High | Migrate in dependency order, build frequently; treat #11 as multi-day; fix leaf projects after Extensions/Cms are green |
| Nested Content content-migration data loss | Medium | High | Back up DBs; write idempotent, well-tested migration; dry-run on a copy; verify rendered output before/after |
| Engage namespace/API changes larger than expected | Medium | Medium | Follow official Engage v14→v17 upgrade docs; isolate Marketing modules; regression-test segments/personalisation |
| Forms subscription-license procurement delay | Medium | Medium | Start procurement now; perpetual licenses are invalid on v17 |
| Auto DB migrations (Block format v15, UTC dates v17) slow/unresponsive on large sites | Medium | Medium | Maintenance window; back up; pre-clean old content versions; test on a DB copy first |
| Backoffice extensions (DynamicListViews, antiforgery) behave differently under Bellissima | Medium | Medium | Manual backoffice testing per TG items; confirm `umbraco-package.json`/JS register correctly |
| Removed Nested Content stubs break downstream client sites at compile time | High | Medium | Communicate the breaking change; provide Block List migration guidance to consumers; coordinate versioning |
| `Umbraco.Code` / NPoco / Swashbuckle direct-usage breakage | Low–Med | Medium | Verify each against v17 before relying on it |

---

## Definition of Done

Migration is complete when all of the following hold:

- [ ] All 120 projects target `net10.0`; no `net8.0` remains; no `Microsoft.AspNetCore.*`/`Extensions.*` packages pinned to 8.x.
- [ ] Solution builds with **0 errors and 0 warnings** against Umbraco 17.3.5 / .NET 10.
- [ ] No references to removed APIs: `UmbracoApiController`, `IPublishedSnapshot(Accessor)`, `IPublishedCache.GetAtRoot/GetContentType/GetByContentType`, common-type `IPublishedCache`, `NestedContentConfiguration`, `Umbraco.NestedContent` alias, `ILocalizationService`, old `SectionRequirement` namespace, abstract `BlockValue` as a bound/parameter type, `BlockItemData.PropertyValues`.
- [ ] `SurfaceController` constructors use only the v17 signature.
- [ ] No migration-introduced `NotSupportedException` Nested Content stubs remain reachable; all in-repo callers migrated to Block List; `PropertyBuilder.Nested.cs` deleted.
- [ ] `OurBackofficeAntiforgery` removed; backoffice + custom API CSRF verified working under Bellissima.
- [ ] `SaveAndPublish` has production-grade error handling (no `/*TODO*/`, no bare `Exception`).
- [ ] `GetPreviewUrlAsync` implemented for all URL provider subclasses that need preview.
- [ ] DynamicListViews delivers list-view UX parity in the v17 backoffice (verified manually).
- [ ] All third-party packages on v17-compatible versions OR an agreed, documented replacement/exclusion: Forms, uSync, Workflow, Azure Blob storage, Engage, GMaps, UI Builder, Perplex, Contentment, `Umbraco.Code`. Forms subscription license procured.
- [ ] ModelsBuilder mode confirmed; `Umbraco.Cms.DevelopmentMode.Backoffice` added if `InMemoryAuto` is used.
- [ ] Content migrations written, tested on a DB copy, and run: Nested Content → Block List, matching uSync XML updated; Engage and Contentment data handled.
- [ ] Auto-migrations (Block format, UTC dates, TinyMCE→TipTap) validated against a representative site copy with output review.
- [ ] Date-handling code audited for UTC correctness (notably Scheduler/Hangfire).
- [ ] Regression testing passed: block preview, custom URL providers, Block List builder output, staging middleware lifecycle, antiforgery, content/media root navigation (incl. cold-start/Examine warmup), multi-lingual `SaveAndPublish`, Auth0 login/logout, and Forms/uSync/Workflow/Storage module smoke tests.
- [ ] Pre-existing `NotImplementedException` stubs (FG-04) triaged: each either confirmed intentional or implemented.
- [ ] At least one downstream client site builds and runs against the migrated framework end-to-end.

---

*Note on scope:* Effort estimates are relative sizing, not calendar commitments. The two largest unknowns — Perplex/Contentment replacement strategy and per-site content-data migration volume — should be spiked first, as they gate the Block List work (CE-03/FG-02) and the production cutover respectively.
