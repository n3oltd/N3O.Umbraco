// Shared backoffice authenticated-fetch runtime, self-hosted as a single ESM module and exposed to all
// backoffice plugins via the import map (umbraco-package.json -> "@n3o/auth-fetch"). Same sharing model
// as the React runtime: built once here, consumed everywhere, never re-bundled per plugin.
//
// WHY: in Umbraco 17 the backoffice authenticates to the server with an OAuth bearer token (not cookies).
// Any custom backoffice fetch to an [Authorize] API controller (our /umbraco/api/... and
// /umbraco/backoffice/api/... controllers) MUST send `Authorization: Bearer <token>` or it gets 401.
//
// `@umbraco-cms/backoffice` is kept external (resolved at runtime against Umbraco's served bundle), so the
// element-api / auth imports below are NOT bundled into this file.
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_AUTH_CONTEXT } from '@umbraco-cms/backoffice/auth';

// Builds an authenticated fetch from a UmbOpenApiConfiguration (authContext.getOpenApiConfiguration()).
// Attaches the current bearer token + the configured credentials mode to every request. The token is
// resolved per request via config.token() (the non-deprecated v17 API; getLatestToken() is deprecated).
export function createAuthFetch(config) {
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
export const UmbAuthFetchMixin = (superClass) =>
    class extends superClass {
        authFetch = null;

        constructor(...args) {
            super(...args);

            this.consumeContext(UMB_AUTH_CONTEXT, (authContext) => {
                this.authFetch = authContext ? createAuthFetch(authContext.getOpenApiConfiguration()) : null;
                this.authFetchChanged?.(this.authFetch);
            });
        }
    };

export { UmbElementMixin };
