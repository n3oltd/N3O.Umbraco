import { customElement as Y } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as Z } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as ee } from "@umbraco-cms/backoffice/document";
import { UmbAuthFetchMixin as te } from "@n3oltd/backoffice-core";
import { useState as g, useEffect as q, useRef as V, createElement as ae } from "react";
import { createRoot as oe } from "react-dom/client";
import { jsxs as p, jsx as o, Fragment as re } from "react/jsx-runtime";
const ne = ".n3o-data-export{display:block;padding:var(--uui-size-space-4)}.n3o-data-export uui-box{--uui-box-default-padding: var(--uui-size-space-4);margin-bottom:var(--uui-size-space-3)}.n3o-data-export .nativeSelect{width:100%;max-width:420px;box-sizing:border-box;height:var(--uui-size-11, 36px);padding:0 var(--uui-size-space-3);font:inherit;color:var(--uui-color-text);background:var(--uui-color-surface);border:1px solid var(--uui-color-border);border-radius:var(--uui-border-radius)}.n3o-data-export .nativeSelect:focus{outline:none;border-color:var(--uui-color-focus);box-shadow:0 0 0 1px var(--uui-color-focus)}.n3o-data-export .nativeSelect:disabled{opacity:.5;cursor:not-allowed}.n3o-data-export .radioGroup{display:flex;flex-direction:row;flex-wrap:wrap;gap:var(--uui-size-space-2) var(--uui-size-space-5)}.n3o-data-export .radioOption,.n3o-data-export .toggleOption,.n3o-data-export .checkOption{display:flex;align-items:center;gap:var(--uui-size-space-2);cursor:pointer}.n3o-data-export .radioOption input,.n3o-data-export .toggleOption input,.n3o-data-export .checkOption input{cursor:pointer}.n3o-data-export .selectionCount{font-size:var(--uui-type-small-size);color:var(--uui-color-text-alt)}.n3o-data-export .selectionActions{display:flex;gap:var(--uui-size-space-2);margin-bottom:var(--uui-size-space-3)}.n3o-data-export .checkboxGrid{display:grid;grid-template-columns:repeat(auto-fill,minmax(180px,1fr));gap:var(--uui-size-space-1) var(--uui-size-space-5)}.n3o-data-export .emptyState{margin:0;color:var(--uui-color-text-alt);font-style:italic}.n3o-data-export .hint{margin:0 0 var(--uui-size-space-4);color:var(--uui-color-text-alt)}.n3o-data-export .errorBox{display:flex;align-items:center;gap:var(--uui-size-space-3);margin-bottom:var(--uui-size-space-4);padding:var(--uui-size-space-4) var(--uui-size-space-5);border-radius:var(--uui-border-radius);background:var(--uui-color-danger);color:var(--uui-color-danger-contrast)}.n3o-data-export .progress{display:flex;flex-direction:column;gap:var(--uui-size-space-2);margin-bottom:var(--uui-size-space-4);color:var(--uui-color-text-alt)}.n3o-data-export .actions{margin-top:var(--uui-size-space-4)}.n3o-data-export .btn{font:inherit;font-weight:700;line-height:1;display:inline-flex;align-items:center;gap:var(--uui-size-space-2);padding:0 var(--uui-size-space-4);height:var(--uui-size-11, 36px);border:1px solid transparent;border-radius:var(--uui-border-radius);cursor:pointer;box-sizing:border-box}.n3o-data-export .btn--compact{height:var(--uui-size-9, 30px);padding:0 var(--uui-size-space-3);font-size:var(--uui-type-small-size)}.n3o-data-export .btn--secondary{background:var(--uui-color-surface);color:var(--uui-color-text);border-color:var(--uui-color-border)}.n3o-data-export .btn--secondary:hover:not(:disabled){background:var(--uui-color-surface-emphasis);border-color:var(--uui-color-border-emphasis)}.n3o-data-export .btn--primary{background:var(--uui-color-default);color:var(--uui-color-default-contrast)}.n3o-data-export .btn--primary.btn--positive{background:var(--uui-color-positive);color:var(--uui-color-positive-contrast)}.n3o-data-export .btn--primary:hover:not(:disabled){background:var(--uui-color-positive-emphasis, var(--uui-color-positive))}.n3o-data-export .btn:disabled{opacity:.5;cursor:not-allowed}";
function ie(e, a) {
  const [r, d] = g([]), [n, u] = g([]);
  return q(() => {
    if (!e || !a)
      return;
    let c = !0;
    return (async () => {
      const [i, x] = await Promise.all([
        a(`/umbraco/backoffice/api/ContentTypes/${e}/relations?type=descendant`, {
          headers: { Accept: "application/json" }
        }),
        a("/umbraco/backoffice/api/Exports/lookups/contentMetadata", {
          headers: { Accept: "application/json" }
        })
      ]), k = await i.json(), b = await x.json();
      for (const l of b)
        l.selected = l.autoSelected;
      b.sort((l, S) => l.displayOrder - S.displayOrder), c && (d(k), u(b));
    })(), () => {
      c = !1;
    };
  }, [e, a]), { contentTypes: r, metadatas: n };
}
function se(e) {
  const [a, r] = g(!1), [d, n] = g(""), [u, c] = g(null), m = V(0), i = V(void 0);
  q(() => () => {
    clearTimeout(i.current), m.current += 1;
  }, []);
  const x = (l) => {
    r(!1), n(""), c(l);
  }, k = (l) => {
    const S = m.current, E = async (y, v) => {
      if (m.current !== S) {
        v(new Error("poll cancelled"));
        return;
      }
      const h = await e(`/umbraco/backoffice/api/Exports/export/${l}/progress`, {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "GET"
      });
      if (!h.ok) {
        const w = await h.json();
        x(String(w)), v(w);
        return;
      }
      const z = await h.json();
      z.isComplete === !0 ? y(z) : (n(z.text), i.current = setTimeout(() => void E(y, v), 2500));
    };
    return new Promise(E);
  };
  return { processing: a, progress: d, errorMessage: u, doExport: async (l, S, E, y, v, h) => {
    if (clearTimeout(i.current), m.current += 1, r(!0), n(""), c(null), !h.length && !v.length) {
      x("At least one property or metadata field must be selected");
      return;
    }
    const z = {
      format: E,
      includeUnpublished: y,
      metadata: v,
      properties: h
    }, w = await e(
      `/umbraco/backoffice/api/Exports/export/${l}/${S}`,
      {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "POST",
        body: JSON.stringify(z)
      }
    );
    if (!w.ok) {
      const P = await w.json();
      x(String(P));
      return;
    }
    const $ = await w.json();
    k($.id).then(async (P) => {
      var t, s;
      const M = await e(`/umbraco/backoffice/api/Exports/export/${P.id}/file`, {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "GET"
      });
      if (!M.ok) {
        x(String(await M.json()));
        return;
      }
      const B = await M.blob(), U = ((t = ((M.headers.get("Content-Disposition") ?? "").split(";")[1] ?? "").split("=")[1]) == null ? void 0 : t.replaceAll('"', "")) ?? "export", D = new Blob([B]), T = window.URL.createObjectURL(D), N = document.createElement("a");
      N.href = T, N.setAttribute("download", U), document.body.appendChild(N), N.click(), (s = N.parentNode) == null || s.removeChild(N), window.URL.revokeObjectURL(T), r(!1), n("");
    }).catch(() => {
    });
  } };
}
function ce({
  contentTypes: e,
  contentType: a,
  format: r,
  includeUnpublished: d,
  processing: n,
  onContentTypeChange: u,
  onFormatChange: c,
  onIncludeUnpublishedChange: m
}) {
  return /* @__PURE__ */ p("uui-box", { headline: "Export options", children: [
    /* @__PURE__ */ o(
      "umb-property-layout",
      {
        label: "Content type",
        description: "The descendant type to export. Properties available for export depend on this.",
        mandatory: !0,
        children: /* @__PURE__ */ o("div", { slot: "editor", children: /* @__PURE__ */ p(
          "select",
          {
            className: "nativeSelect",
            value: (a == null ? void 0 : a.alias) ?? "",
            onChange: u,
            disabled: n || e.length === 0,
            children: [
              /* @__PURE__ */ o("option", { value: "", disabled: !0, children: "Select a content type…" }),
              e.map((i) => /* @__PURE__ */ o("option", { value: i.alias, children: i.name }, i.alias))
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
        children: /* @__PURE__ */ o("div", { slot: "editor", children: /* @__PURE__ */ p("div", { className: "radioGroup", children: [
          /* @__PURE__ */ p("label", { className: "radioOption", children: [
            /* @__PURE__ */ o(
              "input",
              {
                type: "radio",
                name: "format",
                value: "excel",
                checked: r === "excel",
                onChange: (i) => c(i.target.value),
                disabled: n
              }
            ),
            /* @__PURE__ */ o("span", { children: "Excel (.xlsx)" })
          ] }),
          /* @__PURE__ */ p("label", { className: "radioOption", children: [
            /* @__PURE__ */ o(
              "input",
              {
                type: "radio",
                name: "format",
                value: "csv",
                checked: r === "csv",
                onChange: (i) => c(i.target.value),
                disabled: n
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
        children: /* @__PURE__ */ o("div", { slot: "editor", children: /* @__PURE__ */ p("label", { className: "toggleOption", children: [
          /* @__PURE__ */ o(
            "input",
            {
              type: "checkbox",
              checked: d,
              onChange: (i) => m(i.target.checked),
              disabled: n
            }
          ),
          /* @__PURE__ */ o("span", { children: "Include unpublished content" })
        ] }) })
      }
    )
  ] });
}
function X({
  headline: e,
  selectedCount: a,
  items: r,
  getKey: d,
  getLabel: n,
  getChecked: u,
  processing: c,
  onToggle: m,
  onSelectAll: i,
  onClear: x,
  emptyState: k
}) {
  return /* @__PURE__ */ p("uui-box", { headline: e, children: [
    /* @__PURE__ */ p("div", { slot: "header-actions", className: "selectionCount", children: [
      a,
      " selected"
    ] }),
    r.length === 0 ? k : /* @__PURE__ */ p(re, { children: [
      /* @__PURE__ */ p("div", { className: "selectionActions", children: [
        /* @__PURE__ */ o(
          "button",
          {
            type: "button",
            className: "btn btn--secondary btn--compact",
            disabled: c,
            onClick: i,
            children: "Select all"
          }
        ),
        /* @__PURE__ */ o(
          "button",
          {
            type: "button",
            className: "btn btn--secondary btn--compact",
            disabled: c,
            onClick: x,
            children: "Clear"
          }
        )
      ] }),
      /* @__PURE__ */ o("div", { className: "checkboxGrid", children: r.map((b) => /* @__PURE__ */ p("label", { className: "checkOption", children: [
        /* @__PURE__ */ o(
          "input",
          {
            type: "checkbox",
            checked: !!u(b),
            onChange: (l) => m(b, l.target.checked),
            disabled: c
          }
        ),
        /* @__PURE__ */ o("span", { children: n(b) })
      ] }, d(b))) })
    ] })
  ] });
}
function le({ contentKey: e, authFetch: a }) {
  const { contentTypes: r, metadatas: d } = ie(e, a), { processing: n, progress: u, errorMessage: c, doExport: m } = se(a), [i, x] = g(null), [k, b] = g("excel"), [l, S] = g(!1), [E, y] = g([]), [v, h] = g([]);
  q(() => {
    y(d);
  }, [d]);
  const z = async (t) => {
    if (!t || !a) {
      h([]);
      return;
    }
    const s = await a(`/umbraco/backoffice/api/Exports/exportableProperties/${t.alias}`, {
      headers: { Accept: "application/json" }
    }).then((f) => f.json());
    for (const f of s)
      f.selected = !1;
    h(s);
  }, w = (t) => {
    const s = t.target.value, f = r.find((C) => C.alias === s) ?? null;
    x(f), z(f);
  }, $ = () => y((t) => t.map((s) => ({ ...s, selected: !0 }))), P = () => y((t) => t.map((s) => ({ ...s, selected: !1 }))), M = () => h((t) => t.map((s) => ({ ...s, selected: !0 }))), B = () => h((t) => t.map((s) => ({ ...s, selected: !1 }))), H = (t, s) => y((f) => f.map((C) => C === t ? { ...C, selected: s } : C)), J = (t, s) => h((f) => f.map((C) => C === t ? { ...C, selected: s } : C)), U = E.filter((t) => t.selected).length, D = v.filter((t) => t.selected).length, T = U > 0 || D > 0;
  return /* @__PURE__ */ p("div", { className: "n3o-data-export", children: [
    /* @__PURE__ */ o(
      ce,
      {
        contentTypes: r,
        contentType: i,
        format: k,
        includeUnpublished: l,
        processing: n,
        onContentTypeChange: w,
        onFormatChange: b,
        onIncludeUnpublishedChange: S
      }
    ),
    /* @__PURE__ */ o(
      X,
      {
        headline: "Metadata fields",
        selectedCount: U,
        items: E,
        getKey: (t) => t.id,
        getLabel: (t) => t.name,
        getChecked: (t) => t.selected,
        processing: n,
        onToggle: H,
        onSelectAll: $,
        onClear: P,
        emptyState: /* @__PURE__ */ o("p", { className: "emptyState", children: "No metadata fields are available." })
      }
    ),
    /* @__PURE__ */ o(
      X,
      {
        headline: "Properties",
        selectedCount: D,
        items: v,
        getKey: (t) => t.alias,
        getLabel: (t) => t.columnTitle,
        getChecked: (t) => t.selected,
        processing: n,
        onToggle: J,
        onSelectAll: M,
        onClear: B,
        emptyState: i ? /* @__PURE__ */ o("p", { className: "emptyState", children: "This content type has no exportable properties." }) : /* @__PURE__ */ o("p", { className: "emptyState", children: "Select a content type to see its exportable properties." })
      }
    ),
    !n && i && !T ? /* @__PURE__ */ o("p", { className: "hint", children: "Select at least one metadata field or property to export." }) : null,
    c ? /* @__PURE__ */ p("div", { className: "errorBox", children: [
      /* @__PURE__ */ o("uui-icon", { name: "icon-alert" }),
      /* @__PURE__ */ o("span", { children: c })
    ] }) : null,
    n ? /* @__PURE__ */ p("div", { className: "progress", children: [
      /* @__PURE__ */ o("uui-loader-bar", {}),
      /* @__PURE__ */ o("span", { children: u || "Preparing export…" })
    ] }) : null,
    /* @__PURE__ */ o("div", { className: "actions", children: /* @__PURE__ */ o(
      "button",
      {
        type: "button",
        className: "btn btn--primary btn--positive",
        disabled: !(!!i && T && !n && !!a),
        onClick: () => void m(
          e,
          (i == null ? void 0 : i.alias) ?? "",
          k,
          l,
          E.filter((t) => t.selected).map((t) => t.id),
          v.filter((t) => t.selected).map((t) => t.alias)
        ),
        children: n ? "Exporting…" : "Export"
      }
    ) }),
    /* @__PURE__ */ o("style", { children: ne })
  ] });
}
var de = Object.getOwnPropertyDescriptor, Q = (e) => {
  throw TypeError(e);
}, pe = (e, a, r, d) => {
  for (var n = d > 1 ? void 0 : d ? de(a, r) : a, u = e.length - 1, c; u >= 0; u--)
    (c = e[u]) && (n = c(n) || n);
  return n;
}, F = (e, a, r) => a.has(e) || Q("Cannot " + r), _ = (e, a, r) => (F(e, a, "read from private field"), r ? r.call(e) : a.get(e)), L = (e, a, r) => a.has(e) ? Q("Cannot add the same private member more than once") : a instanceof WeakSet ? a.add(e) : a.set(e, r), G = (e, a, r, d) => (F(e, a, "write to private field"), a.set(e, r), r), I = (e, a, r) => (F(e, a, "access private method"), r), O, A, j, R, W;
const ue = "n3o-data-export";
let K = class extends te(Z(HTMLElement)) {
  constructor() {
    super(), L(this, R), L(this, O), L(this, A), L(this, j, null);
    const e = this.attachShadow({ mode: "open" });
    G(this, A, document.createElement("div")), e.appendChild(_(this, A)), this.consumeContext(ee, (a) => {
      a && this.observe(
        a.unique,
        (r) => {
          r && r !== _(this, j) && (G(this, j, r), I(this, R, W).call(this));
        },
        "_observeUnique"
      );
    });
  }
  // Re-render when the shared authenticated fetch becomes available / changes (mixin hook).
  authFetchChanged(e) {
    I(this, R, W).call(this);
  }
  connectedCallback() {
    var e;
    (e = super.connectedCallback) == null || e.call(this), _(this, O) ?? G(this, O, oe(_(this, A))), I(this, R, W).call(this);
  }
  disconnectedCallback() {
    var e, a;
    (e = super.disconnectedCallback) == null || e.call(this), (a = _(this, O)) == null || a.unmount(), G(this, O, void 0);
  }
};
O = /* @__PURE__ */ new WeakMap();
A = /* @__PURE__ */ new WeakMap();
j = /* @__PURE__ */ new WeakMap();
R = /* @__PURE__ */ new WeakSet();
W = function() {
  var e;
  (e = _(this, O)) == null || e.render(
    ae(le, {
      contentKey: _(this, j),
      authFetch: this.authFetch
    })
  );
};
K = pe([
  Y(ue)
], K);
const ye = K;
export {
  K as N3oDataExportElement,
  ye as default
};
//# sourceMappingURL=data-export.js.map
