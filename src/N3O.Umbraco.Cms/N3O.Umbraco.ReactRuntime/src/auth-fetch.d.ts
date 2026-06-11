// Type surface of the shared `@n3o/auth-fetch` runtime module (see auth-fetch.js). This is the canonical
// declaration; consumers copy it (or an equivalent) next to their app as an ambient `*.d.ts` so their
// TypeScript can resolve the externalized `@n3o/auth-fetch` import at build time.
//
// No top-level imports, so it stays a global ambient script and the `declare module` is picked up
// project-wide; the few backoffice types it needs are referenced via inline `import(...)`.

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
