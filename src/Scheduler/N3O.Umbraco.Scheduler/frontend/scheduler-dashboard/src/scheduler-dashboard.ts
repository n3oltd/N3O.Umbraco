import { css, customElement, html } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';

const elementName = 'n3o-scheduler-dashboard';

@customElement(elementName)
export class N3oSchedulerDashboardElement extends UmbLitElement {
    static styles = css`
        :host {
            display: block;
            width: 100%;
        }

        iframe {
            display: block;
            width: 100%;
            /* Fill the viewport below the backoffice header and dashboard tab bar. */
            height: calc(100dvh - 200px);
            min-height: 600px;
            border: 0;
        }
    `;

    render() {
        return html`<iframe
            name="hangfireIFrame"
            id="hangfire"
            title="Scheduler"
            src="/umbraco/backoffice/hangfire/"
            allowfullscreen></iframe>`;
    }
}

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oSchedulerDashboardElement;
    }
}

export default N3oSchedulerDashboardElement;
