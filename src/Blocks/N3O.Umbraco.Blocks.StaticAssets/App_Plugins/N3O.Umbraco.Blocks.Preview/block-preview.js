import { LitElement as B, html as C, css as N, state as k, customElement as P } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as U } from "@umbraco-cms/backoffice/element-api";
import { UMB_BLOCK_ENTRY_CONTEXT as W, UMB_BLOCK_MANAGER_CONTEXT as S } from "@umbraco-cms/backoffice/block";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as D } from "@umbraco-cms/backoffice/document";
var $ = Object.defineProperty, I = Object.getOwnPropertyDescriptor, E = (e) => {
  throw TypeError(e);
}, y = (e, t, r, n) => {
  for (var o = n > 1 ? void 0 : n ? I(t, r) : t, s = e.length - 1, a; s >= 0; s--)
    (a = e[s]) && (o = (n ? a(t, r, o) : a(o)) || o);
  return n && o && $(t, r, o), o;
}, g = (e, t, r) => t.has(e) || E("Cannot " + r), i = (e, t, r) => (g(e, t, "read from private field"), t.get(e)), d = (e, t, r) => t.has(e) ? E("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, r), h = (e, t, r, n) => (g(e, t, "write to private field"), t.set(e, r), r), f = (e, t, r) => (g(e, t, "access private method"), r), _, v, m, w, c, l, p, b, T, x, M, K;
const L = "n3o-block-preview";
let u = class extends U(B) {
  constructor() {
    super(), d(this, p), this._loaded = !1, d(this, _), d(this, v), d(this, m, ""), d(this, w), d(this, c), d(this, l), this.consumeContext(D, (e) => {
      if (!e)
        return;
      const t = e;
      this.observe(t.unique, (r) => {
        h(this, _, r);
      }, "_observeUnique"), this.observe(
        t.splitView.activeVariantsInfo,
        (r) => {
          var o;
          const n = (o = r == null ? void 0 : r[0]) == null ? void 0 : o.culture;
          h(this, m, n ?? "");
        },
        "_observeCulture"
      );
    }), this.consumeContext(W, (e) => {
      e && (this.observe(e.contentKey, (t) => {
        h(this, w, t);
      }, "_observeContentKey"), this.observe(e.contentElementTypeKey, (t) => {
        h(this, v, t);
      }, "_observeContentElementTypeKey"));
    }), this.consumeContext(S, (e) => {
      h(this, l, e);
    });
  }
  connectedCallback() {
    super.connectedCallback(), f(this, p, b).call(this, 0);
  }
  disconnectedCallback() {
    super.disconnectedCallback(), i(this, c) !== void 0 && (clearTimeout(i(this, c)), h(this, c, void 0));
  }
  // Re-render the preview when the block's data or settings change (matches the $watch debouncing).
  updated(e) {
    (e.has("content") || e.has("settings")) && this._loaded && f(this, p, b).call(this, 500);
  }
  render() {
    return C`
            ${this._loaded ? "" : C`<div class="preview-alert preview-alert-info">
                      <uui-loader style="color: #fff"></uui-loader>
                      Loading preview...
                  </div>`}
            <iframe class="block-preview-frame" style="display: none"></iframe>
        `;
  }
};
_ = /* @__PURE__ */ new WeakMap();
v = /* @__PURE__ */ new WeakMap();
m = /* @__PURE__ */ new WeakMap();
w = /* @__PURE__ */ new WeakMap();
c = /* @__PURE__ */ new WeakMap();
l = /* @__PURE__ */ new WeakMap();
p = /* @__PURE__ */ new WeakSet();
b = function(e) {
  i(this, c) !== void 0 && clearTimeout(i(this, c)), h(this, c, setTimeout(() => {
    f(this, p, M).call(this);
  }, e));
};
T = function() {
  if (!i(this, l))
    return null;
  const e = i(this, l).getLayouts(), t = i(this, l).getContents(), r = i(this, l).getSettings(), n = i(this, l).getExposes();
  return {
    layout: {
      "Umbraco.BlockGrid": e
    },
    contentData: t,
    settingsData: r,
    expose: n
  };
};
x = function(e) {
  return e ? `umb://element/${e.replace(/-/g, "")}` : "";
};
M = async function() {
  const e = f(this, p, T).call(this);
  if (!e || !i(this, v))
    return;
  const t = i(this, _) ?? "", r = f(this, p, x).call(this, i(this, w)), n = i(this, m) ?? "", o = `/umbraco/backoffice/api/blockPreviewBackoffice/previewGridBlock/?nodeKey=${t}&documentTypeKey=${i(this, v)}&contentUdi=${r}&culture=${n}`, s = await fetch(o, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(e)
  });
  if (!s.ok)
    return;
  const a = await s.json();
  this._loaded = !0, await this.updateComplete, f(this, p, K).call(this, a);
};
K = function(e) {
  const t = this.renderRoot.querySelector(".block-preview-frame");
  if (!t)
    return;
  const r = t.contentDocument ?? t.contentWindow.document;
  r.open(), r.write(e), r.close();
  const n = () => {
    const a = r.body.querySelector(".preview-content"), O = a ? a.scrollHeight : r.body.scrollHeight;
    t.style.height = `${O}px`, t.style.width = "100%", t.style.border = "none", t.style.display = "block", t.style.transform = "scale(0.9)";
  };
  let o = 0;
  const s = setInterval(() => {
    n(), ++o > 2 && clearInterval(s);
  }, 100);
};
u.styles = N`
        :host {
            display: block;
        }

        .preview-alert {
            background-color: #f0ac00;
            border: 1px solid transparent;
            border-radius: 0;
            margin-bottom: 20px;
            padding: 8px 35px 8px 14px;
            position: relative;
        }

        .preview-alert uui-loader {
            margin-right: 16px;
        }

        .preview-alert,
        .preview-alert a,
        .preview-alert h4 {
            color: #fff;
        }

        .preview-alert pre {
            white-space: normal;
        }

        .preview-alert-warning {
            background-color: #f0ac00;
            border-color: transparent;
            color: #fff;
        }

        .preview-alert-info {
            background-color: #3544b1;
            border-color: transparent;
            color: #fff;
        }

        .preview-alert-danger,
        .preview-alert-error {
            background-color: #d42054;
            border-color: transparent;
            color: #fff;
        }
    `;
y([
  k()
], u.prototype, "content", 2);
y([
  k()
], u.prototype, "settings", 2);
y([
  k()
], u.prototype, "_loaded", 2);
u = y([
  P(L)
], u);
const H = u;
export {
  u as N3oBlockPreviewElement,
  H as default
};
//# sourceMappingURL=block-preview.js.map
