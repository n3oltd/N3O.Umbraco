# Session Handoff — N3O.Umbraco v17 Migration

*Updated: 2026-06-02 (session 5) — use this to orient the next session*

---

## Current State

**Solution builds with 0 errors (re-verified this session). App starts and serves HTTP 200.** Umbraco 17.3.5 on .NET 10, all 120 projects.

### Session 5 — uSync Publisher, branch deliberation review, blocker fixes, NestedContent→BlockList

**1. RR-01 / BLOCKER-05 — uSync Publisher `SyncContentHandler` reimplemented** (research workflow: DLL reflection + web + usage). `IPublisherStateService` is gone; pipeline rebuilt on `Jumoo.Processing` (real assembly `jumoo.processing.core.dll`; the TODO-named `Jumoo.Processing.dll` is an empty placeholder). Handler now injects `uSync.Publisher.Strategies.Processor.PublisherProcessor` → `Process(PublisherActionRequest, PublisherProcessingOptions)` → `SyncPublishResponse`; same Document-UDI push w/ published deps, throws on `!Success`. Registered `AddTransient<PublisherProcessor>` in `SyncExtensionsComposer`. Build 0 errors, boots clean. ⚠️ Needs E2E test vs a real remote uSync server (single-call pipeline completion unverified; `RequestId` left unset; `SyncItem.Name` empty).

**2. Branch deliberation review** (14 agents: 13 Sonnet area/concern reviewers → Opus synthesis of the whole `v17-Talha` diff). Verdict **MAKE-SENSE-WITH-FIXES**. Full tracker: **`REVIEW_FINDINGS.md`** (repo root).

**3. Blocker fixes applied + verified (build 0 errors, boot clean):**
| Fix | Detail |
|---|---|
| CRITICAL duplicate converter | `PropertyConverter.NestedContent.cs` had identical `IsConverter()`→`Umbraco.BlockList` as the BlockList one → crash on every Data import/export. **Deleted** (folded its better `GetMaxValues` into `PropertyConverter.BlockList.cs`). |
| NC migration empty-path JSON | `@Umbraco_BlockList` (underscore) → dotted `"Umbraco.BlockList"` `JObject`. |
| NC migration transaction | wrapped Steps 3+4 in `using var transaction = db.GetTransaction()` + `Complete()`. |
| `UrlInfo.AsUrl` arg order | both `TryGetRelocatedUrl` overloads → `AsUrl(url, Alias, culture, false)`. |

**4. NestedContent → BlockList replacement** (per Talha — everywhere applicable): `PropertyType.Nested.cs` re-keyed to `Aliases.BlockList` + `.BlockList()` builder; `GetNestedContent(s)` ×6, `ContentBuilderExtensions.Nested()`, `NestedPropertyBuilder` → `[Obsolete(error:true)]` redirecting to BlockList; **live caller `DonationItemReceiver.cs` `.Nested(PricingRules)`→`.BlockList(...)` (was throwing at runtime)**. Excluded: generated `Cloud.Platforms/Clients/*` (external API contract); `ContentHelper.GetContentPropertiesForNestedContent[Element]` = false positive (parses **Perplex** block elements, not Umbraco NC — left, validate vs Perplex v4).

**New open blockers from the review (deferred — decisions/security):** BLOCKER-10 access-control regressions (Hangfire dashboard → any auth user; Export/Import/Preview workspaceViews shown to all users/docs); BLOCKER-11 `DataComposer.EnsureDataTypeExists` looks up by alias→dup data types on upgrade; BLOCKER-07/RR-10 Bundling throws at render. Plus: `IsBlockList()` on `UmbracoPropertyInfo`, obsolete the Bundling tag-helpers, consumer breaking-changes guide.

**Residuals to validate with real data/env:** NC→BlockList migration value-transform shape + per-item partial commit (legacy DB w/ NC content); `PropertyType.Nested` BlockList schema behaviour; uSync Publisher push E2E.

**Git:** changelog committed (`2860bb6e8`). Session-5 code fixes + `REVIEW_FINDINGS.md` + `N3ONestedContentMigrationPlan.cs` are **uncommitted** in the working tree.

---

### Session 4 — Phase-2 runtime correctness batch (multi-agent workflow)

Ran a 7-task investigate→implement→review workflow; all edits verified by a full `dotnet build` (0 errors). Each API was confirmed against the installed v17 assemblies (MetadataLoadContext reflection), not from memory.

| Task | Outcome |
|---|---|
| RR-02 / BLOCKER-04 Engage Cockpit factory | ✅ `TelethonOnAirCockpitSegmentRuleFactory` implemented + both DI registrations re-enabled. v17.2.2: `ICockpitSegmentRuleFactory`/`CockpitSegmentRule` in `Umbraco.Engage.Web.Cockpit.Segments`; `ISegmentRule`/`ISegmentRuleFactory` moved to `...Infrastructure.Personalization.Segments.Rules`. AngularJS telethon UI still blocked. |
| RR-03 GetNestedPropertySchemaHandler | ✅ Removed — dead (zero callers); handler + query deleted; `NestedSchemaRes`/mapping kept (still live). |
| RR-07 GetPreviewUrlAsync | ✅ Base impl added (mirrors core `NewDefaultUrlProvider`, key-based, uses `this.Alias`). |
| RR-08 Data controllers auth | ✅ `[Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]` on Content/ContentTypes/DataTypes controllers. |
| RR-09 SaveAndPublish | ✅ Already fixed (surfaces EventMessages + StatusType). |
| BLOCKER-06 NC migration registration | ✅ `N3ONestedContentMigrationPlan` added, auto-discovered (no composer). ⚠️ Runs on startup per site; no-ops without NC data types; destructive on legacy DBs — per-site backup/dry-run checklist still applies before live run. |
| BLOCKER-09 version tags | ✅ `13.0.0`→`17.0.0` across 114 csproj (placeholder; re-stamp CalVer at publish). |
| Forms version skew | ✅ Aligned to 17.0.1 (Forms + StaticAssets). |
| Perplex dead code | ✅ Deleted unreachable `GetOrCreateDataTypeContainer`. |
| **RR-10 Bundling** | ⚠️ **DECISION NEEDED** — project fully orphaned (zero consumers). Recommend delete (project + 2 sln GUID entries + 2 inert `Layout.cshtml` lines) OR build Vite/ESM replacement. Not actioned. |

**Smoke-tested live (app run + DB probes):**
- RR-08 auth ✓ — `POST /umbraco/api/datatypes/find` and `/contenttypes/find` return **401** unauthenticated; `/umbraco` backoffice → 200.
- RR-02 Engage ✓ — clean boot with the new DI registrations; no Engage/segment/resolve errors (container builds = registrations valid).
- BLOCKER-06 ⚠️→✓ — testing caught the registered migration crashing on startup (`Invalid column name 'id'` — the pre-written SQL wasn't v17-schema-correct). **Fixed** `NestedContentToBlockListMigration.cs` against the live v17 schema (`umbracoDataType.nodeId`/`propertyEditorUiAlias`; `cmsContentType`+`umbracoNode.uniqueId` for the content-type key). Re-run: plan now completes + advances state. The JSON value-transform shape is still unvalidated (demo DB has 0 NC data types) — dry-run on a real legacy DB before any live run.

Still **not done** (external/blocked/decision): RR-01 uSync Publisher (Jumoo docs), BLOCKER-02 Perplex stable v4, BLOCKER-08 Forms license, RR-10 Bundling (delete-vs-implement decision), Cropper/Uploader native-picker decision, live-render fixtures, per-site content data migration runs, AngularJS telethon UI.

---

### Session 3 — Bellissima frontend (BLOCKER-07) — see below

Run command:
```
cd "D:\AI Migration Test\N3O.Umbraco\src\DemoSite\DemoSite.Web"
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --no-build -c Debug
```
URLs: `http://localhost:6000` / `https://localhost:6001` (Chrome blocks :6000 as an unsafe port — use :6001 for
the backoffice). Backoffice login: `talha.malik@n3o.ltd`. DB = SQL Server `(local)\DemoSite.Web` (already installed).

Key docs:
- `BELLISSIMA_MIGRATION_LOG.md` — full step-by-step log of this session (scoping → fan-out → build → test → fix).
- `BELLISSIMA_MIGRATION_GUIDE.md` — the reusable AngularJS→Bellissima migration guide (extension-type cookbook).
- `MIGRATION_PLAN.md` / `MIGRATION_BLOCKERS.md` — overall plan + blockers (BLOCKER-07 updated).

---

## What Was Done This Session (BLOCKER-07 — Bellissima frontend)

Migrated **all 16 AngularJS backoffice plugin areas** from `package.manifest` + AngularJS controllers to
`umbraco-package.json` + Lit web components (parallel 16-subagent workflow). Backend C# APIs unchanged.

**15 done, 1 blocked:**

| Plugin | Extension type | Status |
|---|---|---|
| WelcomeDashboard | dashboard | ✅ done — **rendered live & verified** |
| Scheduler | dashboard (Hangfire iframe) | ✅ done — **rendered live & verified** |
| Data.ImportDataEditor | propertyEditorUi | ✅ done — **data type resolves (verified)** |
| Data.ImportNoticesViewer | propertyEditorUi | ✅ done — **data type resolves (verified)** |
| Data.Import | workspaceView | ✅ done — not yet live-rendered |
| Data.Export | workspaceView | ✅ done — not yet live-rendered |
| Cloud.Platforms.Preview | workspaceView | ✅ done — not yet live-rendered |
| Blocks.Preview | blockEditorCustomView | ✅ done — not yet live-rendered |
| Cells | propertyEditorUi (Handsontable) | ✅ done — not yet live-rendered |
| SerpEditor | propertyEditorUi | ✅ done — not yet live-rendered |
| TextResourceEditor | propertyEditorUi | ✅ done — not yet live-rendered |
| EditorJs | propertyEditorUi | ✅ done — not yet live-rendered (modal imports unverified) |
| Cropper | propertyEditorUi ×2 | ✅ done — **PENDING DECISION** (see below) |
| Uploader | propertyEditorUi | ✅ done — **PENDING DECISION** (see below) |
| Blazor.BackOffice | bundle | ✅ done — not yet live-rendered (jQuery-dependent loader) |
| telethon-on-air-rule | (Engage segment rule) | 🚫 BLOCKED on BLOCKER-04; placeholder only, `package.manifest` kept as `.bak` |

**Critical bug found & fixed during testing:** custom `propertyEditorUi.alias` (and `meta.propertyEditorSchemaAlias`)
MUST equal the backend `[DataEditor]` alias — not a new `N3O.PropertyEditorUi.*` alias — or existing data types
show "Property Editor UI not found" (v17 stores `editorUiAlias` separately from the v13 single alias). Fixed for
all 8 custom editors. Backend aliases: Cells `N3O.Umbraco.Cells`, Cropper `N3O.Umbraco.Cropper`, EditorJs
`N3O.Umbraco.EditorJs`, SerpEditor `N3O.Umbraco.SerpEditor`, Uploader `N3O.Umbraco.Uploader`, ImportDataEditor
`N3O.Umbraco.Data.ImportDataEditor`, ImportNoticesViewer `N3O.Umbraco.Data.ImportNoticesViewer`, TextResourceEditor
**`N3O.Umbraco.TemplateTextEditor`** (note: backend alias ≠ folder name).

**Verified:** build 0 errors; all 17 JS files syntax-valid; all 16 manifests valid JSON; app boots with no manifest
parse errors; all migrated assets served HTTP 200 from content-root `App_Plugins`; backoffice loads with no console
errors from N3O code; WelcomeDashboard + Scheduler render live; both Data data types resolve their UI.

---

## PENDING DECISION — Cropper & Uploader (awaiting Talha)

Cropper and Uploader were ported 1:1 and still use bundled libs (cropperjs + Formstone) that need a **global
jQuery the v17 backoffice no longer ships** (Uploader currently CDN-loads jQuery on demand). A header comment has
been added to `cropper.js` and `uploader.js` flagging that they **may or may not** be re-migrated to use Umbraco's
**native media/image picker + built-in Image Cropper** instead. **Do not rewrite these until Talha confirms.**

---

## Remaining Work

### BLOCKER-07 finish (next session)
- **Live-render testing** of the not-yet-rendered editors. The demo DB has **no content nodes and no doctypes**, so
  this requires building fixtures: create a data type per property editor (Cells/Cropper/EditorJs/SerpEditor/
  TextResourceEditor/Uploader) → add to a Document Type → create content → open it. Opening any document also shows
  the workspace-view tabs (Data.Import/Export, Cloud.Platforms.Preview) and (with a Block Grid property) Blocks.Preview.
- **Runtime risks to watch when rendering** (flagged in the log): EditorJs uses `@umbraco-cms/backoffice/modal`,
  `/media` (UMB_MEDIA_PICKER_MODAL), `/multi-url-picker` (UMB_LINK_PICKER_MODAL) — unverified import paths/shapes;
  Cloud.Platforms.Preview uses `UMB_DOCUMENT_WORKSPACE_CONTEXT.getData()` field shapes unverified; Blocks.Preview uses
  `@umbraco-cms/backoffice/block` contexts + BlockGridValue body shape (medium confidence); Blazor loader + Cropper/
  Uploader need global jQuery.
- `telethon-on-air-rule`: unblock with BLOCKER-04 (implement Engage `ICockpitSegmentRuleFactory`), then wire the
  Engage v17 client segment-rule registration API.

### Other open items (unchanged from prior sessions)
- RR-02 Engage `TelethonOnAirCockpitSegmentRuleFactory` (BLOCKER-04), RR-03 `GetNestedPropertySchemaHandler`,
  RR-07 `GetPreviewUrlAsync`, RR-08 Data controllers auth, RR-09 `SaveAndPublish` error handling, RR-10 Bundling.
- BLOCKER-05 uSync Publisher (`SyncContentHandler`), BLOCKER-06 Nested Content DB migration registration,
  BLOCKER-02 Perplex stable v4, BLOCKER-08 Forms license, BLOCKER-09 assembly `<Version>` tags (still `13.0.0`).

---

## Notes for the next session
- No commits were made this session (Talha handles commits). Working tree has the 16 new `umbraco-package.json`
  + Lit `.js` files and deleted AngularJS `package.manifest`/controllers/views.
- DemoSite `App_Plugins` (both content-root and wwwroot) are gitignored build outputs — the `.targets` in each
  `*.StaticAssets` project copy `App_Plugins/<name>/**` into the consuming site on build. Source of truth = the
  `*.StaticAssets` project folders.
- Only 11 of 16 plugins are project-referenced by DemoSite.Web; to runtime-test Blazor.BackOffice, Blocks.Preview,
  Cells, EditorJs, Cloud.Platforms.Preview you must add their `*.StaticAssets` `ProjectReference` (+ `.targets`
  `Import`) to `DemoSite.Web.csproj`.
