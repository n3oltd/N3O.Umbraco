// Ambient types for the shared `@n3o/auth-fetch` runtime module. The module's JS is provided at runtime
// by N3O.Umbraco.Cms (App_Plugins/N3O.Umbraco.ReactRuntime/auth-fetch.js) via the backoffice import map
// and is marked `external` in vite.config.ts, so it is never bundled here — this declaration only lets
// TypeScript resolve the import. To adopt @n3o/auth-fetch in another plugin, copy this file into its
// ClientApp/src (it mirrors N3O.Umbraco.ReactRuntime/src/auth-fetch.d.ts).
//
// This file has NO top-level imports so it stays a global ambient script and the `declare module` below
// is picked up project-wide; the few backoffice types it needs are referenced via inline `import(...)`.

declare module '@n3o/auth-fetch' {
    /**
     * An authenticated fetch: behaves like `fetch` but always attaches the backoffice OAuth bearer
     * token + the configured credentials mode, so custom [Authorize] API controllers don't 401.
     */
    export type AuthFetch = (input: string, init?: RequestInit) => Promise<Response>;

    /** Builds an {@link AuthFetch} from `authContext.getOpenApiConfiguration()`. */
    export function createAuthFetch(
        config: import('@umbraco-cms/backoffice/auth').UmbOpenApiConfiguration,
    ): AuthFetch;

    type Constructor<T = object> = new (...args: any[]) => T;

    type UmbControllerHostElement = import('@umbraco-cms/backoffice/controller-api').UmbControllerHostElement;

    /** A class that has gained `this.authFetch` via {@link UmbAuthFetchMixin}. */
    export declare class UmbAuthFetchElement {
        /** The shared authenticated fetch, or null until UMB_AUTH_CONTEXT resolves. */
        authFetch: AuthFetch | null;
        /** Optional hook invoked whenever `authFetch` is (re)built; override to re-render. */
        authFetchChanged?(authFetch: AuthFetch | null): void;
    }

    /**
     * Lit mixin that wires `this.authFetch` from UMB_AUTH_CONTEXT. Apply over an Umbraco element base,
     * e.g. `UmbAuthFetchMixin(UmbElementMixin(HTMLElement))`.
     */
    export function UmbAuthFetchMixin<T extends Constructor<UmbControllerHostElement>>(
        superClass: T,
    ): T & Constructor<UmbAuthFetchElement>;
}
