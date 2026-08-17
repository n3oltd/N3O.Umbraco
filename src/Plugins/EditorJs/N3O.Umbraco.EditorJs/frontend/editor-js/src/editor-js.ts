import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import type {
    UmbPropertyEditorConfigCollection,
    UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { UMB_MODAL_MANAGER_CONTEXT, type UmbModalManagerContext } from '@umbraco-cms/backoffice/modal';
import { UMB_MEDIA_PICKER_MODAL, UmbMediaUrlRepository, UmbMediaItemRepository } from '@umbraco-cms/backoffice/media';
import { UMB_LINK_PICKER_MODAL } from '@umbraco-cms/backoffice/multi-url-picker';
import type { MediaPickerResultItem } from './tools/UmbracoImageTool';
import type { EditorJsFrameConfig } from './editor-js-frame';

const elementName = 'n3o-editor-js';

// The handshake hangs off the iframe element rather than postMessage or a window global, which keeps it
// per-instance and lets live function references cross into the frame. Only safe because it is same-origin.
type EditorJsFrame = HTMLIFrameElement & {
    __n3oInit?: (config: EditorJsFrameConfig) => void;
    __n3oOnReady?: () => void;
};

interface MediaPickerModalResult {
    selection: Array<string | null>;
}

// EditorJS assumes a top-level document: it injects styles into document.head and its outside-click
// handler reads event.target. Umbraco 17's backoffice is entirely shadow DOM, which breaks both — styles
// cannot cross the boundary, and event.target retargets to umb-app, so EditorJS closes its own toolbar on
// every click. EditorJS does not support shadow DOM (codex-team/editor.js#1009), so it is hosted in a
// same-origin iframe where both work natively.
@customElement(elementName)
export class N3oEditorJsElement
    extends UmbElementMixin(HTMLElement)
    implements UmbPropertyEditorUiElement
{
    #iframe?: EditorJsFrame;
    #value: string | undefined;
    #resizeObserver?: ResizeObserver;
    #modalManager?: UmbModalManagerContext;

    constructor() {
        super();

        this.consumeContext(UMB_MODAL_MANAGER_CONTEXT, (context) => {
            this.#modalManager = context;
        });
    }

    get value(): string | undefined {
        return this.#value;
    }

    set value(value: string | undefined) {
        // Init-once: the editor is hydrated from the value present when the frame is ready, and later
        // external value changes are not pushed back in.
        this.#value = value;
    }

    // config is set by Umbraco; not used by this editor but accepted to satisfy the contract.
    public set config(_config: UmbPropertyEditorConfigCollection | undefined) {}

    connectedCallback(): void {
        super.connectedCallback();
        if (!this.#iframe) {
            this.#createFrame();
        }
    }

    disconnectedCallback(): void {
        super.disconnectedCallback();
        this.#resizeObserver?.disconnect();
        this.#resizeObserver = undefined;
        // Iframes reload their content when re-parented, so tear down and rebuild from the latest value
        // on reconnect (the value is kept current via the change callback).
        this.#iframe?.remove();
        this.#iframe = undefined;
    }

    #createFrame(): void {
        const iframe = document.createElement('iframe') as EditorJsFrame;
        iframe.setAttribute('allow', 'fullscreen');
        iframe.style.width = '100%';
        iframe.style.border = '0';
        iframe.style.display = 'block';
        iframe.style.minHeight = '300px';
        iframe.__n3oOnReady = () => this.#onFrameReady();

        this.appendChild(iframe);
        this.#iframe = iframe;

        const doc = iframe.contentDocument;
        if (!doc) {
            return;
        }
        doc.open();
        doc.write('<!doctype html><html><head><meta charset="utf-8"></head><body></body></html>');
        doc.close();

        const script = doc.createElement('script');
        script.type = 'module';
        // Do not switch this to `new URL('literal', import.meta.url)` — Vite rewrites that at build time
        // into an inlined asset, embedding the .ts source as a data: URI. A string replace on the runtime
        // module URL is left alone.
        script.src = import.meta.url.replace('editor-js.js', 'editor-js-frame.js');
        doc.head.appendChild(script);
    }

    #onFrameReady(): void {
        const iframe = this.#iframe;
        if (!iframe?.__n3oInit) {
            return;
        }

        iframe.__n3oInit({
            value: this.#value,
            onChange: (value: string) => {
                this.#value = value;
                this.dispatchEvent(new UmbChangeEvent());
            },
            pickMedia: () => this.#pickMedia(),
            pickLink: () => this.#pickLink(),
        });

        const body = iframe.contentDocument?.body;
        if (body) {
            const resize = () => {
                iframe.style.height = `${body.scrollHeight}px`;
            };
            this.#resizeObserver = new ResizeObserver(resize);
            this.#resizeObserver.observe(body);
            resize();
        }
    }

    // Must run in the parent realm: the Umbraco modal manager and media repositories live there, not in
    // the frame. Returns plain data so the frame's tool stays free of Umbraco imports.
    async #pickMedia(): Promise<MediaPickerResultItem | null> {
        const modalManager = this.#modalManager;
        if (!modalManager) {
            return null;
        }

        const modal = modalManager.open(this, UMB_MEDIA_PICKER_MODAL, { data: { multiple: false } });

        try {
            const result = (await modal.onSubmit()) as MediaPickerModalResult;
            const unique = result?.selection?.find((s) => s != null) ?? null;
            if (!unique) {
                return null;
            }

            const urlRepo = new UmbMediaUrlRepository(this);
            const itemRepo = new UmbMediaItemRepository(this);
            try {
                const [urlResult, itemResult] = await Promise.all([
                    urlRepo.requestItems([unique]),
                    itemRepo.requestItems([unique]),
                ]);
                return {
                    url: urlResult.data?.[0]?.url ?? '',
                    name: itemResult.data?.[0]?.name ?? '',
                    unique,
                    // ImageBlockData.Udi deserialises into Umbraco's Udi type, which expects
                    // umb://media/<guid> with the guid in dash-less "N" form; the picker returns the
                    // bare, dashed entity key.
                    udi: `umb://media/${unique.replace(/-/g, '')}`,
                };
            } finally {
                urlRepo.destroy();
                itemRepo.destroy();
            }
        } catch {
            // Picker cancelled or resolution failed.
            return null;
        }
    }

    async #pickLink(): Promise<string | null> {
        const modalManager = this.#modalManager;
        if (!modalManager) {
            return null;
        }

        // The EditorJS link tool always wraps the current selection in a new link, so the picker never
        // opens against an existing entry.
        const modal = modalManager.open(this, UMB_LINK_PICKER_MODAL, {
            data: { config: {}, index: null, isNew: true },
        });

        try {
            const result = await modal.onSubmit();
            const link = result?.link;
            return link ? (link.url ?? link.unique ?? '') : null;
        } catch {
            return null;
        }
    }
}

export default N3oEditorJsElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oEditorJsElement;
    }
}
