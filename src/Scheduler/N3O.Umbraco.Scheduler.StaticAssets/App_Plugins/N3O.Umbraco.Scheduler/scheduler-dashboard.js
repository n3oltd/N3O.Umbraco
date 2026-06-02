import { LitElement as i, html as m, css as c, customElement as h } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as d } from "@umbraco-cms/backoffice/element-api";
var f = Object.getOwnPropertyDescriptor, u = (l, a, n, s) => {
  for (var e = s > 1 ? void 0 : s ? f(a, n) : a, t = l.length - 1, o; t >= 0; t--)
    (o = l[t]) && (e = o(e) || e);
  return e;
};
const b = "n3o-scheduler-dashboard";
let r = class extends d(i) {
  render() {
    return m`
            <iframe
                name="hangfireIFrame"
                id="hangfire"
                title="Scheduler"
                frameborder="0"
                scrolling="yes"
                src="/umbraco/backoffice/hangfire/"
                allowfullscreen></iframe>
        `;
  }
};
r.styles = c`
        :host {
            display: block;
            width: 100%;
            height: 100%;
        }

        iframe {
            display: block;
            width: 100%;
            height: 100%;
            border: 0;
        }
    `;
r = u([
  h(b)
], r);
const v = r;
export {
  r as N3oSchedulerDashboardElement,
  v as default
};
//# sourceMappingURL=scheduler-dashboard.js.map
