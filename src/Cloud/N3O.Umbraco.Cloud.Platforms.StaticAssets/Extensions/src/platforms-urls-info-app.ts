import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { html, css, customElement, state } from '@umbraco-cms/backoffice/external/lit';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { UMB_AUTH_CONTEXT, type UmbOpenApiConfiguration } from '@umbraco-cms/backoffice/auth';

interface ContentUrlsRes {
    permitted: boolean;
    stagingUrl: string | null;
    productionUrl: string | null;
}

const elementName = 'n3o-platforms-urls-info-app';

// workspaceInfoApp panel that renders staging + production platform URLs inside the document
// Info tab (replaces the v13 CampaignSending / OfferingSending SendingContentNotification approach).
// The element handles its own visibility: if the backend returns permitted=false (content is not
// a campaign or offering) it renders nothing, so no custom condition extension is needed.
@customElement(elementName)
export class N3oPlatformsUrlsInfoAppElement extends UmbLitElement {
    @state() private _stagingUrl: string | null = null;
    @state() private _productionUrl: string | null = null;

    #unique: string | null | undefined;
    #authConfig: UmbOpenApiConfiguration | null = null;

    constructor() {
        super();

        this.consumeContext(UMB_AUTH_CONTEXT, (authContext) => {
            this.#authConfig = authContext ? authContext.getOpenApiConfiguration() : null;

            if (this.#unique) {
                void this.#loadUrls(this.#unique);
            }
        });

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            this.observe(context?.unique, (unique) => {
                this.#unique = unique;

                if (unique && this.#authConfig) {
                    void this.#loadUrls(unique);
                }
            });
        });
    }

    async #loadUrls(unique: string): Promise<void> {
        if (!this.#authConfig) {
            return;
        }

        const rawToken = this.#authConfig.token;
        const token = typeof rawToken === 'function' ? await rawToken() : rawToken;

        const headers: Record<string, string> = { Accept: 'application/json' };

        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        try {
            const res = await fetch(`/umbraco/backoffice/api/PlatformsBackOffice/contentUrls/${unique}`, { headers });
            const data = (await res.json()) as ContentUrlsRes;

            this._stagingUrl = data.permitted ? (data.stagingUrl ?? null) : null;
            this._productionUrl = data.permitted ? (data.productionUrl ?? null) : null;
        } catch {
            this._stagingUrl = null;
            this._productionUrl = null;
        }
    }

    #copy(url: string): void {
        void navigator.clipboard.writeText(url);
    }

    override render() {
        if (!this._stagingUrl && !this._productionUrl) {
            return html``;
        }

        return html`
            <umb-workspace-info-app-layout headline="Platform URLs">
                ${this._stagingUrl ? this.#renderRow('Staging', this._stagingUrl) : ''}
                ${this._productionUrl ? this.#renderRow('Production', this._productionUrl) : ''}
            </umb-workspace-info-app-layout>
        `;
    }

    #renderRow(label: string, url: string) {
        return html`
            <div class="url-row">
                <span class="label">${label}</span>
                <a href="${url}" target="_blank" rel="noreferrer" class="url">${url}</a>
                <button class="copy" @click=${() => this.#copy(url)} title="Copy">Copy</button>
            </div>
        `;
    }

    static override styles = css`
        :host { display: block; }

        .url-row {
            display: flex;
            align-items: baseline;
            gap: var(--uui-size-space-3, 8px);
            padding: var(--uui-size-space-2, 4px) 0;
            font-size: 0.8125rem;
        }

        .label {
            flex-shrink: 0;
            width: 72px;
            color: var(--uui-color-text-alt);
            font-weight: 600;
        }

        .url {
            flex: 1;
            color: var(--uui-color-interactive);
            word-break: break-all;
            text-decoration: none;
        }

        .url:hover {
            text-decoration: underline;
        }

        .copy {
            flex-shrink: 0;
            background: none;
            border: 1px solid var(--uui-color-border);
            border-radius: 3px;
            cursor: pointer;
            color: var(--uui-color-interactive);
            padding: 2px 8px;
            font-size: 0.75rem;
        }

        .copy:hover {
            background: var(--uui-color-surface-alt);
        }
    `;
}

export default N3oPlatformsUrlsInfoAppElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oPlatformsUrlsInfoAppElement;
    }
}
