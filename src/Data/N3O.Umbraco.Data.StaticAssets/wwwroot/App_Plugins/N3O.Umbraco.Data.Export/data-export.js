import { customElement as be } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as ve } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as xe } from "@umbraco-cms/backoffice/document";
import { UmbAuthFetchMixin as fe } from "@n3o/backoffice-core";
import { useState as u, useRef as Y, useEffect as B, createElement as ge } from "react";
import { createRoot as ye } from "react-dom/client";
import { jsxs as i, jsx as o, Fragment as Z } from "react/jsx-runtime";
const ke = ".n3o-data-export{display:block;padding:var(--uui-size-space-4)}.n3o-data-export uui-box{--uui-box-default-padding: var(--uui-size-space-4);margin-bottom:var(--uui-size-space-3)}.n3o-data-export .nativeSelect{width:100%;max-width:420px;box-sizing:border-box;height:var(--uui-size-11, 36px);padding:0 var(--uui-size-space-3);font:inherit;color:var(--uui-color-text);background:var(--uui-color-surface);border:1px solid var(--uui-color-border);border-radius:var(--uui-border-radius)}.n3o-data-export .nativeSelect:focus{outline:none;border-color:var(--uui-color-focus);box-shadow:0 0 0 1px var(--uui-color-focus)}.n3o-data-export .nativeSelect:disabled{opacity:.5;cursor:not-allowed}.n3o-data-export .radioGroup{display:flex;flex-direction:row;flex-wrap:wrap;gap:var(--uui-size-space-2) var(--uui-size-space-5)}.n3o-data-export .radioOption,.n3o-data-export .toggleOption,.n3o-data-export .checkOption{display:flex;align-items:center;gap:var(--uui-size-space-2);cursor:pointer}.n3o-data-export .radioOption input,.n3o-data-export .toggleOption input,.n3o-data-export .checkOption input{cursor:pointer}.n3o-data-export .selectionCount{font-size:var(--uui-type-small-size);color:var(--uui-color-text-alt)}.n3o-data-export .selectionActions{display:flex;gap:var(--uui-size-space-2);margin-bottom:var(--uui-size-space-3)}.n3o-data-export .checkboxGrid{display:grid;grid-template-columns:repeat(auto-fill,minmax(180px,1fr));gap:var(--uui-size-space-1) var(--uui-size-space-5)}.n3o-data-export .emptyState{margin:0;color:var(--uui-color-text-alt);font-style:italic}.n3o-data-export .hint{margin:0 0 var(--uui-size-space-4);color:var(--uui-color-text-alt)}.n3o-data-export .errorBox{display:flex;align-items:center;gap:var(--uui-size-space-3);margin-bottom:var(--uui-size-space-4);padding:var(--uui-size-space-4) var(--uui-size-space-5);border-radius:var(--uui-border-radius);background:var(--uui-color-danger);color:var(--uui-color-danger-contrast)}.n3o-data-export .progress{display:flex;flex-direction:column;gap:var(--uui-size-space-2);margin-bottom:var(--uui-size-space-4);color:var(--uui-color-text-alt)}.n3o-data-export .actions{margin-top:var(--uui-size-space-4)}.n3o-data-export .btn{font:inherit;font-weight:700;line-height:1;display:inline-flex;align-items:center;gap:var(--uui-size-space-2);padding:0 var(--uui-size-space-4);height:var(--uui-size-11, 36px);border:1px solid transparent;border-radius:var(--uui-border-radius);cursor:pointer;box-sizing:border-box}.n3o-data-export .btn--compact{height:var(--uui-size-9, 30px);padding:0 var(--uui-size-space-3);font-size:var(--uui-type-small-size)}.n3o-data-export .btn--secondary{background:var(--uui-color-surface);color:var(--uui-color-text);border-color:var(--uui-color-border)}.n3o-data-export .btn--secondary:hover:not(:disabled){background:var(--uui-color-surface-emphasis);border-color:var(--uui-color-border-emphasis)}.n3o-data-export .btn--primary{background:var(--uui-color-default);color:var(--uui-color-default-contrast)}.n3o-data-export .btn--primary.btn--positive{background:var(--uui-color-positive);color:var(--uui-color-positive-contrast)}.n3o-data-export .btn--primary:hover:not(:disabled){background:var(--uui-color-positive-emphasis, var(--uui-color-positive))}.n3o-data-export .btn:disabled{opacity:.5;cursor:not-allowed}";
function Ce(t, a) {
  const [n, b] = u([]), [c, f] = u([]);
  return B(() => {
    if (!t || !a)
      return;
    let h = !0;
    return (async () => {
      const [O, D] = await Promise.all([
        a(`/umbraco/backoffice/api/ContentTypes/${t}/relations?type=descendant`, {
          headers: { Accept: "application/json" }
        }),
        a("/umbraco/backoffice/api/Exports/lookups/contentMetadata", {
          headers: { Accept: "application/json" }
        })
      ]), C = await O.json(), v = await D.json();
      for (const m of v)
        m.selected = m.autoSelected;
      v.sort((m, g) => m.displayOrder - g.displayOrder), h && (b(C), f(v));
    })(), () => {
      h = !1;
    };
  }, [t, a]), { contentTypes: n, metadatas: c };
}
function we({ contentKey: t, authFetch: a }) {
  const { contentTypes: n, metadatas: b } = Ce(t, a), [c, f] = u(null), [h, U] = u("excel"), [O, D] = u(!1), [C, v] = u([]), [m, g] = u([]), [p, G] = u(!1), [te, P] = u(""), [q, H] = u(null), T = Y(0), W = Y(void 0);
  B(() => () => {
    clearTimeout(W.current), T.current += 1;
  }, []), B(() => {
    v(b);
  }, [b]);
  const ae = async (e) => {
    if (!e || !a) {
      g([]);
      return;
    }
    const r = await a(`/umbraco/backoffice/api/Exports/exportableProperties/${e.alias}`, {
      headers: { Accept: "application/json" }
    }).then((l) => l.json());
    for (const l of r)
      l.selected = !1;
    g(r);
  }, oe = (e) => {
    const r = e.target.value, l = n.find((s) => s.alias === r) ?? null;
    f(l), ae(l);
  }, E = (e) => {
    G(!1), P(""), H(e);
  }, re = (e) => {
    const r = T.current, l = async (s, N) => {
      if (T.current !== r) {
        N(new Error("poll cancelled"));
        return;
      }
      const d = await a(`/umbraco/backoffice/api/Exports/export/${e}/progress`, {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "GET"
      });
      if (!d.ok) {
        const M = await d.json();
        E(String(M)), N(M);
        return;
      }
      const x = await d.json();
      x.isComplete === !0 ? s(x) : (P(x.text), W.current = setTimeout(() => void l(s, N), 2500));
    };
    return new Promise(l);
  }, ne = async () => {
    if (clearTimeout(W.current), T.current += 1, G(!0), P(""), H(null), !c) {
      E("Please select a content type");
      return;
    }
    const e = C.filter((d) => d.selected).map((d) => d.id), r = m.filter((d) => d.selected).map((d) => d.alias);
    if (!r.length && !e.length) {
      E("At least one property or metadata field must be selected");
      return;
    }
    const l = {
      format: h,
      includeUnpublished: O,
      metadata: e,
      properties: r
    }, s = await a(
      `/umbraco/backoffice/api/Exports/export/${t}/${c.alias}`,
      {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "POST",
        body: JSON.stringify(l)
      }
    );
    if (!s.ok) {
      const d = await s.json();
      E(String(d));
      return;
    }
    const N = await s.json();
    re(N.id).then(async (d) => {
      var K, Q;
      const x = await a(`/umbraco/backoffice/api/Exports/export/${d.id}/file`, {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "GET"
      });
      if (!x.ok) {
        E(String(await x.json()));
        return;
      }
      const M = await x.blob(), he = ((K = ((x.headers.get("Content-Disposition") ?? "").split(";")[1] ?? "").split("=")[1]) == null ? void 0 : K.replaceAll('"', "")) ?? "export", me = new Blob([M]), F = window.URL.createObjectURL(me), w = document.createElement("a");
      w.href = F, w.setAttribute("download", he), document.body.appendChild(w), w.click(), (Q = w.parentNode) == null || Q.removeChild(w), window.URL.revokeObjectURL(F), G(!1), P("");
    }).catch(() => {
    });
  }, ie = () => v((e) => e.map((r) => ({ ...r, selected: !0 }))), se = () => v((e) => e.map((r) => ({ ...r, selected: !1 }))), ce = () => g((e) => e.map((r) => ({ ...r, selected: !0 }))), le = () => g((e) => e.map((r) => ({ ...r, selected: !1 }))), de = (e, r) => v((l) => l.map((s) => s === e ? { ...s, selected: r } : s)), pe = (e, r) => g((l) => l.map((s) => s === e ? { ...s, selected: r } : s)), J = C.filter((e) => e.selected).length, V = m.filter((e) => e.selected).length, X = J > 0 || V > 0, ue = !!c && X && !p && !!a;
  return /* @__PURE__ */ i("div", { className: "n3o-data-export", children: [
    /* @__PURE__ */ i("uui-box", { headline: "Export options", children: [
      /* @__PURE__ */ o(
        "umb-property-layout",
        {
          label: "Content type",
          description: "The descendant type to export. Properties available for export depend on this.",
          mandatory: !0,
          children: /* @__PURE__ */ o("div", { slot: "editor", children: /* @__PURE__ */ i(
            "select",
            {
              className: "nativeSelect",
              value: (c == null ? void 0 : c.alias) ?? "",
              onChange: oe,
              disabled: p || n.length === 0,
              children: [
                /* @__PURE__ */ o("option", { value: "", disabled: !0, children: "Select a content type…" }),
                n.map((e) => /* @__PURE__ */ o("option", { value: e.alias, children: e.name }, e.alias))
              ]
            }
          ) })
        }
      ),
      /* @__PURE__ */ o(
        "umb-property-layout",
        {
          label: "File format",
          description: "Choose the file format for the exported data.",
          mandatory: !0,
          children: /* @__PURE__ */ o("div", { slot: "editor", children: /* @__PURE__ */ i("div", { className: "radioGroup", children: [
            /* @__PURE__ */ i("label", { className: "radioOption", children: [
              /* @__PURE__ */ o(
                "input",
                {
                  type: "radio",
                  name: "format",
                  value: "excel",
                  checked: h === "excel",
                  onChange: (e) => U(e.target.value),
                  disabled: p
                }
              ),
              /* @__PURE__ */ o("span", { children: "Excel (.xlsx)" })
            ] }),
            /* @__PURE__ */ i("label", { className: "radioOption", children: [
              /* @__PURE__ */ o(
                "input",
                {
                  type: "radio",
                  name: "format",
                  value: "csv",
                  checked: h === "csv",
                  onChange: (e) => U(e.target.value),
                  disabled: p
                }
              ),
              /* @__PURE__ */ o("span", { children: "CSV (.csv)" })
            ] })
          ] }) })
        }
      ),
      /* @__PURE__ */ o(
        "umb-property-layout",
        {
          label: "Include unpublished",
          description: "When enabled, unpublished content is included in the export.",
          children: /* @__PURE__ */ o("div", { slot: "editor", children: /* @__PURE__ */ i("label", { className: "toggleOption", children: [
            /* @__PURE__ */ o(
              "input",
              {
                type: "checkbox",
                checked: O,
                onChange: (e) => D(e.target.checked),
                disabled: p
              }
            ),
            /* @__PURE__ */ o("span", { children: "Include unpublished content" })
          ] }) })
        }
      )
    ] }),
    /* @__PURE__ */ i("uui-box", { headline: "Metadata fields", children: [
      /* @__PURE__ */ i("div", { slot: "header-actions", className: "selectionCount", children: [
        J,
        " selected"
      ] }),
      C.length === 0 ? /* @__PURE__ */ o("p", { className: "emptyState", children: "No metadata fields are available." }) : /* @__PURE__ */ i(Z, { children: [
        /* @__PURE__ */ i("div", { className: "selectionActions", children: [
          /* @__PURE__ */ o(
            "button",
            {
              type: "button",
              className: "btn btn--secondary btn--compact",
              disabled: p,
              onClick: ie,
              children: "Select all"
            }
          ),
          /* @__PURE__ */ o(
            "button",
            {
              type: "button",
              className: "btn btn--secondary btn--compact",
              disabled: p,
              onClick: se,
              children: "Clear"
            }
          )
        ] }),
        /* @__PURE__ */ o("div", { className: "checkboxGrid", children: C.map((e) => /* @__PURE__ */ i("label", { className: "checkOption", children: [
          /* @__PURE__ */ o(
            "input",
            {
              type: "checkbox",
              checked: !!e.selected,
              onChange: (r) => de(e, r.target.checked),
              disabled: p
            }
          ),
          /* @__PURE__ */ o("span", { children: e.name })
        ] }, e.id)) })
      ] })
    ] }),
    /* @__PURE__ */ i("uui-box", { headline: "Properties", children: [
      /* @__PURE__ */ i("div", { slot: "header-actions", className: "selectionCount", children: [
        V,
        " selected"
      ] }),
      c ? m.length === 0 ? /* @__PURE__ */ o("p", { className: "emptyState", children: "This content type has no exportable properties." }) : /* @__PURE__ */ i(Z, { children: [
        /* @__PURE__ */ i("div", { className: "selectionActions", children: [
          /* @__PURE__ */ o(
            "button",
            {
              type: "button",
              className: "btn btn--secondary btn--compact",
              disabled: p,
              onClick: ce,
              children: "Select all"
            }
          ),
          /* @__PURE__ */ o(
            "button",
            {
              type: "button",
              className: "btn btn--secondary btn--compact",
              disabled: p,
              onClick: le,
              children: "Clear"
            }
          )
        ] }),
        /* @__PURE__ */ o("div", { className: "checkboxGrid", children: m.map((e) => /* @__PURE__ */ i("label", { className: "checkOption", children: [
          /* @__PURE__ */ o(
            "input",
            {
              type: "checkbox",
              checked: !!e.selected,
              onChange: (r) => pe(e, r.target.checked),
              disabled: p
            }
          ),
          /* @__PURE__ */ o("span", { children: e.columnTitle })
        ] }, e.alias)) })
      ] }) : /* @__PURE__ */ o("p", { className: "emptyState", children: "Select a content type to see its exportable properties." })
    ] }),
    !p && c && !X ? /* @__PURE__ */ o("p", { className: "hint", children: "Select at least one metadata field or property to export." }) : null,
    q ? /* @__PURE__ */ i("div", { className: "errorBox", children: [
      /* @__PURE__ */ o("uui-icon", { name: "icon-alert" }),
      /* @__PURE__ */ o("span", { children: q })
    ] }) : null,
    p ? /* @__PURE__ */ i("div", { className: "progress", children: [
      /* @__PURE__ */ o("uui-loader-bar", {}),
      /* @__PURE__ */ o("span", { children: te || "Preparing export…" })
    ] }) : null,
    /* @__PURE__ */ o("div", { className: "actions", children: /* @__PURE__ */ o(
      "button",
      {
        type: "button",
        className: "btn btn--primary btn--positive",
        disabled: !ue,
        onClick: () => void ne(),
        children: p ? "Exporting…" : "Export"
      }
    ) }),
    /* @__PURE__ */ o("style", { children: ke })
  ] });
}
var Ee = Object.getOwnPropertyDescriptor, ee = (t) => {
  throw TypeError(t);
}, Ne = (t, a, n, b) => {
  for (var c = b > 1 ? void 0 : b ? Ee(a, n) : a, f = t.length - 1, h; f >= 0; f--)
    (h = t[f]) && (c = h(c) || c);
  return c;
}, I = (t, a, n) => a.has(t) || ee("Cannot " + n), y = (t, a, n) => (I(t, a, "read from private field"), n ? n.call(t) : a.get(t)), A = (t, a, n) => a.has(t) ? ee("Cannot add the same private member more than once") : a instanceof WeakSet ? a.add(t) : a.set(t, n), j = (t, a, n, b) => (I(t, a, "write to private field"), a.set(t, n), n), $ = (t, a, n) => (I(t, a, "access private method"), n), k, z, _, S, R;
const ze = "n3o-data-export";
let L = class extends fe(ve(HTMLElement)) {
  constructor() {
    super(), A(this, S), A(this, k), A(this, z), A(this, _, null);
    const t = this.attachShadow({ mode: "open" });
    j(this, z, document.createElement("div")), t.appendChild(y(this, z)), this.consumeContext(xe, (a) => {
      a && this.observe(
        a.unique,
        (n) => {
          n && n !== y(this, _) && (j(this, _, n), $(this, S, R).call(this));
        },
        "_observeUnique"
      );
    });
  }
  // Re-render when the shared authenticated fetch becomes available / changes (mixin hook).
  authFetchChanged(t) {
    $(this, S, R).call(this);
  }
  connectedCallback() {
    var t;
    (t = super.connectedCallback) == null || t.call(this), y(this, k) ?? j(this, k, ye(y(this, z))), $(this, S, R).call(this);
  }
  disconnectedCallback() {
    var t, a;
    (t = super.disconnectedCallback) == null || t.call(this), (a = y(this, k)) == null || a.unmount(), j(this, k, void 0);
  }
};
k = /* @__PURE__ */ new WeakMap();
z = /* @__PURE__ */ new WeakMap();
_ = /* @__PURE__ */ new WeakMap();
S = /* @__PURE__ */ new WeakSet();
R = function() {
  var t;
  (t = y(this, k)) == null || t.render(
    ge(we, {
      contentKey: y(this, _),
      authFetch: this.authFetch
    })
  );
};
L = Ne([
  be(ze)
], L);
const Ue = L;
export {
  L as N3oDataExportElement,
  Ue as default
};
//# sourceMappingURL=data-export.js.map
