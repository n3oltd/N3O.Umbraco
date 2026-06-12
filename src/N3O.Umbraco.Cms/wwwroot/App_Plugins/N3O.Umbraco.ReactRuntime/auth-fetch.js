var c = Object.defineProperty;
var a = (t, e, n) => e in t ? c(t, e, { enumerable: !0, configurable: !0, writable: !0, value: n }) : t[e] = n;
var s = (t, e, n) => a(t, typeof e != "symbol" ? e + "" : e, n);
import { UmbElementMixin as F } from "@umbraco-cms/backoffice/element-api";
import { UMB_AUTH_CONTEXT as o } from "@umbraco-cms/backoffice/auth";
function i(t) {
  return async (e, n = {}) => {
    const r = await t.token(), h = new Headers(n.headers);
    return r && h.set("Authorization", `Bearer ${r}`), fetch(e, { ...n, credentials: t.credentials, headers: h });
  };
}
const d = (t) => class extends t {
  constructor(...n) {
    super(...n);
    s(this, "authFetch", null);
    this.consumeContext(o, (r) => {
      var h;
      this.authFetch = r ? i(r.getOpenApiConfiguration()) : null, (h = this.authFetchChanged) == null || h.call(this, this.authFetch);
    });
  }
};
export {
  d as UmbAuthFetchMixin,
  F as UmbElementMixin,
  i as createAuthFetch
};
