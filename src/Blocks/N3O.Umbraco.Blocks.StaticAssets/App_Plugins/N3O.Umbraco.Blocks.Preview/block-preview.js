import { LitElement, html, css } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { UMB_BLOCK_ENTRY_CONTEXT, UMB_BLOCK_MANAGER_CONTEXT } from '@umbraco-cms/backoffice/block';

const elementName = 'n3o-block-preview';

// Block grid custom view that renders a server-rendered preview of the block. It posts the whole
// block grid editor value to the backoffice preview endpoint and writes the returned HTML markup into
// an iframe, then scales/sizes the iframe to fit its content. Ported from the AngularJS
// "N3O.Umbraco.Blocks.Preview" controller (previewGridBlock endpoint, bind-compile/iframe rendering).
class N3oBlockPreviewElement extends UmbElementMixin(LitElement) {
    static properties = {
        // Provided by the block editor custom view contract.
        content: { attribute: false },
        settings: { attribute: false },
        config: { attribute: false },
        index: { type: Number },
        // Internal UI state.
        _loaded: { state: true },
    };

    #nodeKey;
    #documentTypeKey;
    #culture = '';
    #contentKey;
    #reloadHandle;

    constructor() {
        super();

        this._loaded = false;

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            if (!context) {
                return;
            }

            this.observe(context.unique, (unique) => { this.#nodeKey = unique; }, '_observeUnique');

            this.observe(
                context.splitView.activeVariantsInfo,
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

    #blockManager;

    connectedCallback() {
        super.connectedCallback();

        // Defer until contexts have resolved on the next frame, mirroring the original loadPreview() call.
        this.#scheduleReload(0);
    }

    disconnectedCallback() {
        super.disconnectedCallback();

        if (this.#reloadHandle) {
            clearTimeout(this.#reloadHandle);
            this.#reloadHandle = undefined;
        }
    }

    // Re-render the preview when the block's data or settings change (matches the $watch debouncing).
    updated(changedProperties) {
        if (changedProperties.has('content') || changedProperties.has('settings')) {
            if (this._loaded) {
                this.#scheduleReload(500);
            }
        }
    }

    #scheduleReload(delay) {
        if (this.#reloadHandle) {
            clearTimeout(this.#reloadHandle);
        }

        this.#reloadHandle = setTimeout(() => this.#loadPreview(), delay);
    }

    #buildBlockData() {
        if (!this.#blockManager) {
            return null;
        }

        const layouts = this.#blockManager.getLayouts?.() ?? [];
        const contentData = this.#blockManager.getContents?.() ?? [];
        const settingsData = this.#blockManager.getSettings?.() ?? [];
        const expose = this.#blockManager.getExposes?.() ?? [];

        return {
            layout: {
                'Umbraco.BlockGrid': layouts,
            },
            contentData,
            settingsData,
            expose,
        };
    }

    #toElementUdi(key) {
        if (!key) {
            return '';
        }

        return `umb://element/${key.replace(/-/g, '')}`;
    }

    async #loadPreview() {
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
        const markup = await response.json();

        this._loaded = true;

        await this.updateComplete;

        this.#renderIntoFrame(markup);
    }

    #renderIntoFrame(markup) {
        const iframe = this.renderRoot.querySelector('.block-preview-frame');

        if (!iframe) {
            return;
        }

        const doc = iframe.contentDocument || iframe.contentWindow.document;

        doc.open();
        doc.write(markup);
        doc.close();

        const resizeIframe = () => {
            const body = doc.body.querySelector('.preview-content');
            const height = body ? body.scrollHeight : doc.body.scrollHeight;

            iframe.style.height = height + 'px';
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

    render() {
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

    static styles = css`
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

customElements.define(elementName, N3oBlockPreviewElement);

export default N3oBlockPreviewElement;
export { N3oBlockPreviewElement };
