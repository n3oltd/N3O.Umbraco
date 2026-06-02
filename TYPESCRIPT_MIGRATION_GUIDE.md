# Bellissima Plugin → TypeScript + Vite Migration Guide

*Shared reference for all plugin-migration subagents. Created 2026-06-02 (session 6).*

You are converting **one Umbraco backoffice plugin project** from plain-JS Lit components to **TypeScript +
Vite**, the modern Umbraco-recommended build. The backend C# APIs and the `umbraco-package.json` registration
are **unchanged** — you only add a typed build pipeline and rewrite the component(s) in TypeScript.

A complete, verified-building reference already exists: **`Plugins/SerpEditor/N3O.Umbraco.SerpEditor.StaticAssets`**
(`ClientApp/` + the `BuildClientApp` MSBuild target in its `.csproj`). **Read it first and mirror it exactly.**
It builds with `dotnet build` → 0 errors and `npm run build` → emits `App_Plugins/.../serp-editor.js`.

---

## The recipe (mirror SerpEditor)

For each `*.StaticAssets` project, create a `ClientApp/` folder **next to** `App_Plugins/` (NOT inside it — files
inside `App_Plugins` get shipped; `ClientApp` must not be). It contains:

### `ClientApp/package.json`
```json
{
    "name": "n3o-umbraco-<plugin-kebab>",
    "version": "1.0.0",
    "private": true,
    "type": "module",
    "scripts": {
        "build": "tsc --noEmit && vite build",
        "watch": "vite build --watch"
    },
    "devDependencies": {
        "@umbraco-cms/backoffice": "17.3.5",
        "typescript": "~5.7.0",
        "vite": "^6.0.0"
    }
}
```
Add any **third-party libs** the plugin uses as real dependencies here (see "Third-party libraries" below).

### `ClientApp/tsconfig.json`
```json
{
    "compilerOptions": {
        "target": "ES2022",
        "module": "ESNext",
        "lib": ["ES2022", "DOM", "DOM.Iterable"],
        "moduleResolution": "bundler",
        "useDefineForClassFields": false,
        "experimentalDecorators": true,
        "skipLibCheck": true,
        "isolatedModules": true,
        "moduleDetection": "force",
        "noEmit": true,
        "strict": true,
        "noUnusedLocals": true,
        "noUnusedParameters": true,
        "noFallthroughCasesInSwitch": true
    },
    "include": ["src"]
}
```

### `ClientApp/vite.config.ts` — SINGLE-entry plugin
```ts
import { defineConfig } from 'vite';

export default defineConfig({
    build: {
        lib: {
            entry: 'src/<file>.ts',
            formats: ['es'],
            fileName: () => '<file>.js',
        },
        outDir: '../App_Plugins/<FolderName>',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
        },
    },
});
```
- `<file>.js` MUST match the filename the `umbraco-package.json` `element`/`js` path points at.
- `external: [/^@umbraco/]` keeps `@umbraco-cms/backoffice/*` imports as bare specifiers (Umbraco import-maps
  them at runtime). Everything else (your code + npm libs) is bundled into the one output file.
- `emptyOutDir: false` is **required** — the committed `umbraco-package.json` lives in that folder.

### `ClientApp/vite.config.ts` — MULTI-entry project (e.g. Data has 4 plugins in one project)
Use an entry **map** whose keys are the per-plugin output paths, with `outDir: '../App_Plugins'`:
```ts
import { defineConfig } from 'vite';

export default defineConfig({
    build: {
        lib: {
            entry: {
                'N3O.Umbraco.Data.Import/data-import': 'src/data-import.ts',
                'N3O.Umbraco.Data.Export/data-export': 'src/data-export.ts',
                'N3O.Umbraco.Data.ImportDataEditor/import-data-editor': 'src/import-data-editor.ts',
                'N3O.Umbraco.Data.ImportNoticesViewer/import-notices-viewer': 'src/import-notices-viewer.ts',
            },
            formats: ['es'],
        },
        outDir: '../App_Plugins',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
            output: { entryFileNames: '[name].js' },
        },
    },
});
```
(Each key resolves to `App_Plugins/<folder>/<file>.js`.) Confirm each path matches that plugin's
`umbraco-package.json` `element`.

### `ClientApp/.gitignore`
```
node_modules/
```

---

## MSBuild wiring (edit the project's `.csproj`)

Mirror SerpEditor's `.csproj`:

1. **Exclude built outputs from the static `<Content>` glob** (so the target can add them without duplicates):
```xml
<Content Include="App_Plugins\<FolderName>\**\*.*"
         Exclude="App_Plugins\<FolderName>\**\*.js;App_Plugins\<FolderName>\**\*.js.map">
    <ExcludeFromSingleFile>true</ExcludeFromSingleFile>
    <CopyToPublishDirectory>Always</CopyToPublishDirectory>
</Content>
```
(For a project covering multiple plugin folders, exclude each folder's `**\*.js;**\*.js.map`, or use
`App_Plugins\**\*.js;App_Plugins\**\*.js.map`.)

2. **Add the build target** (before the SerpEditor `<None Include="build\...">` line is fine):
```xml
<PropertyGroup>
    <ClientAppDir>$(MSBuildProjectDirectory)\ClientApp</ClientAppDir>
</PropertyGroup>
<Target Name="BuildClientApp" BeforeTargets="AssignTargetPaths" Condition="Exists('$(ClientAppDir)\package.json')">
    <Message Text="Building <Plugin> client app (Vite)" Importance="high" />
    <Exec Command="npm ci" WorkingDirectory="$(ClientAppDir)" Condition="!Exists('$(ClientAppDir)\node_modules')" />
    <Exec Command="npm run build" WorkingDirectory="$(ClientAppDir)" />
    <ItemGroup>
        <Content Include="App_Plugins\<FolderName>\**\*.js;App_Plugins\<FolderName>\**\*.js.map">
            <ExcludeFromSingleFile>true</ExcludeFromSingleFile>
            <CopyToPublishDirectory>Always</CopyToPublishDirectory>
        </Content>
    </ItemGroup>
</Target>
```

---

## Writing the TypeScript component

Port the **existing** `App_Plugins/.../<file>.js` faithfully into `ClientApp/src/<file>.ts`. Same API calls,
same fields, same UX, same behaviour. **Do not add features or abstractions.** Modernize only the language:

- Use **Lit decorators**: `@customElement('n3o-...')`, `@property({...})`, `@state()` — imported from
  `@umbraco-cms/backoffice/external/lit` (it re-exports Lit + its decorators).
- Add **types**: implement the right Umbraco interface and type all params/returns/fields. Common ones:
  - Property editor UI: `implements UmbPropertyEditorUiElement`; `value` getter/setter; `set config(c: UmbPropertyEditorConfigCollection | undefined)`; dispatch `new UmbPropertyValueChangeEvent()` on change. Imports from `@umbraco-cms/backoffice/property-editor`.
  - Workspace view / content app: consume `UMB_DOCUMENT_WORKSPACE_CONTEXT` from `@umbraco-cms/backoffice/document`; `this.observe(...)`.
  - Block custom view: `implements UmbBlockEditorCustomViewElement` (or the element shape it had); imports from `@umbraco-cms/backoffice/block` / `.../block-custom-view`.
  - Dashboard / bundle / script: a plain `LitElement` (or the existing loader) — no special interface.
- Type DOM events: `(event.target as HTMLInputElement).value`, etc.
- Type `fetch` responses with a small `interface` for the JSON shape.
- Keep `export default <Class>` (Umbraco loads the default export) plus a named export.
- `UmbElementMixin(LitElement)` base, as the reference does.

`strict` is on. Prefer real Umbraco types. If a specific Umbraco generic is genuinely hard to satisfy, a
**narrow, commented** `as`-cast or a single `// @ts-expect-error <reason>` is acceptable rather than blocking —
keep it minimal and note it in your report. `skipLibCheck` is already on so the dependency's own types won't fail you.

---

## Third-party libraries

| Lib | Guidance |
|---|---|
| Available as a clean ESM npm package (`cropperjs`, `handsontable`, `@editorjs/*`, `editorjs-*`) | Add to `ClientApp/package.json` `dependencies`, `import` it in the `.ts`, let Vite bundle it. Delete the old vendored copy from `App_Plugins` (it becomes dead). Match the version the vendored copy used if discernible. |
| jQuery / Formstone (Cropper upload, Uploader) | **Pending product decision (do NOT rewrite to native pickers).** Keep the existing vendored files and the existing load mechanism; just port the wrapper to TS. If you bundle them, fine, but don't change behaviour. **Flag this in your report.** |
| Blazor loader (`N3O.Umbraco.Blazor.BackOffice.js`) | It's a non-Lit loader that relies on global `$`. Port to TS minimally (typed), keep it a `bundle`/`script` entry. Don't Lit-ify it. Flag the jQuery dependency. |

If a CSS file is imported (not inlined as `css\`\``), Vite emits a `style.css` — load it from the
`umbraco-package.json` or import it in the `.ts`; note any such change in your report.

---

## What you MUST NOT change
- `umbraco-package.json` contents — especially each `propertyEditorUi.alias` and `meta.propertyEditorSchemaAlias`,
  which **must stay equal to the backend `[DataEditor]` alias** (a hard-won fix; changing it breaks existing data
  types). You may keep the file exactly as-is.
- The output filename/path the manifest points at.
- Backend endpoints, request/response shapes, behaviour, UX.
- Don't rewrite Cropper/Uploader to Umbraco native media/image pickers (pending Talha's decision).

## Verify before reporting done (REQUIRED)
1. `cd ClientApp && npm install` then `npm run build` → must succeed (tsc strict passes + Vite emits).
2. Confirm the output `.js` landed at `App_Plugins/<FolderName>/<file>.js` and its `import`s of
   `@umbraco-cms/backoffice/*` are still **bare external specifiers** (grep the output).
3. Confirm the element is registered (the `@customElement('n3o-...')` tag name appears in the output) and
   `export ... default` is present.
4. `dotnet build <project>.csproj -c Debug` → **0 errors** (this exercises the MSBuild target end-to-end).
5. Old hand-written `App_Plugins/.../<file>.js` is overwritten by the build (expected) — don't hand-delete it.

## Report back
Plugin/project name; files created (ClientApp/*); component(s) ported; third-party libs (npm-bundled vs kept
vendored, with versions); any `as`/`@ts-expect-error` casts used and why; CSS handling; `npm run build` result;
`dotnet build` result (errors/warnings count); anything uncertain or flagged.
