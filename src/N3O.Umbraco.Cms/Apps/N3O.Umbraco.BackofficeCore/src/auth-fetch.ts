// Shared backoffice authenticated-fetch runtime, self-hosted as a single ESM module and exposed to all
// backoffice plugins via the import map (umbraco-package.json -> "@n3o/backoffice-core"). Same sharing
// model as the React runtime: built once here, consumed everywhere, never re-bundled per plugin. This
// file is also the canonical TYPE source: consumers depend on "@n3o/backoffice-core" as a workspace
// devDependency and TypeScript resolves types via the npm workspace symlink.
//
// WHY: in Umbraco 17 the backoffice authenticates to the server with an OAuth bearer token (not cookies).
// Any custom backoffice fetch to an [Authorize] API controller (our /umbraco/api/... and
// /umbraco/backoffice/api/... controllers) MUST send `Authorization: Bearer <token>` or it gets 401.
//
// `@umbraco-cms/backoffice` is kept external (resolved at runtime against Umbraco's served bundle), so the
// element-api / auth imports below are NOT bundled into this file.
import { UmbElementMixin, type UmbElement } from '@umbraco-cms/backoffice/element-api';
import { UMB_AUTH_CONTEXT, type UmbOpenApiConfiguration } from '@umbraco-cms/backoffice/auth';

/**
 * An authenticated fetch: behaves like `fetch` but always attaches the backoffice OAuth bearer
 * token + the configured credentials mode, so custom [Authorize] API controllers don't 401.
 */
export type AuthFetch = (input: string, init?: RequestInit) => Promise<Response>;

type Constructor<T = object> = new (...args: any[]) => T;

// Builds an authenticated fetch from a UmbOpenApiConfiguration (authContext.getOpenApiConfiguration()).
// Attaches the current bearer token + the configured credentials mode to every request. The token is
// resolved per request via config.token() (the non-deprecated v17 API; getLatestToken() is deprecated).
export function createAuthFetch(config: UmbOpenApiConfiguration): AuthFetch {
    return async (input, init = {}) => {
        const token = await config.token();
        const headers = new Headers(init.headers);

        if (token) {
            headers.set('Authorization', `Bearer ${token}`);
        }

        return fetch(input, { ...init, credentials: config.credentials, headers });
    };
}

// Lit mixin that gives a custom element `this.authFetch` with zero per-plugin boilerplate. It consumes
// UMB_AUTH_CONTEXT and (re)builds the authenticated fetch whenever the auth context changes, calling the
// optional `authFetchChanged(fetch)` hook so the element can re-render. Until the auth context resolves,
// `this.authFetch` is null.
//
// Usage: `class MyElement extends UmbAuthFetchMixin(UmbElementMixin(HTMLElement)) { ... }`
// (or extend any Umbraco element base). Then read `this.authFetch` / override `authFetchChanged`.
export const UmbAuthFetchMixin = <T extends Constructor<UmbElement>>(superClass: T) =>
    class extends superClass {
        /** The shared authenticated fetch, or null until UMB_AUTH_CONTEXT resolves. */
        authFetch: AuthFetch | null = null;

        /** Optional hook invoked whenever `authFetch` is (re)built; override to re-render. */
        authFetchChanged?(authFetch: AuthFetch | null): void;

        constructor(...args: any[]) {
            super(...args);

            this.consumeContext(UMB_AUTH_CONTEXT, (authContext) => {
                this.authFetch = authContext ? createAuthFetch(authContext.getOpenApiConfiguration()) : null;
                this.authFetchChanged?.(this.authFetch);
            });
        }
    };

export { UmbElementMixin };
