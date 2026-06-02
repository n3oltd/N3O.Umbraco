// TODO Migration Review (BLOCKER-10 #3): this Preview workspaceView is registered in
// umbraco-package.json with only an `Umb.Condition.WorkspaceAlias = Umb.Workspace.Document`
// condition, so the tab shows on ALL document types. The pre-Bellissima ContentApp only showed
// it for content composing the `platformsOffering` composition. There is no built-in Bellissima
// condition with OR/"composes composition X" semantics, so restoring that gating needs a custom
// `condition` extension (cf. DynamicListViews). Display-only (offering preview URLs), so UX not a
// privilege boundary — deferred.
import { LitElement, css, customElement, html } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import type { UmbDocumentDetailModel, UmbDocumentVariantModel, UmbDocumentValueModel, UmbDocumentWorkspaceContext } from '@umbraco-cms/backoffice/document';

const elementName = 'n3o-platforms-preview';

// Response shape from /umbraco/backoffice/api/platformsBackOffice/previewHtml/...
interface PreviewHtmlResponse {
    eTag: string;
    html: string;
}

// Workspace view (was the "platformsPreview" content app) that renders a live preview of the current
// document. It builds a request from the current variant's property values, posts it to the platforms
// back office preview endpoint, and renders the returned HTML in an iframe loading the tenant's
// platforms.js. The preview is refreshed every 10 seconds; unchanged responses (same eTag) are skipped.
@customElement(elementName)
export class N3oPlatformsPreviewElement extends UmbElementMixin(LitElement) {
    #workspaceContext: UmbDocumentWorkspaceContext | undefined;
    #previousETag: string | null = null;
    #intervalId: number | undefined;

    constructor() {
        super();

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            this.#workspaceContext = context;
        });
    }

    override connectedCallback(): void {
        super.connectedCallback();

        this.#loadPreview();

        this.#intervalId = window.setInterval(() => {
            this.#loadPreview();
        }, 10000);
    }

    override disconnectedCallback(): void {
        super.disconnectedCallback();

        if (this.#intervalId !== undefined) {
            window.clearInterval(this.#intervalId);
            this.#intervalId = undefined;
        }
    }

    async #loadPreview(): Promise<void> {
        if (!this.#workspaceContext) {
            return;
        }

        const content: UmbDocumentDetailModel | undefined = this.#workspaceContext.getData();

        if (!content) {
            return;
        }

        // WORKSPACE-CONTEXT SHAPE FLAG:
        // The TS type UmbDocumentDetailModel has no `contentType`, `contentTypeAlias`, `parent`, or `parentId`
        // properties. The original JS accessed these as legacy/undocumented fields on the in-memory model.
        // `documentType.unique` is the GUID not an alias; there is no typed way to get the content-type alias
        // via getData(). These casts preserve the runtime behaviour without changing the API calls.
        // They should be verified once the content-type alias requirement is clarified upstream.
        const rawContent = content as UmbDocumentDetailModel & {
            contentType?: { alias?: string };
            contentTypeAlias?: string;
            parent?: { unique?: string };
            parentId?: string;
        };

        const contentTypeAlias: string | undefined = rawContent.contentType?.alias ?? rawContent.contentTypeAlias;

        const variants: Array<UmbDocumentVariantModel> = content.variants ?? [];
        const variant: UmbDocumentVariantModel | undefined =
            variants.find((v) => v.culture == null || v.segment == null) ?? variants[0];

        const values: Array<UmbDocumentValueModel> = content.values ?? [];
        const apiReq = this.#getApiReq(values, contentTypeAlias);

        apiReq['name'] = variant?.name;
        apiReq['key'] = content.unique;
        apiReq['parentId'] = rawContent.parent?.unique ?? rawContent.parentId;

        const subscriptionCodeRes = await fetch('/umbraco/backoffice/api/cloudBackOffice/subscription/code');
        const subscriptionCode = (await subscriptionCodeRes.json()) as string;

        const apiRes = await fetch(`/umbraco/backoffice/api/platformsBackOffice/previewHtml/${contentTypeAlias}`, {
            method: 'POST',
            headers: {
                accept: 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(apiReq),
        });

        const res = (await apiRes.json()) as PreviewHtmlResponse;

        if (res.eTag === this.#previousETag) {
            return;
        }

        this.#previousETag = res.eTag;

        // renderRoot is a ShadowRoot which has getElementById, but the LitElement type widens it to
        // HTMLElement | DocumentFragment so a cast is needed.
        const container = (this.renderRoot as ShadowRoot).getElementById('platformsPreviewContainer');

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

        // iframe.contentWindow is non-null immediately after appending a same-origin iframe.
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        const doc = iframe.contentWindow!.document;
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

    #getApiReq(values: Array<UmbDocumentValueModel>, contentTypeAlias: string | undefined): Record<string, unknown> {
        const req: Record<string, unknown> = {};

        values.forEach((property) => {
            req[property.alias] = property.value;
        });

        req['contentTypeAlias'] = contentTypeAlias;

        return req;
    }

    override render() {
        return html`<div id="platformsPreviewContainer" style="display: none;"></div>`;
    }

    static override styles = css`
        :host {
            display: block;
            height: 100%;
            padding: var(--uui-size-layout-1);
        }
    `;
}

export default N3oPlatformsPreviewElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oPlatformsPreviewElement;
    }
}
