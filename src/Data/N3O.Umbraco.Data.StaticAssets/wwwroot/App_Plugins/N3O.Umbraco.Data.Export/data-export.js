import { customElement as ue } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as he } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as be } from "@umbraco-cms/backoffice/document";
import { UmbAuthFetchMixin as me } from "@n3o/backoffice-core";
import { useState as h, useEffect as ve, createElement as xe } from "react";
import { createRoot as fe } from "react-dom/client";
import { jsxs as s, jsx as a, Fragment as J } from "react/jsx-runtime";
const ge = ".n3o-data-export{display:block;padding:var(--uui-size-space-4)}.n3o-data-export uui-box{--uui-box-default-padding: var(--uui-size-space-4);margin-bottom:var(--uui-size-space-3)}.n3o-data-export .nativeSelect{width:100%;max-width:420px;box-sizing:border-box;height:var(--uui-size-11, 36px);padding:0 var(--uui-size-space-3);font:inherit;color:var(--uui-color-text);background:var(--uui-color-surface);border:1px solid var(--uui-color-border);border-radius:var(--uui-border-radius)}.n3o-data-export .nativeSelect:focus{outline:none;border-color:var(--uui-color-focus);box-shadow:0 0 0 1px var(--uui-color-focus)}.n3o-data-export .nativeSelect:disabled{opacity:.5;cursor:not-allowed}.n3o-data-export .radioGroup{display:flex;flex-direction:row;flex-wrap:wrap;gap:var(--uui-size-space-2) var(--uui-size-space-5)}.n3o-data-export .radioOption,.n3o-data-export .toggleOption,.n3o-data-export .checkOption{display:flex;align-items:center;gap:var(--uui-size-space-2);cursor:pointer}.n3o-data-export .radioOption input,.n3o-data-export .toggleOption input,.n3o-data-export .checkOption input{cursor:pointer}.n3o-data-export .selectionCount{font-size:var(--uui-type-small-size);color:var(--uui-color-text-alt)}.n3o-data-export .selectionActions{display:flex;gap:var(--uui-size-space-2);margin-bottom:var(--uui-size-space-3)}.n3o-data-export .checkboxGrid{display:grid;grid-template-columns:repeat(auto-fill,minmax(180px,1fr));gap:var(--uui-size-space-1) var(--uui-size-space-5)}.n3o-data-export .emptyState{margin:0;color:var(--uui-color-text-alt);font-style:italic}.n3o-data-export .hint{margin:0 0 var(--uui-size-space-4);color:var(--uui-color-text-alt)}.n3o-data-export .errorBox{display:flex;align-items:center;gap:var(--uui-size-space-3);margin-bottom:var(--uui-size-space-4);padding:var(--uui-size-space-4) var(--uui-size-space-5);border-radius:var(--uui-border-radius);background:var(--uui-color-danger);color:var(--uui-color-danger-contrast)}.n3o-data-export .progress{display:flex;flex-direction:column;gap:var(--uui-size-space-2);margin-bottom:var(--uui-size-space-4);color:var(--uui-color-text-alt)}.n3o-data-export .actions{margin-top:var(--uui-size-space-4)}.n3o-data-export .btn{font:inherit;font-weight:700;line-height:1;display:inline-flex;align-items:center;gap:var(--uui-size-space-2);padding:0 var(--uui-size-space-4);height:var(--uui-size-11, 36px);border:1px solid transparent;border-radius:var(--uui-border-radius);cursor:pointer;box-sizing:border-box}.n3o-data-export .btn--compact{height:var(--uui-size-9, 30px);padding:0 var(--uui-size-space-3);font-size:var(--uui-type-small-size)}.n3o-data-export .btn--secondary{background:var(--uui-color-surface);color:var(--uui-color-text);border-color:var(--uui-color-border)}.n3o-data-export .btn--secondary:hover:not(:disabled){background:var(--uui-color-surface-emphasis);border-color:var(--uui-color-border-emphasis)}.n3o-data-export .btn--primary{background:var(--uui-color-default);color:var(--uui-color-default-contrast)}.n3o-data-export .btn--primary.btn--positive{background:var(--uui-color-positive);color:var(--uui-color-positive-contrast)}.n3o-data-export .btn--primary:hover:not(:disabled){background:var(--uui-color-positive-emphasis, var(--uui-color-positive))}.n3o-data-export .btn:disabled{opacity:.5;cursor:not-allowed}";
function ye({ contentKey: t, authFetch: o }) {
  const [i, f] = h([]), [p, g] = h(null), [v, G] = h("excel"), [R, X] = h(!1), [N, z] = h([]), [S, y] = h([]), [d, M] = h(!1), [K, _] = h(""), [W, $] = h(null);
  ve(() => {
    if (!t || !o)
      return;
    let e = !0;
    return (async () => {
      const c = await Q(t), n = await o("/umbraco/backoffice/api/Exports/lookups/contentMetadata", {
        headers: { Accept: "application/json" }
      }).then((u) => u.json());
      for (const u of n)
        u.selected = u.autoSelected;
      n.sort((u, l) => u.displayOrder - l.displayOrder), e && (f(c), z(n));
    })(), () => {
      e = !1;
    };
  }, [t, o]);
  const Q = async (e) => await (await o(`/umbraco/api/ContentTypes/${e}/relations?type=descendant`, {
    headers: { Accept: "application/json" }
  })).json(), Y = async (e) => {
    if (!e) {
      y([]);
      return;
    }
    const r = await o(`/umbraco/backoffice/api/Exports/exportableProperties/${e.alias}`, {
      headers: { Accept: "application/json" }
    }).then((c) => c.json());
    for (const c of r)
      c.selected = !1;
    y(r);
  }, Z = (e) => {
    const r = e.target.value, c = i.find((n) => n.alias === r) ?? null;
    g(c), Y(c);
  }, k = (e) => {
    M(!1), _(""), $(e);
  }, ee = (e) => {
    const r = async (c, n) => {
      const u = await o(`/umbraco/backoffice/api/Exports/export/${e}/progress`, {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "GET"
      }), l = await u.json();
      if (u.status !== 200) {
        k(String(l)), n(l);
        return;
      }
      l.isComplete === !0 ? c(l) : (_(l.text), setTimeout(() => void r(c, n), 2500));
    };
    return new Promise(r);
  }, te = async () => {
    if (M(!0), _(""), $(null), !p) {
      k("Please select a content type");
      return;
    }
    const e = N.filter((l) => l.selected).map((l) => l.id), r = S.filter((l) => l.selected).map((l) => l.alias);
    if (!r.length && !e.length) {
      k("At least one property or metadata field must be selected");
      return;
    }
    const c = {
      format: v,
      includeUnpublished: R,
      metadata: e,
      properties: r
    }, n = await o(
      `/umbraco/backoffice/api/Exports/export/${t}/${p.alias}`,
      {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "POST",
        body: JSON.stringify(c)
      }
    ), u = await n.json();
    if (n.status !== 200) {
      k(String(u));
      return;
    }
    ee(u.id).then(async (l) => {
      var q, H;
      const O = await o(`/umbraco/backoffice/api/Exports/export/${l.id}/file`, {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "GET"
      });
      if (O.status !== 200) {
        k(String(await O.json()));
        return;
      }
      const le = await O.blob(), de = ((q = ((O.headers.get("Content-Disposition") ?? "").split(";")[1] ?? "").split("=")[1]) == null ? void 0 : q.replaceAll('"', "")) ?? "export", pe = new Blob([le]), F = window.URL.createObjectURL(pe), x = document.createElement("a");
      x.href = F, x.setAttribute("download", de), document.body.appendChild(x), x.click(), (H = x.parentNode) == null || H.removeChild(x), window.URL.revokeObjectURL(F), M(!1), _("");
    }).catch(() => {
    });
  }, ae = () => z((e) => e.map((r) => ({ ...r, selected: !0 }))), oe = () => z((e) => e.map((r) => ({ ...r, selected: !1 }))), re = () => y((e) => e.map((r) => ({ ...r, selected: !0 }))), ie = () => y((e) => e.map((r) => ({ ...r, selected: !1 }))), ne = (e, r) => z((c) => c.map((n) => n === e ? { ...n, selected: r } : n)), se = (e, r) => y((c) => c.map((n) => n === e ? { ...n, selected: r } : n)), B = N.filter((e) => e.selected).length, L = S.filter((e) => e.selected).length, I = B > 0 || L > 0, ce = !!p && I && !d;
  return /* @__PURE__ */ s("div", { className: "n3o-data-export", children: [
    /* @__PURE__ */ s("uui-box", { headline: "Export options", children: [
      /* @__PURE__ */ a(
        "umb-property-layout",
        {
          label: "Content type",
          description: "The descendant type to export. Properties available for export depend on this.",
          mandatory: !0,
          children: /* @__PURE__ */ a("div", { slot: "editor", children: /* @__PURE__ */ s(
            "select",
            {
              className: "nativeSelect",
              value: (p == null ? void 0 : p.alias) ?? "",
              onChange: Z,
              disabled: d || i.length === 0,
              children: [
                /* @__PURE__ */ a("option", { value: "", disabled: !0, children: "Select a content type…" }),
                i.map((e) => /* @__PURE__ */ a("option", { value: e.alias, children: e.name }, e.alias))
              ]
            }
          ) })
        }
      ),
      /* @__PURE__ */ a(
        "umb-property-layout",
        {
          label: "File format",
          description: "Choose the file format for the exported data.",
          mandatory: !0,
          children: /* @__PURE__ */ a("div", { slot: "editor", children: /* @__PURE__ */ s("div", { className: "radioGroup", children: [
            /* @__PURE__ */ s("label", { className: "radioOption", children: [
              /* @__PURE__ */ a(
                "input",
                {
                  type: "radio",
                  name: "format",
                  value: "excel",
                  checked: v === "excel",
                  onChange: (e) => G(e.target.value),
                  disabled: d
                }
              ),
              /* @__PURE__ */ a("span", { children: "Excel (.xlsx)" })
            ] }),
            /* @__PURE__ */ s("label", { className: "radioOption", children: [
              /* @__PURE__ */ a(
                "input",
                {
                  type: "radio",
                  name: "format",
                  value: "csv",
                  checked: v === "csv",
                  onChange: (e) => G(e.target.value),
                  disabled: d
                }
              ),
              /* @__PURE__ */ a("span", { children: "CSV (.csv)" })
            ] })
          ] }) })
        }
      ),
      /* @__PURE__ */ a(
        "umb-property-layout",
        {
          label: "Include unpublished",
          description: "When enabled, unpublished content is included in the export.",
          children: /* @__PURE__ */ a("div", { slot: "editor", children: /* @__PURE__ */ s("label", { className: "toggleOption", children: [
            /* @__PURE__ */ a(
              "input",
              {
                type: "checkbox",
                checked: R,
                onChange: (e) => X(e.target.checked),
                disabled: d
              }
            ),
            /* @__PURE__ */ a("span", { children: "Include unpublished content" })
          ] }) })
        }
      )
    ] }),
    /* @__PURE__ */ s("uui-box", { headline: "Metadata fields", children: [
      /* @__PURE__ */ s("div", { slot: "header-actions", className: "selectionCount", children: [
        B,
        " selected"
      ] }),
      N.length === 0 ? /* @__PURE__ */ a("p", { className: "emptyState", children: "No metadata fields are available." }) : /* @__PURE__ */ s(J, { children: [
        /* @__PURE__ */ s("div", { className: "selectionActions", children: [
          /* @__PURE__ */ a(
            "button",
            {
              type: "button",
              className: "btn btn--secondary btn--compact",
              disabled: d,
              onClick: ae,
              children: "Select all"
            }
          ),
          /* @__PURE__ */ a(
            "button",
            {
              type: "button",
              className: "btn btn--secondary btn--compact",
              disabled: d,
              onClick: oe,
              children: "Clear"
            }
          )
        ] }),
        /* @__PURE__ */ a("div", { className: "checkboxGrid", children: N.map((e) => /* @__PURE__ */ s("label", { className: "checkOption", children: [
          /* @__PURE__ */ a(
            "input",
            {
              type: "checkbox",
              checked: !!e.selected,
              onChange: (r) => ne(e, r.target.checked),
              disabled: d
            }
          ),
          /* @__PURE__ */ a("span", { children: e.name })
        ] }, e.id)) })
      ] })
    ] }),
    /* @__PURE__ */ s("uui-box", { headline: "Properties", children: [
      /* @__PURE__ */ s("div", { slot: "header-actions", className: "selectionCount", children: [
        L,
        " selected"
      ] }),
      p ? S.length === 0 ? /* @__PURE__ */ a("p", { className: "emptyState", children: "This content type has no exportable properties." }) : /* @__PURE__ */ s(J, { children: [
        /* @__PURE__ */ s("div", { className: "selectionActions", children: [
          /* @__PURE__ */ a(
            "button",
            {
              type: "button",
              className: "btn btn--secondary btn--compact",
              disabled: d,
              onClick: re,
              children: "Select all"
            }
          ),
          /* @__PURE__ */ a(
            "button",
            {
              type: "button",
              className: "btn btn--secondary btn--compact",
              disabled: d,
              onClick: ie,
              children: "Clear"
            }
          )
        ] }),
        /* @__PURE__ */ a("div", { className: "checkboxGrid", children: S.map((e) => /* @__PURE__ */ s("label", { className: "checkOption", children: [
          /* @__PURE__ */ a(
            "input",
            {
              type: "checkbox",
              checked: !!e.selected,
              onChange: (r) => se(e, r.target.checked),
              disabled: d
            }
          ),
          /* @__PURE__ */ a("span", { children: e.columnTitle })
        ] }, e.alias)) })
      ] }) : /* @__PURE__ */ a("p", { className: "emptyState", children: "Select a content type to see its exportable properties." })
    ] }),
    !d && p && !I ? /* @__PURE__ */ a("p", { className: "hint", children: "Select at least one metadata field or property to export." }) : null,
    W ? /* @__PURE__ */ s("div", { className: "errorBox", children: [
      /* @__PURE__ */ a("uui-icon", { name: "icon-alert" }),
      /* @__PURE__ */ a("span", { children: W })
    ] }) : null,
    d ? /* @__PURE__ */ s("div", { className: "progress", children: [
      /* @__PURE__ */ a("uui-loader-bar", {}),
      /* @__PURE__ */ a("span", { children: K || "Preparing export…" })
    ] }) : null,
    /* @__PURE__ */ a("div", { className: "actions", children: /* @__PURE__ */ a(
      "button",
      {
        type: "button",
        className: "btn btn--primary btn--positive",
        disabled: !ce,
        onClick: () => void te(),
        children: d ? "Exporting…" : "Export"
      }
    ) }),
    /* @__PURE__ */ a("style", { children: ge })
  ] });
}
var ke = Object.getOwnPropertyDescriptor, V = (t) => {
  throw TypeError(t);
}, Ce = (t, o, i, f) => {
  for (var p = f > 1 ? void 0 : f ? ke(o, i) : o, g = t.length - 1, v; g >= 0; g--)
    (v = t[g]) && (p = v(p) || p);
  return p;
}, D = (t, o, i) => o.has(t) || V("Cannot " + i), b = (t, o, i) => (D(t, o, "read from private field"), i ? i.call(t) : o.get(t)), P = (t, o, i) => o.has(t) ? V("Cannot add the same private member more than once") : o instanceof WeakSet ? o.add(t) : o.set(t, i), T = (t, o, i, f) => (D(t, o, "write to private field"), o.set(t, i), i), j = (t, o, i) => (D(t, o, "access private method"), i), m, C, E, w, A;
const we = "n3o-data-export";
let U = class extends me(he(HTMLElement)) {
  constructor() {
    super(), P(this, w), P(this, m), P(this, C), P(this, E, null);
    const t = this.attachShadow({ mode: "open" });
    T(this, C, document.createElement("div")), t.appendChild(b(this, C)), this.consumeContext(be, (o) => {
      o && this.observe(
        o.unique,
        (i) => {
          i && i !== b(this, E) && (T(this, E, i), j(this, w, A).call(this));
        },
        "_observeUnique"
      );
    });
  }
  // Re-render when the shared authenticated fetch becomes available / changes (mixin hook).
  authFetchChanged(t) {
    j(this, w, A).call(this);
  }
  connectedCallback() {
    var t;
    (t = super.connectedCallback) == null || t.call(this), b(this, m) ?? T(this, m, fe(b(this, C))), j(this, w, A).call(this);
  }
  disconnectedCallback() {
    var t, o;
    (t = super.disconnectedCallback) == null || t.call(this), (o = b(this, m)) == null || o.unmount(), T(this, m, void 0);
  }
};
m = /* @__PURE__ */ new WeakMap();
C = /* @__PURE__ */ new WeakMap();
E = /* @__PURE__ */ new WeakMap();
w = /* @__PURE__ */ new WeakSet();
A = function() {
  var t;
  (t = b(this, m)) == null || t.render(
    xe(ye, {
      contentKey: b(this, E),
      authFetch: this.authFetch
    })
  );
};
U = Ce([
  ue(we)
], U);
const Me = U;
export {
  U as N3oDataExportElement,
  Me as default
};
//# sourceMappingURL=data-export.js.map
