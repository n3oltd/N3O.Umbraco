import { LitElement, css, customElement, html } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import type {
    UmbDocumentDetailModel,
    UmbDocumentValueModel,
    UmbDocumentVariantModel,
    UmbDocumentWorkspaceContext,
} from '@umbraco-cms/backoffice/document';
import { UmbAuthFetchMixin } from '@n3oltd/backoffice-core';
import type { AuthFetch } from '@n3oltd/backoffice-core';

interface PreviewHtmlResponse {
    eTag: string;
    html: string;
}

const elementName = 'n3o-platforms-preview';

@customElement(elementName)
export class N3oPlatformsPreviewElement extends UmbAuthFetchMixin(UmbElementMixin(LitElement)) {
    #workspaceContext?: UmbDocumentWorkspaceContext;
    #previousETag: string | null = null;
    #intervalId?: number;
    #disconnected = false;

    constructor() {
        super();

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            this.#workspaceContext = context;

            this.observe(context?.unique, () => {
                this.#restart();
            });
        });
    }

    override connectedCallback(): void {
        super.connectedCallback();

        this.#disconnected = false;
        this.#restart();
    }

    override disconnectedCallback(): void {
        super.disconnectedCallback();

        this.#disconnected = true;
        this.#stop();
    }

    authFetchChanged(_authFetch: AuthFetch | null): void {
        this.#restart();
    }

    #stop(): void {
        if (this.#intervalId != null) {
            window.clearInterval(this.#intervalId);
            this.#intervalId = undefined;
        }
    }

    #restart(): void {
        this.#stop();

        void this.#loadPreview();

        this.#intervalId = window.setInterval(() => {
            void this.#loadPreview();
        }, 10000);
    }

    async #loadPreview(): Promise<void> {
        if (!this.authFetch) {
            return;
        }

        const content = this.#workspaceContext?.getData();

        if (!content) {
            return;
        }

        const documentTypeUnique = content.documentType?.unique;

        const rawContent = content as UmbDocumentDetailModel & {
            parent?: { unique?: string };
            parentId?: string;
        };

        const variants: Array<UmbDocumentVariantModel> = content.variants ?? [];
        const variant = variants.find((v) => v.culture == null || v.segment == null) ?? variants[0];

        const values: Array<UmbDocumentValueModel> = content.values ?? [];
        const apiReq: Record<string, unknown> = {};

        values.forEach((property) => {
            apiReq[property.alias] = property.value;
        });

        apiReq['contentTypeAlias'] = documentTypeUnique;
        apiReq['name'] = variant?.name;
        apiReq['key'] = content.unique;
        apiReq['parentId'] = rawContent.parent?.unique ?? rawContent.parentId;

        const subscriptionCodeRes = await this.authFetch('/umbraco/backoffice/api/cloudBackOffice/subscription/code');
        const subscriptionCode = (await subscriptionCodeRes.json()) as string;

        const apiRes = await this.authFetch(
            `/umbraco/backoffice/api/platformsBackOffice/previewHtml/${content.unique}`,
            {
                method: 'POST',
                headers: {
                    accept: 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(apiReq),
            },
        );

        const res = (await apiRes.json()) as PreviewHtmlResponse;

        if (this.#disconnected || res.eTag === this.#previousETag) {
            return;
        }

        this.#previousETag = res.eTag;

        const container = this.renderRoot.querySelector<HTMLDivElement>('#platformsPreviewContainer');

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

        const doc = iframe.contentWindow!.document;
        doc.open();
        doc.write(res.html);
        doc.close();

        const script = doc.createElement('script');
        script.src = `https://cdn.n3o.cloud/connect-${subscriptionCode}/platforms-js/platforms.js`;
        script.type = 'module';

        doc.body.appendChild(script);

        window.setTimeout(() => {
            if (this.#disconnected) {
                return;
            }

            iframe.style.display = 'block';
            container.style.display = 'block';
        }, 2000);
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
