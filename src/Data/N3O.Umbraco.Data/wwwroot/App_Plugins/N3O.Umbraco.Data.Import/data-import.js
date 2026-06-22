import { customElement as de } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as pe } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as ue } from "@umbraco-cms/backoffice/document";
import { UmbAuthFetchMixin as me } from "@n3oltd/backoffice-core";
import { useState as v, useEffect as he, useRef as V, createElement as be } from "react";
import { createRoot as ve } from "react-dom/client";
import { jsxs as n, Fragment as Y, jsx as t } from "react/jsx-runtime";
function fe(e, o) {
  const [a, d] = v([]), [p, u] = v([]), [m, f] = v(null);
  return he(() => {
    if (!e || !o)
      return;
    let g = !0;
    return (async () => {
      const h = await (await o(`/umbraco/backoffice/api/ContentTypes/${e}/relations?type=child`, {
        headers: { Accept: "application/json" }
      })).json(), w = await (await o("/umbraco/backoffice/api/Imports/lookups/datePatterns", {
        headers: { Accept: "application/json" }
      })).json();
      g && (d(h), u(w), f(w[0] ?? null));
    })(), () => {
      g = !1;
    };
  }, [e, o]), { contentTypes: a, datePatterns: p, datePattern: m, setDatePattern: f };
}
function ye({
  processing: e,
  contentTypes: o,
  contentType: a,
  datePatterns: d,
  datePattern: p,
  moveUpdatedContentToCurrentLocation: u,
  importableProperties: m,
  selectedPropertyCount: f,
  csvFileRef: g,
  zipFileRef: T,
  onContentTypeChange: S,
  onDatePatternChange: h,
  onMoveUpdatedChange: E,
  onPropertyToggle: w,
  onSelectAllProperties: P,
  onClearSelectedProperties: D,
  onGetTemplate: x,
  onImport: M
}) {
  return /* @__PURE__ */ n(Y, { children: [
    /* @__PURE__ */ n("uui-box", { headline: "1. Choose what to import", children: [
      /* @__PURE__ */ t(
        "umb-property-layout",
        {
          label: "Content type",
          description: "The child type that rows in your CSV will be imported as.",
          mandatory: !0,
          children: /* @__PURE__ */ t("div", { slot: "editor", children: /* @__PURE__ */ n(
            "select",
            {
              className: "nativeSelect",
              value: (a == null ? void 0 : a.alias) ?? "",
              onChange: S,
              disabled: e || o.length === 0,
              children: [
                /* @__PURE__ */ t("option", { value: "", disabled: !0, children: "Select a content type…" }),
                o.map((l) => /* @__PURE__ */ t("option", { value: l.alias, children: l.name }, l.alias))
              ]
            }
          ) })
        }
      ),
      /* @__PURE__ */ t(
        "umb-property-layout",
        {
          label: "Date pattern",
          description: "How dates in your CSV are formatted, so they can be parsed correctly.",
          mandatory: !0,
          children: /* @__PURE__ */ t("div", { slot: "editor", children: /* @__PURE__ */ t(
            "select",
            {
              className: "nativeSelect",
              value: (p == null ? void 0 : p.id) ?? "",
              onChange: h,
              disabled: e || d.length === 0,
              children: d.map((l) => /* @__PURE__ */ t("option", { value: l.id, children: l.name }, l.id))
            }
          ) })
        }
      ),
      /* @__PURE__ */ t(
        "umb-property-layout",
        {
          label: "Move updated content",
          description: "When enabled, existing content that is updated will be moved beneath the current item.",
          children: /* @__PURE__ */ t("div", { slot: "editor", children: /* @__PURE__ */ n("label", { className: "toggleOption", children: [
            /* @__PURE__ */ t(
              "input",
              {
                type: "checkbox",
                checked: u,
                onChange: (l) => E(l.target.checked),
                disabled: e
              }
            ),
            /* @__PURE__ */ t("span", { children: "Move updated content to the current location" })
          ] }) })
        }
      )
    ] }),
    /* @__PURE__ */ n("uui-box", { headline: "2. Select properties", children: [
      /* @__PURE__ */ n("div", { slot: "header-actions", className: "selectionCount", children: [
        f,
        " selected"
      ] }),
      a ? m.length === 0 ? /* @__PURE__ */ t("p", { className: "emptyState", children: "This content type has no importable properties." }) : /* @__PURE__ */ n(Y, { children: [
        /* @__PURE__ */ n("div", { className: "selectionActions", children: [
          /* @__PURE__ */ t(
            "button",
            {
              type: "button",
              className: "btn btn--secondary btn--compact",
              disabled: e,
              onClick: P,
              children: "Select all"
            }
          ),
          /* @__PURE__ */ t(
            "button",
            {
              type: "button",
              className: "btn btn--secondary btn--compact",
              disabled: e,
              onClick: D,
              children: "Clear"
            }
          )
        ] }),
        /* @__PURE__ */ t("div", { className: "checkboxGrid", children: m.map((l) => /* @__PURE__ */ n("label", { className: "checkOption", children: [
          /* @__PURE__ */ t(
            "input",
            {
              type: "checkbox",
              checked: !!l.selected,
              onChange: (z) => w(l, z.target.checked),
              disabled: e
            }
          ),
          /* @__PURE__ */ t("span", { children: l.columnTitle })
        ] }, l.alias)) })
      ] }) : /* @__PURE__ */ t("p", { className: "emptyState", children: "Select a content type above to choose which properties to import." })
    ] }),
    /* @__PURE__ */ n("uui-box", { headline: "3. Download template", children: [
      /* @__PURE__ */ t("p", { className: "boxHint", children: "Download a CSV template containing a column for each selected property, then fill it in with your data." }),
      /* @__PURE__ */ n(
        "button",
        {
          type: "button",
          className: "btn btn--secondary",
          disabled: !a || f === 0 || e,
          onClick: x,
          children: [
            /* @__PURE__ */ t("uui-icon", { name: "icon-download-alt" }),
            "Download template"
          ]
        }
      )
    ] }),
    /* @__PURE__ */ n("uui-box", { headline: "4. Upload & queue", children: [
      /* @__PURE__ */ t(
        "umb-property-layout",
        {
          label: "CSV file",
          description: "The completed CSV file containing the rows to import.",
          mandatory: !0,
          children: /* @__PURE__ */ t("div", { slot: "editor", children: /* @__PURE__ */ t("input", { type: "file", id: "csvFile", accept: ".csv", ref: g, disabled: e }) })
        }
      ),
      /* @__PURE__ */ t(
        "umb-property-layout",
        {
          label: "ZIP assets file",
          description: "Optional. A ZIP archive of media/assets referenced by the CSV.",
          children: /* @__PURE__ */ t("div", { slot: "editor", children: /* @__PURE__ */ t("input", { type: "file", id: "zipFile", accept: ".zip", ref: T, disabled: e }) })
        }
      ),
      e ? /* @__PURE__ */ n("div", { className: "progress", children: [
        /* @__PURE__ */ t("uui-loader-bar", {}),
        /* @__PURE__ */ t("span", { children: "Queueing import…" })
      ] }) : null,
      /* @__PURE__ */ t("div", { className: "actions", children: /* @__PURE__ */ t(
        "button",
        {
          type: "button",
          className: "btn btn--primary btn--positive",
          disabled: !a || e,
          onClick: M,
          children: e ? "Importing…" : "Import"
        }
      ) })
    ] })
  ] });
}
function ge({ onStartOver: e }) {
  return /* @__PURE__ */ n("uui-box", { headline: "Import queued", children: [
    /* @__PURE__ */ n("div", { className: "statusBox statusBox--positive", children: [
      /* @__PURE__ */ t("uui-icon", { name: "icon-check" }),
      /* @__PURE__ */ t("span", { children: "Your CSV file has been queued and will be processed shortly." })
    ] }),
    /* @__PURE__ */ n("div", { className: "actions", children: [
      /* @__PURE__ */ t("a", { className: "btn btn--primary", href: "/umbraco/section/content/dashboard/imports", children: "View import queue" }),
      /* @__PURE__ */ t("button", { type: "button", className: "btn btn--secondary", onClick: e, children: "Import another file" })
    ] })
  ] });
}
function we({ errorMessages: e, onStartOver: o }) {
  return /* @__PURE__ */ n("uui-box", { headline: "Import failed", children: [
    /* @__PURE__ */ n("div", { className: "statusBox statusBox--danger", children: [
      /* @__PURE__ */ t("uui-icon", { name: "icon-alert" }),
      /* @__PURE__ */ t("div", { children: e && e.length > 0 ? /* @__PURE__ */ t("ul", { className: "errorList", children: e.map((a) => /* @__PURE__ */ t("li", { children: a }, a)) }) : /* @__PURE__ */ t("span", { children: "Something went wrong while queueing the import." }) })
    ] }),
    /* @__PURE__ */ t("div", { className: "actions", children: /* @__PURE__ */ t("button", { type: "button", className: "btn btn--secondary", onClick: o, children: "Start over" }) })
  ] });
}
const xe = ".n3o-data-import{display:block;padding:var(--uui-size-space-4)}.n3o-data-import uui-box{--uui-box-default-padding: var(--uui-size-space-4);margin-bottom:var(--uui-size-space-3)}.n3o-data-import .nativeSelect{width:100%;max-width:420px;box-sizing:border-box;height:var(--uui-size-11, 36px);padding:0 var(--uui-size-space-3);font:inherit;color:var(--uui-color-text);background:var(--uui-color-surface);border:1px solid var(--uui-color-border);border-radius:var(--uui-border-radius)}.n3o-data-import .nativeSelect:focus{outline:none;border-color:var(--uui-color-focus);box-shadow:0 0 0 1px var(--uui-color-focus)}.n3o-data-import .nativeSelect:disabled{opacity:.5;cursor:not-allowed}.n3o-data-import input[type=file]{font:inherit}.n3o-data-import .toggleOption,.n3o-data-import .checkOption{display:flex;align-items:center;gap:var(--uui-size-space-2);cursor:pointer}.n3o-data-import .toggleOption input,.n3o-data-import .checkOption input{cursor:pointer}.n3o-data-import .selectionCount{font-size:var(--uui-type-small-size);color:var(--uui-color-text-alt)}.n3o-data-import .selectionActions{display:flex;gap:var(--uui-size-space-2);margin-bottom:var(--uui-size-space-3)}.n3o-data-import .checkboxGrid{display:grid;grid-template-columns:repeat(auto-fill,minmax(180px,1fr));gap:var(--uui-size-space-1) var(--uui-size-space-5)}.n3o-data-import .emptyState{margin:0;color:var(--uui-color-text-alt);font-style:italic}.n3o-data-import .boxHint{margin:0 0 var(--uui-size-space-4);color:var(--uui-color-text-alt)}.n3o-data-import .progress{display:flex;flex-direction:column;gap:var(--uui-size-space-2);margin:var(--uui-size-space-4) 0;color:var(--uui-color-text-alt)}.n3o-data-import .statusBox{display:flex;align-items:flex-start;gap:var(--uui-size-space-3);padding:var(--uui-size-space-4) var(--uui-size-space-5);border-radius:var(--uui-border-radius);margin-bottom:var(--uui-size-space-4)}.n3o-data-import .statusBox--positive{background:var(--uui-color-positive);color:var(--uui-color-positive-contrast)}.n3o-data-import .statusBox--danger{background:var(--uui-color-danger);color:var(--uui-color-danger-contrast)}.n3o-data-import .errorList{margin:0;padding-left:var(--uui-size-space-4)}.n3o-data-import .actions{display:flex;gap:var(--uui-size-space-3);align-items:center;margin-top:var(--uui-size-space-4)}.n3o-data-import .btn{font:inherit;font-weight:700;line-height:1;display:inline-flex;align-items:center;gap:var(--uui-size-space-2);padding:0 var(--uui-size-space-4);height:var(--uui-size-11, 36px);border:1px solid transparent;border-radius:var(--uui-border-radius);cursor:pointer;box-sizing:border-box;text-decoration:none}.n3o-data-import .btn--compact{height:var(--uui-size-9, 30px);padding:0 var(--uui-size-space-3);font-size:var(--uui-type-small-size)}.n3o-data-import .btn--secondary{background:var(--uui-color-surface);color:var(--uui-color-text);border-color:var(--uui-color-border)}.n3o-data-import .btn--secondary:hover:not(:disabled){background:var(--uui-color-surface-emphasis);border-color:var(--uui-color-border-emphasis)}.n3o-data-import .btn--primary{background:var(--uui-color-default);color:var(--uui-color-default-contrast)}.n3o-data-import .btn--primary.btn--positive{background:var(--uui-color-positive);color:var(--uui-color-positive-contrast)}.n3o-data-import .btn--primary:hover:not(:disabled){background:var(--uui-color-positive-emphasis, var(--uui-color-positive))}.n3o-data-import .btn:disabled{opacity:.5;cursor:not-allowed}";
function Ce({ contentKey: e, authFetch: o }) {
  const [a, d] = v("form"), [p, u] = v(!1), [m, f] = v(null), [g, T] = v(!1), [S, h] = v([]), [E, w] = v(null), P = V(null), D = V(null), x = V(null), { contentTypes: M, datePatterns: l, datePattern: z, setDatePattern: ee } = fe(e, o), te = async (r) => {
    if (!r) {
      h([]);
      return;
    }
    const s = await (await o(`/umbraco/backoffice/api/Imports/importableProperties/${r.alias}`, {
      headers: { Accept: "application/json" }
    })).json();
    for (const c of s)
      c.selected = !1;
    h(s);
  }, H = () => {
    var r;
    (r = x.current) == null || r.abort(), x.current = null;
  }, Z = () => {
    H(), u(!1), f(null), w(null), h([]), d("form");
  }, oe = (r) => {
    const i = r.target.value, s = M.find((c) => c.alias === i) ?? null;
    f(s), te(s);
  }, ae = (r) => {
    const i = r.target.value;
    ee(l.find((s) => s.id === i) ?? null);
  }, re = (r, i) => {
    h((s) => s.map((c) => c === r ? { ...c, selected: i } : c));
  }, ie = () => h((r) => r.map((i) => ({ ...i, selected: !0 }))), ne = () => h((r) => r.map((i) => ({ ...i, selected: !1 }))), _ = (r) => {
    const i = Array.isArray(r) ? r : [r];
    u(!1), w(i), d("error");
  }, se = async () => {
    var Q, X;
    const r = S.filter((F) => F.selected).map((F) => F.alias);
    if (!r.length) {
      _("At least one property must be selected");
      return;
    }
    const i = { properties: r }, s = await o(`/umbraco/backoffice/api/Imports/template/${m.alias}`, {
      method: "POST",
      headers: {
        Accept: "*/*",
        "Content-Type": "application/json"
      },
      body: JSON.stringify(i)
    }), c = await s.blob(), B = ((Q = ((s.headers.get("Content-Disposition") ?? "").split(";")[1] ?? "").split("=")[1]) == null ? void 0 : Q.replaceAll('"', "")) ?? "template.csv", q = new Blob([c]), R = window.URL.createObjectURL(q), b = document.createElement("a");
    b.href = R, b.setAttribute("download", B), document.body.appendChild(b), b.dispatchEvent(new MouseEvent("click", { bubbles: !1, cancelable: !1 })), (X = b.parentNode) == null || X.removeChild(b), window.URL.revokeObjectURL(R);
  }, J = async (r, i) => {
    if (!r.files || r.files.length === 0)
      return null;
    const s = new FormData();
    return s.append("file", r.files[0]), await (await o("/umbraco/api/Storage/tempUpload", {
      method: "POST",
      body: s,
      signal: i
    })).json();
  }, le = async () => {
    var s, c;
    H();
    const r = new AbortController();
    x.current = r;
    const { signal: i } = r;
    u(!0);
    try {
      const y = P.current, N = D.current;
      if (!y || !y.value || ((s = y.value.split(".")[1]) == null ? void 0 : s.toLowerCase()) !== "csv") {
        _("A valid CSV file must be specified");
        return;
      }
      if (N && N.value && ((c = N.value.split(".")[1]) == null ? void 0 : c.toLowerCase()) !== "zip") {
        _("The selected file is not a valid ZIP file");
        return;
      }
      const B = await J(y, i);
      if (i.aborted) return;
      const q = N ? await J(N, i) : null;
      if (i.aborted) return;
      const R = {
        datePattern: z == null ? void 0 : z.id,
        moveUpdatedContentToCurrentLocation: g,
        csvFile: B,
        zipFile: q
      }, b = await o(
        `/umbraco/backoffice/api/Imports/queue/${e}/${m.alias}`,
        {
          method: "POST",
          headers: {
            Accept: "*/*",
            "Content-Type": "application/json"
          },
          body: JSON.stringify(R),
          signal: i
        }
      );
      if (i.aborted) return;
      b.status === 200 ? (d("success"), u(!1)) : _(await b.json());
    } catch (y) {
      if (y instanceof DOMException && y.name === "AbortError") return;
      _("An unexpected error occurred while queueing the import.");
    } finally {
      x.current === r && (x.current = null);
    }
  }, ce = S.filter((r) => r.selected).length;
  return /* @__PURE__ */ n("div", { className: "n3o-data-import", children: [
    a === "success" ? /* @__PURE__ */ t(ge, { onStartOver: Z }) : a === "error" ? /* @__PURE__ */ t(we, { errorMessages: E, onStartOver: Z }) : /* @__PURE__ */ t(
      ye,
      {
        processing: p,
        contentTypes: M,
        contentType: m,
        datePatterns: l,
        datePattern: z,
        moveUpdatedContentToCurrentLocation: g,
        importableProperties: S,
        selectedPropertyCount: ce,
        csvFileRef: P,
        zipFileRef: D,
        onContentTypeChange: oe,
        onDatePatternChange: ae,
        onMoveUpdatedChange: T,
        onPropertyToggle: re,
        onSelectAllProperties: ie,
        onClearSelectedProperties: ne,
        onGetTemplate: () => void se(),
        onImport: () => void le()
      }
    ),
    /* @__PURE__ */ t("style", { children: xe })
  ] });
}
var ke = Object.getOwnPropertyDescriptor, K = (e) => {
  throw TypeError(e);
}, Se = (e, o, a, d) => {
  for (var p = d > 1 ? void 0 : d ? ke(o, a) : o, u = e.length - 1, m; u >= 0; u--)
    (m = e[u]) && (p = m(p) || p);
  return p;
}, G = (e, o, a) => o.has(e) || K("Cannot " + a), C = (e, o, a) => (G(e, o, "read from private field"), a ? a.call(e) : o.get(e)), U = (e, o, a) => o.has(e) ? K("Cannot add the same private member more than once") : o instanceof WeakSet ? o.add(e) : o.set(e, a), L = (e, o, a, d) => (G(e, o, "write to private field"), o.set(e, a), a), W = (e, o, a) => (G(e, o, "access private method"), a), k, A, O, I, j;
const ze = "n3o-data-import";
let $ = class extends me(pe(HTMLElement)) {
  constructor() {
    super(), U(this, I), U(this, k), U(this, A), U(this, O, null);
    const e = this.attachShadow({ mode: "open" });
    L(this, A, document.createElement("div")), e.appendChild(C(this, A)), this.consumeContext(ue, (o) => {
      o && this.observe(
        o.unique,
        (a) => {
          a && a !== C(this, O) && (L(this, O, a), W(this, I, j).call(this));
        },
        "_observeUnique"
      );
    });
  }
  // Re-render when the shared authenticated fetch becomes available / changes (mixin hook).
  authFetchChanged(e) {
    W(this, I, j).call(this);
  }
  connectedCallback() {
    var e;
    (e = super.connectedCallback) == null || e.call(this), C(this, k) ?? L(this, k, ve(C(this, A))), W(this, I, j).call(this);
  }
  disconnectedCallback() {
    var e, o;
    (e = super.disconnectedCallback) == null || e.call(this), (o = C(this, k)) == null || o.unmount(), L(this, k, void 0);
  }
};
k = /* @__PURE__ */ new WeakMap();
A = /* @__PURE__ */ new WeakMap();
O = /* @__PURE__ */ new WeakMap();
I = /* @__PURE__ */ new WeakSet();
j = function() {
  var e;
  (e = C(this, k)) == null || e.render(
    be(Ce, {
      contentKey: C(this, O),
      authFetch: this.authFetch
    })
  );
};
$ = Se([
  de(ze)
], $);
const Pe = $;
export {
  $ as N3oDataImportElement,
  Pe as default
};
//# sourceMappingURL=data-import.js.map
