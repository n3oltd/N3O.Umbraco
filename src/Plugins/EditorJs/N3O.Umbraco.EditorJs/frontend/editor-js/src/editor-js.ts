import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import type {
    UmbPropertyEditorConfigCollection,
    UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { UMB_MODAL_MANAGER_CONTEXT } from '@umbraco-cms/backoffice/modal';
import { UMB_MEDIA_PICKER_MODAL, UmbMediaUrlRepository, UmbMediaItemRepository } from '@umbraco-cms/backoffice/media';
import { UMB_LINK_PICKER_MODAL } from '@umbraco-cms/backoffice/multi-url-picker';
import type { MediaPickerResultItem } from './tools/UmbracoImageTool';
import type { EditorJsFrameConfig } from './editor-js-frame';

const elementName = 'n3o-editor-js';

// The <iframe> is same-origin, so the shell and the frame module communicate through the iframe element
// itself: the shell sets __n3oOnReady and the frame calls it once loaded, then the shell calls __n3oInit
// with live callbacks (value/change + the two pickers).
type EditorJsFrame = HTMLIFrameElement & {
    __n3oInit?: (config: EditorJsFrameConfig) => void;
    __n3oOnReady?: () => void;
};

interface MediaPickerModalResult {
    selection: Array<string | null>;
}

interface LinkPickerModalResult {
    link?: { url?: string; unique?: string };
}

// Web-component SHELL for the EditorJS property editor.
//
// EditorJS assumes a top-level document (it injects styles into document.head and its outside-click
// handler reads event.target). Umbraco 17's backoffice is entirely shadow DOM, which breaks both
// (styles can't cross the boundary; event.target retargets to umb-app so EditorJS closes its toolbar on
// every click). Rather than shim each symptom, this shell hosts EditorJS in a same-origin IFRAME — its
// own real document — so styles and events both work natively. The heavy editor bundle runs inside the
// frame (editor-js-frame.js); this shell only owns the Umbraco contract (value + change event) and
// bridges the media/link pickers back to the parent, where the Umbraco modal manager lives.
@customElement(elementName)
export class N3oEditorJsElement
    extends UmbElementMixin(HTMLElement)
    implements UmbPropertyEditorUiElement
{
    #iframe?: EditorJsFrame;
    #value: string | undefined;
    #resizeObserver?: ResizeObserver;
    // FLAG: UMB_MODAL_MANAGER_CONTEXT consumer value — using `any` then casting at call site (as in the
    // original Lit version).
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    #modalManager: any;

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
        // Init-once semantics (matching the previous implementation): the editor is hydrated from the
        // value present when the frame is ready; later external value changes are not pushed back in.
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
        // Resolve the sibling frame bundle at runtime. NOTE: do NOT use `new URL('literal',
        // import.meta.url)` — Vite rewrites that at build time into an inlined asset (it would embed
        // the .ts source as a data: URI). A plain string replace on the runtime module URL is untouched.
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

        // Size the iframe to its content so the editor grows with the document.
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

    // Runs in the parent realm (where the Umbraco modal manager + media repositories live). Opens the
    // media picker, resolves the picked GUID to a URL + name, and returns plain data for the frame's tool.
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
                    udi: unique,
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

        const modal = modalManager.open(this, UMB_LINK_PICKER_MODAL, { data: { config: {} } });

        try {
            const result = (await modal.onSubmit()) as LinkPickerModalResult;
            const link = result?.link;
            return link ? (link.url ?? link.unique ?? '') : null;
        } catch {
            // Picker cancelled.
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
