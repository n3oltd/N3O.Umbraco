import { LitElement, html, css } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';

const elementName = 'n3o-platforms-preview';

// Workspace view (was the "platformsPreview" content app) that renders a live preview of the current
// document. It builds a request from the current variant's property values, posts it to the platforms
// back office preview endpoint, and renders the returned HTML in an iframe loading the tenant's
// platforms.js. The preview is refreshed every 10 seconds; unchanged responses (same eTag) are skipped.
class N3oPlatformsPreviewElement extends UmbElementMixin(LitElement) {
    #workspaceContext;
    #previousETag = null;
    #intervalId;

    constructor() {
        super();

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            this.#workspaceContext = context;
        });
    }

    connectedCallback() {
        super.connectedCallback();

        this.#loadPreview();

        this.#intervalId = window.setInterval(() => {
            this.#loadPreview();
        }, 10000);
    }

    disconnectedCallback() {
        super.disconnectedCallback();

        if (this.#intervalId) {
            window.clearInterval(this.#intervalId);
            this.#intervalId = undefined;
        }
    }

    async #loadPreview() {
        if (!this.#workspaceContext) {
            return;
        }

        const content = this.#workspaceContext.getData();

        if (!content) {
            return;
        }

        const contentTypeAlias = content.contentType?.alias ?? content.contentTypeAlias;
        const variant = (content.variants ?? []).find((v) => v.culture == null || v.segment == null) ?? content.variants?.[0];

        const apiReq = this.#getApiReq(content.values ?? [], contentTypeAlias);

        apiReq['name'] = variant?.name;
        apiReq['key'] = content.unique ?? content.key;
        apiReq['parentId'] = content.parent?.unique ?? content.parentId;

        const subscriptionCodeRes = await fetch('/umbraco/backoffice/api/cloudBackOffice/subscription/code');
        const subscriptionCode = await subscriptionCodeRes.json();

        const apiRes = await fetch(`/umbraco/backoffice/api/platformsBackOffice/previewHtml/${contentTypeAlias}`, {
            method: 'POST',
            headers: {
                accept: 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(apiReq),
        });

        const res = await apiRes.json();

        if (res.eTag === this.#previousETag) {
            return;
        }

        this.#previousETag = res.eTag;

        const container = this.renderRoot.getElementById('platformsPreviewContainer');

        if (!container) {
            return;
        }

        container.innerHTML = '';

        const iframe = document.createElement('iframe');
        iframe.style.width = '100%';
        iframe.style.aspectRatio = '16 / 9';
        iframe.style.border = '0';
        iframe.style.transform = 'scale(0.9)';
        iframe.style.transformOrigin = '0 0';
        iframe.style.display = 'none';

        container.appendChild(iframe);

        const doc = iframe.contentWindow.document;
        doc.open();
        doc.write(res.html);
        doc.close();

        const script = doc.createElement('script');
        script.src = `https://cdn.n3o.cloud/connect-${subscriptionCode}/platforms-js/platforms.js`;
        script.type = 'module';

        doc.body.appendChild(script);

        window.setInterval(() => {
            iframe.style.display = 'block';
            container.style.display = 'block';
        }, 2000);
    }

    #getApiReq(values, contentTypeAlias) {
        const req = {};

        values.forEach((property) => {
            req[property.alias] = property.value;
        });

        req['contentTypeAlias'] = contentTypeAlias;

        return req;
    }

    render() {
        return html`<div id="platformsPreviewContainer" style="display: none;"></div>`;
    }

    static styles = css`
        :host {
            display: block;
            height: 100%;
            padding: var(--uui-size-layout-1);
        }
    `;
}

customElements.define(elementName, N3oPlatformsPreviewElement);

export default N3oPlatformsPreviewElement;
export { N3oPlatformsPreviewElement };
