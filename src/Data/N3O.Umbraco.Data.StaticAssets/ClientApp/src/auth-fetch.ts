// Local contract surface for the React apps. The authenticated-fetch capability now lives in the shared
// backoffice runtime (`@n3o/auth-fetch` from N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.ReactRuntime) and is
// resolved at runtime via the import map — see ../../README adoption notes. This module simply re-exports
// the shared `AuthFetch` type so existing imports (`import type { AuthFetch } from './auth-fetch'`) keep
// working unchanged. The host shells (data-export.ts / data-import.ts) get the implementation
// (createAuthFetch / UmbAuthFetchMixin) directly from `@n3o/auth-fetch`.
//
// In Umbraco 17 the custom Data API controllers ([Authorize]) reject a plain, token-less fetch with 401;
// AuthFetch always attaches the backoffice OAuth bearer token + credentials so they succeed.
export type { AuthFetch } from '@n3o/auth-fetch';
