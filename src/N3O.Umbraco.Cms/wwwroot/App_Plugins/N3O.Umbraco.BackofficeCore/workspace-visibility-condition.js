var C = (i) => {
  throw TypeError(i);
};
var d = (i, e, t) => e.has(i) || C("Cannot " + t);
var o = (i, e, t) => (d(i, e, "read from private field"), t ? t.call(i) : e.get(i)), p = (i, e, t) => e.has(i) ? C("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(i) : e.set(i, t), l = (i, e, t, s) => (d(i, e, "write to private field"), s ? s.call(i, t) : e.set(i, t), t), m = (i, e, t) => (d(i, e, "access private method"), t);
import { UmbConditionBase as A } from "@umbraco-cms/backoffice/extension-registry";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as O } from "@umbraco-cms/backoffice/document";
import { UMB_AUTH_CONTEXT as U } from "@umbraco-cms/backoffice/auth";
import { createAuthFetch as _ } from "./auth-fetch.js";
var h, a, u, r, f, T;
class w extends A {
  constructor(t, s) {
    super(t, s);
    p(this, r);
    p(this, h);
    p(this, a, null);
    p(this, u, null);
    l(this, h, s), this.consumeContext(U, (n) => {
      l(this, a, n ? _(n.getOpenApiConfiguration()) : null), m(this, r, f).call(this);
    }), this.consumeContext(O, (n) => {
      n && this.observe(n.unique, (c) => {
        l(this, u, c ?? null), m(this, r, f).call(this);
      });
    });
  }
}
h = new WeakMap(), a = new WeakMap(), u = new WeakMap(), r = new WeakSet(), f = async function() {
  var s;
  const t = (s = o(this, h).config) == null ? void 0 : s.endpoint;
  !t || !o(this, u) || !o(this, a) || (this.permitted = await m(this, r, T).call(this, t, o(this, u)), o(this, h).onChange(this.permitted));
}, T = async function(t, s) {
  try {
    const n = await o(this, a).call(this, `${t}/${s}`, {
      headers: { Accept: "application/json" }
    });
    if (!n.ok)
      return !1;
    const c = await n.json();
    return (c == null ? void 0 : c.permitted) === !0;
  } catch {
    return !1;
  }
};
export {
  w as WorkspaceVisibilityCondition,
  w as default
};
//# sourceMappingURL=workspace-visibility-condition.js.map
