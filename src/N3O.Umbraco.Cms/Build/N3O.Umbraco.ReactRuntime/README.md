# N3O.Umbraco.ReactRuntime

Shared, self-hosted backoffice runtime modules. Each entry is built once here and exposed to **every**
backoffice plugin via the import map in `../wwwroot/App_Plugins/N3O.Umbraco.ReactRuntime/umbraco-package.json`
(Umbraco merges import maps server-side). Plugins mark these modules `external` in their own
`vite.config.ts`, so they are resolved at runtime against this single shared copy instead of being
re-bundled per plugin.

Built by the `BuildClientApps` MSBuild target in `N3O.Umbraco.Cms.csproj` (`npm run build` →
`vite build -c vite.config.react.ts && vite build -c vite.config.rest.ts`).

## Modules

| Import specifier      | Purpose                                                            |
|-----------------------|--------------------------------------------------------------------|
| `react`               | Single shared React instance.                                      |
| `react/jsx-runtime`   | JSX automatic runtime.                                             |
| `react-dom`, `react-dom/client` | React DOM (+ client `createRoot`/`hydrateRoot`).         |
| `@n3o/backoffice-core` | Shared **authenticated fetch** for calling `[Authorize]` APIs.    |

## `@n3o/backoffice-core` — why it exists

In Umbraco 17 the backoffice authenticates to the server with an **OAuth bearer token, not cookies**. Any
custom backoffice `fetch` to an `[Authorize]` API controller (our `/umbraco/api/...` and
`/umbraco/backoffice/api/...` controllers) **must** send `Authorization: Bearer <token>` or it returns 401.

This module centralises that wrapper so no plugin re-implements it. It exposes:

- `createAuthFetch(config): AuthFetch` — builds an authenticated `fetch` from
  `authContext.getOpenApiConfiguration()` (the non-deprecated v17 API; `getLatestToken()` is deprecated).
- `UmbAuthFetchMixin(superClass)` — a Lit mixin that gives an element `this.authFetch` automatically
  (rebuilt whenever `UMB_AUTH_CONTEXT` changes), with an optional `authFetchChanged(fetch)` hook.
- `AuthFetch` — the function type `(input: string, init?: RequestInit) => Promise<Response>`.

## Adopting `@n3o/backoffice-core` in another plugin

1. **Mark it external** in the plugin's `Apps/vite.config.ts` (or, with the shared `n3oPluginConfig`
   preset, pass `additionalExternals: ['@n3o/backoffice-core']`):
   ```ts
   external: [/^@umbraco/, 'react', 'react-dom', 'react-dom/client', 'react/jsx-runtime', '@n3o/backoffice-core'],
   ```
2. **Declare the workspace dependency** so TypeScript resolves the types from the package itself (no `paths`,
   no copied `.d.ts`) — add to the plugin's `Apps/package.json`:
   ```json
   "devDependencies": { "@n3o/backoffice-core": "*" }
   ```
   It resolves through the npm workspace (rooted at `src/`); the package's `types` field points at the
   canonical source. It is externalized at build and resolved at runtime via the import map, so it is never
   bundled into the plugin.
3. **Use it.** Either the mixin (zero boilerplate):
   ```ts
   import { UmbAuthFetchMixin } from '@n3o/backoffice-core';
   import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

   class MyElement extends UmbAuthFetchMixin(UmbElementMixin(HTMLElement)) {
       authFetchChanged() { this.#render(); }          // re-render when auth becomes available
       async load() { const res = await this.authFetch!('/umbraco/api/...'); /* ... */ }
   }
   ```
   …or the helper directly when you already hold the auth context:
   ```ts
   import { createAuthFetch } from '@n3o/backoffice-core';
   this.consumeContext(UMB_AUTH_CONTEXT, (ctx) => {
       const authFetch = ctx ? createAuthFetch(ctx.getOpenApiConfiguration()) : null;
   });
   ```

No import-map entry or build wiring is needed in the consuming plugin — only the workspace devDependency and
marking it external.
