import { LitElement as D, nothing as p, html as d, css as N, state as y, customElement as q } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as F } from "@umbraco-cms/backoffice/element-api";
import { UmbPropertyValueChangeEvent as V } from "@umbraco-cms/backoffice/property-editor";
var H = Object.defineProperty, j = Object.getOwnPropertyDescriptor, A = (e) => {
  throw TypeError(e);
}, g = (e, t, s, h) => {
  for (var r = h > 1 ? void 0 : h ? j(t, s) : t, I = e.length - 1, $; I >= 0; I--)
    ($ = e[I]) && (r = (h ? $(t, s, r) : $(r)) || r);
  return h && r && H(t, s, r), r;
}, U = (e, t, s) => t.has(e) || A("Cannot " + s), l = (e, t, s) => (U(e, t, "read from private field"), s ? s.call(e) : t.get(e)), f = (e, t, s) => t.has(e) ? A("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, s), b = (e, t, s, h) => (U(e, t, "write to private field"), t.set(e, s), s), a = (e, t, s) => (U(e, t, "access private method"), s), n, u, k, v, i, o, _, w, z, m, E, S, O, W, C, T, B;
const L = "n3o-uploader", x = "/App_Plugins/N3O.Umbraco.Uploader";
let M = null;
function P(e) {
  return new Promise((t, s) => {
    if (document.querySelector(`script[data-n3o-uploader="${e}"]`)) {
      t();
      return;
    }
    const r = document.createElement("script");
    r.src = e, r.async = !1, r.dataset.n3oUploader = e, r.onload = () => t(), r.onerror = () => s(new Error(`Failed to load ${e}`)), document.head.appendChild(r);
  });
}
async function Q() {
  return M || (M = (async () => {
    window.jQuery || await P("https://code.jquery.com/jquery-3.7.1.min.js"), await P(`${x}/formstone/core.js`), await P(`${x}/formstone/upload.js`);
  })()), M;
}
function R() {
  let e = "";
  const t = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
  for (let s = 0; s < 10; s++)
    e += t.charAt(Math.floor(Math.random() * t.length));
  return e;
}
let c = class extends F(D) {
  constructor() {
    super(...arguments), f(this, i), f(this, n), f(this, u), f(this, k, R()), f(this, v, !1), this._uploadInProgress = !1, this._errorMessage = null, this._progress = 0, this._mediaId = "";
  }
  get value() {
    return l(this, n);
  }
  set value(e) {
    const t = l(this, n);
    b(this, n, e), this.requestUpdate("value", t);
  }
  set config(e) {
    b(this, u, e);
  }
  updated() {
    if (l(this, v) || l(this, n))
      return;
    const e = this.renderRoot.querySelector(".upload");
    e && (b(this, v, !0), Q().then(() => {
      const t = window.jQuery;
      t(e).upload({
        action: "/umbraco/backoffice/api/uploader/upload",
        label: "Drop and drop a file, or click to select",
        maxSize: 5368709120,
        maxQueue: 1,
        postData: {
          allowedExtensions: a(this, i, o).call(this, "allowedExtensions"),
          maxFileSizeMb: a(this, i, o).call(this, "maxFileSizeMb"),
          imagesOnly: l(this, i, _),
          minImageWidth: a(this, i, o).call(this, "minImageWidth"),
          maxImageWidth: a(this, i, o).call(this, "maxImageWidth"),
          minImageHeight: a(this, i, o).call(this, "minImageHeight"),
          maxImageHeight: a(this, i, o).call(this, "maxImageHeight")
        }
      }).on("filestart.upload", () => {
        this._progress = 0, this._uploadInProgress = !0;
      }).on("fileprogress.upload", (s, h, r) => {
        this._progress = r;
      }).on("filecomplete.upload", (s, h, r) => {
        a(this, i, m).call(this, null, r);
      }).on("fileerror.upload", () => {
        a(this, i, m).call(this, "The specified file either has an invalid extensions, exceeds the maximum allowed size, or does not meet dimension constraints");
      });
    }).catch(() => {
      this._errorMessage = "Failed to load the uploader";
    }));
  }
  render() {
    return d`
            <link rel="stylesheet" href="${x}/radial-progress.css" />
            <link rel="stylesheet" href="${x}/formstone/upload.css" />
            <div class="n3o-uploader">
                <div id=${l(this, k)}>
                    ${l(this, n) ? p : a(this, i, T).call(this)}
                    ${l(this, n) && !this._uploadInProgress ? a(this, i, B).call(this) : p}
                </div>
            </div>
        `;
  }
};
n = /* @__PURE__ */ new WeakMap();
u = /* @__PURE__ */ new WeakMap();
k = /* @__PURE__ */ new WeakMap();
v = /* @__PURE__ */ new WeakMap();
i = /* @__PURE__ */ new WeakSet();
o = function(e) {
  if (l(this, u) && typeof l(this, u).getValueByAlias == "function")
    return l(this, u).getValueByAlias(e);
};
_ = function() {
  const e = a(this, i, o).call(this, "imagesOnly");
  return !(e === "0" || e === 0 || e === !1);
};
w = function(e) {
  this.value = e, this.dispatchEvent(new V());
};
z = function(e) {
  e && !confirm("Are you sure?") || (this._errorMessage = null, a(this, i, w).call(this, null));
};
m = function(e, t) {
  if (e === null) {
    let s;
    typeof t == "string" ? s = JSON.parse(t) : s = t, a(this, i, w).call(this, {
      urlPath: s.urlPath,
      mediaId: s.mediaId,
      extension: s.extension,
      sizeMb: s.sizeMb,
      filename: s.filename
    });
  } else
    this._errorMessage = e;
  this._uploadInProgress = !1;
};
E = function(e) {
  this._mediaId = e.target.value, a(this, i, S).call(this);
};
S = function() {
  !this._mediaId || this._mediaId.length !== 17 || fetch(`/umbraco/backoffice/api/uploader/media/${this._mediaId}`).then((e) => {
    if (!e.ok)
      throw new Error("not found");
    return e.json();
  }).then((e) => a(this, i, m).call(this, null, e)).catch(() => a(this, i, m).call(this, "No media found with the specified ID"));
};
O = function(e) {
  a(this, i, w).call(this, { ...l(this, n), altText: e.target.value });
};
W = function(e) {
  const t = document.createElement("input");
  document.body.appendChild(t), t.value = e, t.select(), document.execCommand("copy"), t.remove();
};
C = function() {
  if (!this._uploadInProgress)
    return p;
  const e = [];
  e.push(d`<span>-</span>`);
  for (let t = 0; t <= 100; t++)
    e.push(d`<span>${t}%</span>`);
  return d`
            <div class="radial-progress" data-progress=${this._progress}>
                <div class="circle">
                    <div class="mask full"><div class="fill"></div></div>
                    <div class="mask half">
                        <div class="fill"></div>
                        <div class="fill fix"></div>
                    </div>
                    <div class="shadow"></div>
                </div>
                <div class="inset">
                    <div class="percentage">
                        <div class="numbers">${e}</div>
                    </div>
                </div>
            </div>
        `;
};
T = function() {
  return d`
            ${a(this, i, C).call(this)}
            ${!this._errorMessage && !this._uploadInProgress ? d`
                      <div class="upload"></div>

                      <p>
                          <br />
                          Allowed file types : ${a(this, i, o).call(this, "allowedExtensions")} <br />
                          Maximum file size : ${a(this, i, o).call(this, "maxFileSizeMb")}MB
                      </p>

                      <input
                          class="textBox media-id"
                          type="text"
                          placeholder="Load media by ID"
                          .value=${this._mediaId}
                          @input=${a(this, i, E)}
                          @paste=${a(this, i, E)} />
                  ` : p}
            ${this._errorMessage ? d`
                      <p class="error">
                          Uploading of the file failed with the error:<br /><br />
                          ${this._errorMessage}
                      </p>

                      <p class="start-over">
                          <a @click=${() => a(this, i, z).call(this, !1)} class="cursor reset">Try Again</a>
                      </p>
                  ` : p}
        `;
};
B = function() {
  const e = l(this, n);
  return d`
            <a href=${e.urlPath} target="_blank">${e.filename} (${e.sizeMb}MB)</a>

            ${l(this, i, _) ? d`
                      <br />
                      <img src=${e.urlPath} style="max-width: 280px; margin: 10px; background-color: #eeeeee;" />
                      <br /><br />
                  ` : p}
            ${l(this, i, _) && a(this, i, o).call(this, "altTextRequired") ? d`
                      <p>
                          <input
                              class="textBox"
                              type="text"
                              placeholder="Alt text"
                              .value=${e.altText ?? ""}
                              @input=${a(this, i, O)} />
                      </p>
                  ` : p}

            <div class="start-over">
                <div style="float: left;">
                    <a class="cursor" @click=${() => a(this, i, W).call(this, e.mediaId)}>${e.mediaId}</a>
                    |
                    <a href=${e.urlPath} target="_blank">Download</a>
                </div>

                <div style="float: right;">
                    <a @click=${() => a(this, i, z).call(this, !0)} class="reset cursor">Delete file</a>
                </div>
            </div>
        `;
};
c.styles = N`
        .n3o-uploader {
            max-width: 500px;
        }

        .n3o-uploader .hidden {
            display: none;
        }

        .n3o-uploader .upload {
            padding: 10px;
            border: 1px dashed #666;
            border-radius: 5px;
            text-align: center;
        }

        .n3o-uploader .error {
            background: red;
            color: white;
            padding: 10px;
        }

        .n3o-uploader .textBox {
            margin-top: 5px;
            margin-bottom: 5px;
            height: 30px;
            width: 100%;
            font-size: 15px;
            font-family: Verdana;
            line-height: 30px;
            display: inline-block;
            vertical-align: middle;
        }

        .n3o-uploader .cursor {
            cursor: pointer;
            text-decoration: none;
        }

        .n3o-uploader .reset {
            color: red;
            font-size: 120%;
            font-weight: bold;
            text-decoration: none;
            text-align: right;
        }
    `;
g([
  y()
], c.prototype, "_uploadInProgress", 2);
g([
  y()
], c.prototype, "_errorMessage", 2);
g([
  y()
], c.prototype, "_progress", 2);
g([
  y()
], c.prototype, "_mediaId", 2);
c = g([
  q(L)
], c);
const X = c;
export {
  c as N3oUploaderElement,
  X as default
};
//# sourceMappingURL=uploader.js.map
