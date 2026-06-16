var C = (i) => {
  throw TypeError(i);
};
var d = (i, e, t) => e.has(i) || C("Cannot " + t);
var n = (i, e, t) => (d(i, e, "read from private field"), t ? t.call(i) : e.get(i)), p = (i, e, t) => e.has(i) ? C("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(i) : e.set(i, t), u = (i, e, t, s) => (d(i, e, "write to private field"), s ? s.call(i, t) : e.set(i, t), t), f = (i, e, t) => (d(i, e, "access private method"), t);
import { UmbConditionBase as v } from "@umbraco-cms/backoffice/extension-registry";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as y } from "@umbraco-cms/backoffice/document";
import { UMB_AUTH_CONTEXT as T } from "@umbraco-cms/backoffice/auth";
import { createAuthFetch as U } from "./auth-fetch.js";
var a, h, c, r, m, b;
class E extends v {
  constructor(t, s) {
    super(t, s);
    p(this, r);
    p(this, a);
    p(this, h, null);
    p(this, c, null);
    u(this, a, s), this.consumeContext(T, (o) => {
      u(this, h, o ? U(o.getOpenApiConfiguration()) : null), f(this, r, m).call(this);
    }), this.consumeContext(y, (o) => {
      o && this.observe(o.unique, (l) => {
        u(this, c, l ?? null), f(this, r, m).call(this);
      });
    });
  }
}
a = new WeakMap(), h = new WeakMap(), c = new WeakMap(), r = new WeakSet(), m = async function() {
  var s;
  const t = (s = n(this, a).config) == null ? void 0 : s.endpoint;
  !t || !n(this, c) || !n(this, h) || (this.permitted = await f(this, r, b).call(this, t, n(this, c)), n(this, a).onChange(this.permitted));
}, b = async function(t, s) {
  try {
    const o = await n(this, h).call(this, `${t}/${s}`, {
      headers: { Accept: "application/json" }
    });
    if (!o.ok)
      return !1;
    const l = await o.json();
    return typeof l.visible != "boolean" ? (console.error("[WorkspaceVisibilityCondition] Unexpected response shape from", t, "— expected { visible: boolean }, got", l), !1) : l.visible;
  } catch {
    return !1;
  }
};
export {
  E as WorkspaceVisibilityCondition,
  E as default
};
//# sourceMappingURL=workspace-visibility-condition.js.map
