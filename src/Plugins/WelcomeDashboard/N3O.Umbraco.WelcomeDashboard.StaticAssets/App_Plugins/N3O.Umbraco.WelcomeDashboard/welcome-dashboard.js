import { LitElement as l, html as p, css as d, customElement as u } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as c } from "@umbraco-cms/backoffice/element-api";
var v = Object.getOwnPropertyDescriptor, m = (t, o, s, n) => {
  for (var e = n > 1 ? void 0 : n ? v(o, s) : o, r = t.length - 1, i; r >= 0; r--)
    (i = t[r]) && (e = i(e) || e);
  return e;
};
const _ = "n3o-welcome-dashboard";
let a = class extends c(l) {
  render() {
    return p`
            <div class="panel">
                <div class="panel__header">
                    <h3>Help &amp; Support</h3>
                </div>

                <div class="panel__content">
                    <p>
                        Please visit the N3O Support Centre to view the latest help articles, documentation and to
                        contact our support team with any queries.
                    </p>

                    <p>
                        <a href="https://support.n3o.ltd" target="_blank" rel="noopener">Visit Support Centre &rarr;</a>
                    </p>
                </div>
            </div>
        `;
  }
};
a.styles = d`
        :host {
            display: block;
            padding: var(--uui-size-layout-1);
        }

        .panel {
            background: var(--uui-color-surface);
            border: 1px solid var(--uui-color-divider-standalone);
            border-radius: var(--uui-border-radius);
        }

        .panel__header {
            padding: var(--uui-size-space-4) var(--uui-size-space-5);
            border-bottom: 1px solid var(--uui-color-divider-standalone);
        }

        .panel__header h3 {
            margin: 0;
        }

        .panel__content {
            padding: var(--uui-size-space-4) var(--uui-size-space-5);
        }

        .panel__content p {
            margin: 0 0 var(--uui-size-space-4);
        }

        .panel__content p:last-child {
            margin-bottom: 0;
        }

        a {
            color: var(--uui-color-interactive);
        }
    `;
a = m([
  u(_)
], a);
const g = a;
export {
  a as N3oWelcomeDashboardElement,
  g as default
};
//# sourceMappingURL=welcome-dashboard.js.map
