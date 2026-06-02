import { LitElement, html, css } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

const elementName = 'n3o-welcome-dashboard';

// Static welcome dashboard. Ports the original AngularJS dashboard view, which had an empty
// controller and simply rendered a help/support panel. No backend endpoints are involved.
class N3oWelcomeDashboardElement extends UmbElementMixin(LitElement) {
    render() {
        return html`
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

    static styles = css`
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
}

customElements.define(elementName, N3oWelcomeDashboardElement);

export default N3oWelcomeDashboardElement;
export { N3oWelcomeDashboardElement };
