# Session Handoff — N3O.Umbraco v17 Migration

*Updated: 2026-06-02 — use this to orient the next session*

---

## Current State

**Solution builds with 0 errors. App starts and serves HTTP 200.** Umbraco 17.3.5 on .NET 10, all 120 projects.
This session completed the bulk of **BLOCKER-07 (Bellissima frontend)** — see below.

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
