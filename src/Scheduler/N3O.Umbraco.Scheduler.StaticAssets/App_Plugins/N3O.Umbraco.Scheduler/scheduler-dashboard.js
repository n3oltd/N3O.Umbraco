import { LitElement, html, css } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

const elementName = 'n3o-scheduler-dashboard';

// Scheduler dashboard: wraps the Hangfire backoffice UI in an iframe filling the dashboard.
// Ported from the legacy AngularJS view N3O.Umbraco.Scheduler.html.
class N3oSchedulerDashboardElement extends UmbElementMixin(LitElement) {
    render() {
        return html`
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

    static styles = css`
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
}

customElements.define(elementName, N3oSchedulerDashboardElement);

export default N3oSchedulerDashboardElement;
export { N3oSchedulerDashboardElement };
