import { UmbElementMixin as l } from "@umbraco-cms/backoffice/element-api";
import { UMB_AUTH_CONTEXT as s } from "@umbraco-cms/backoffice/auth";
function c(n) {
  return async (r, t = {}) => {
    const e = await n.token(), h = new Headers(t.headers);
    return e && h.set("Authorization", `Bearer ${e}`), fetch(r, { ...t, credentials: n.credentials, headers: h });
  };
}
const i = (n) => class extends n {
  constructor(...r) {
    super(...r), this.authFetch = null, this.consumeContext(s, (t) => {
      var e;
      this.authFetch = t ? c(t.getOpenApiConfiguration()) : null, (e = this.authFetchChanged) == null || e.call(this, this.authFetch);
    });
  }
};
export {
  i as UmbAuthFetchMixin,
  l as UmbElementMixin,
  c as createAuthFetch
};
//# sourceMappingURL=auth-fetch.js.map
