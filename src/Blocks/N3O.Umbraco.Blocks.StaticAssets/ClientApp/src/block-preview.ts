import { LitElement, css, customElement, html, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_BLOCK_ENTRY_CONTEXT, UMB_BLOCK_MANAGER_CONTEXT } from '@umbraco-cms/backoffice/block';
import type { UmbBlockManagerContext, UmbBlockLayoutBaseModel, UmbBlockDataModel, UmbBlockExposeModel } from '@umbraco-cms/backoffice/block';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';

import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import type { UmbActiveVariant } from '@umbraco-cms/backoffice/workspace';

const elementName = 'n3o-block-preview';

// Shape of the full BlockGrid value we POST to the preview endpoint.
interface BlockGridValue {
    layout: { 'Umbraco.BlockGrid': UmbBlockLayoutBaseModel[] };
    contentData: UmbBlockDataModel[];
    settingsData: UmbBlockDataModel[];
    expose: UmbBlockExposeModel[];
}

// Block grid custom view that renders a server-rendered preview of the block. It posts the whole
// block grid editor value to the backoffice preview endpoint and writes the returned HTML markup
// into an iframe, then scales/sizes the iframe to fit its content. Ported from the AngularJS
// "N3O.Umbraco.Blocks.Preview" controller (previewGridBlock endpoint, bind-compile/iframe rendering).
@customElement(elementName)
export class N3oBlockPreviewElement extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {
    // Properties provided by the block editor custom-view contract (UmbBlockEditorCustomViewElement).
    // Declared with @state so Lit tracks changes and calls updated().
    @state() content?: UmbBlockEditorCustomViewElement['content'];
    @state() settings?: UmbBlockEditorCustomViewElement['settings'];

    @state() private _loaded = false;

    #nodeKey: string | undefined;
    #documentTypeKey: string | undefined;
    #culture = '';
    #contentKey: string | undefined;
    #reloadHandle: ReturnType<typeof setTimeout> | undefined;
    #blockManager: UmbBlockManagerContext | undefined;

    constructor() {
        super();

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context: unknown) => {
            if (!context) {
                return;
            }

            // Cast to a minimal structural interface covering only the fields we access.
            const ctx = context as {
                unique: import('@umbraco-cms/backoffice/external/rxjs').Observable<string | undefined>;
                splitView: { activeVariantsInfo: import('@umbraco-cms/backoffice/external/rxjs').Observable<UmbActiveVariant[]> };
            };

            this.observe(ctx.unique, (unique) => { this.#nodeKey = unique; }, '_observeUnique');

            this.observe(
                ctx.splitView.activeVariantsInfo,
                (infos) => {
                    const culture = infos?.[0]?.culture;
                    this.#culture = culture ?? '';
                },
                '_observeCulture'
            );
        });

        this.consumeContext(UMB_BLOCK_ENTRY_CONTEXT, (context) => {
            if (!context) {
                return;
            }

            this.observe(context.contentKey, (key) => { this.#contentKey = key; }, '_observeContentKey');
            // The block element's content type key (matches the AngularJS ElementEditorContentComponentController.model.contentTypeKey).
            this.observe(context.contentElementTypeKey, (key) => { this.#documentTypeKey = key; }, '_observeContentElementTypeKey');
        });

        this.consumeContext(UMB_BLOCK_MANAGER_CONTEXT, (context) => {
            this.#blockManager = context;
        });
    }

    override connectedCallback(): void {
        super.connectedCallback();

        // Defer until contexts have resolved on the next frame, mirroring the original loadPreview() call.
        this.#scheduleReload(0);
    }

    override disconnectedCallback(): void {
        super.disconnectedCallback();

        if (this.#reloadHandle !== undefined) {
            clearTimeout(this.#reloadHandle);
            this.#reloadHandle = undefined;
        }
    }

    // Re-render the preview when the block's data or settings change (matches the $watch debouncing).
    override updated(changedProperties: Map<PropertyKey, unknown>): void {
        if (changedProperties.has('content') || changedProperties.has('settings')) {
            if (this._loaded) {
                this.#scheduleReload(500);
            }
        }
    }

    #scheduleReload(delay: number): void {
        if (this.#reloadHandle !== undefined) {
            clearTimeout(this.#reloadHandle);
        }

        this.#reloadHandle = setTimeout(() => { void this.#loadPreview(); }, delay);
    }

    #buildBlockData(): BlockGridValue | null {
        if (!this.#blockManager) {
            return null;
        }

        const layouts = this.#blockManager.getLayouts();
        const contentData = this.#blockManager.getContents();
        const settingsData = this.#blockManager.getSettings();
        const expose = this.#blockManager.getExposes();

        return {
            layout: {
                'Umbraco.BlockGrid': layouts,
            },
            contentData,
            settingsData,
            expose,
        };
    }

    #toElementUdi(key: string | undefined): string {
        if (!key) {
            return '';
        }

        return `umb://element/${key.replace(/-/g, '')}`;
    }

    async #loadPreview(): Promise<void> {
        const blockData = this.#buildBlockData();

        if (!blockData || !this.#documentTypeKey) {
            return;
        }

        const nodeKey = this.#nodeKey ?? '';
        const contentUdi = this.#toElementUdi(this.#contentKey);
        const culture = this.#culture ?? '';

        const url = `/umbraco/backoffice/api/blockPreviewBackoffice/previewGridBlock/?nodeKey=${nodeKey}&documentTypeKey=${this.#documentTypeKey}&contentUdi=${contentUdi}&culture=${culture}`;

        const response = await fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(blockData),
        });

        if (!response.ok) {
            return;
        }

        // Endpoint returns the markup as a JSON-encoded string.
        const markup = (await response.json()) as string;

        this._loaded = true;

        await this.updateComplete;

        this.#renderIntoFrame(markup);
    }

    #renderIntoFrame(markup: string): void {
        const iframe = this.renderRoot.querySelector<HTMLIFrameElement>('.block-preview-frame');

        if (!iframe) {
            return;
        }

        // contentDocument is always present on same-origin iframes; the fallback covers old browsers.
        const doc = iframe.contentDocument ?? (iframe.contentWindow as Window & typeof globalThis).document;

        doc.open();
        doc.write(markup);
        doc.close();

        const resizeIframe = (): void => {
            const body = doc.body.querySelector<HTMLElement>('.preview-content');
            const height = body ? body.scrollHeight : doc.body.scrollHeight;

            iframe.style.height = `${height}px`;
            iframe.style.width = '100%';
            iframe.style.border = 'none';
            iframe.style.display = 'block';
            iframe.style.transform = 'scale(0.9)';
        };

        let checks = 0;
        const interval = setInterval(() => {
            resizeIframe();
            if (++checks > 2) {
                clearInterval(interval);
            }
        }, 100);
    }

    override render() {
        return html`
            ${!this._loaded
                ? html`<div class="preview-alert preview-alert-info">
                      <uui-loader style="color: #fff"></uui-loader>
                      Loading preview...
                  </div>`
                : ''}
            <iframe class="block-preview-frame" style="display: none"></iframe>
        `;
    }

    static override styles = css`
        :host {
            display: block;
        }

        .preview-alert {
            background-color: #f0ac00;
            border: 1px solid transparent;
            border-radius: 0;
            margin-bottom: 20px;
            padding: 8px 35px 8px 14px;
            position: relative;
        }

        .preview-alert uui-loader {
            margin-right: 16px;
        }

        .preview-alert,
        .preview-alert a,
        .preview-alert h4 {
            color: #fff;
        }

        .preview-alert pre {
            white-space: normal;
        }

        .preview-alert-warning {
            background-color: #f0ac00;
            border-color: transparent;
            color: #fff;
        }

        .preview-alert-info {
            background-color: #3544b1;
            border-color: transparent;
            color: #fff;
        }

        .preview-alert-danger,
        .preview-alert-error {
            background-color: #d42054;
            border-color: transparent;
            color: #fff;
        }
    `;
}

export default N3oBlockPreviewElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oBlockPreviewElement;
    }
}
