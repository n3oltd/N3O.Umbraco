import { g as v } from "./react-dom-internals-C6fGbg64.js";
var u = { exports: {} }, e = {};
/**
 * @license React
 * react-jsx-runtime.production.js
 *
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */
var i;
function l() {
  if (i) return e;
  i = 1;
  var R = Symbol.for("react.transitional.element"), j = Symbol.for("react.fragment");
  function x(p, r, t) {
    var s = null;
    if (t !== void 0 && (s = "" + t), r.key !== void 0 && (s = "" + r.key), "key" in r) {
      t = {};
      for (var n in r)
        n !== "key" && (t[n] = r[n]);
    } else t = r;
    return r = t.ref, {
      $$typeof: R,
      type: p,
      key: s,
      ref: r !== void 0 ? r : null,
      props: t
    };
  }
  return e.Fragment = j, e.jsx = x, e.jsxs = x, e;
}
var a;
function m() {
  return a || (a = 1, u.exports = l()), u.exports;
}
var E = m();
const o = /* @__PURE__ */ v(E), c = o.jsx, f = o.jsxs, _ = o.Fragment;
export {
  _ as Fragment,
  c as jsx,
  f as jsxs
};
