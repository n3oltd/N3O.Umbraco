import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_BLOCK_ENTRY_CONTEXT, UMB_BLOCK_MANAGER_CONTEXT } from '@umbraco-cms/backoffice/block';
import type { UmbBlockManagerContext, UmbBlockLayoutBaseModel, UmbBlockDataModel, UmbBlockExposeModel } from '@umbraco-cms/backoffice/block';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';

import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import type { UmbActiveVariant } from '@umbraco-cms/backoffice/workspace';

import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { BlockPreviewApp } from './block-preview-app';

const elementName = 'n3o-block-preview';

// Shape of the full BlockGrid value we POST to the preview endpoint.
interface BlockGridValue {
    layout: { 'Umbraco.BlockGrid': UmbBlockLayoutBaseModel[] };
    contentData: UmbBlockDataModel[];
    settingsData: UmbBlockDataModel[];
    expose: UmbBlockExposeModel[];
}

// Web-component SHELL for the block grid custom view. The Lit base (UmbElementMixin) is kept ONLY for
// context plumbing — it consumes the document workspace + block entry/manager contexts, POSTs the whole
// block grid editor value to the backoffice preview endpoint, and pushes the returned server-rendered
// HTML markup into the React app (BlockPreviewApp), which renders it. React itself is NOT bundled here —
// it is external and resolved at runtime from the shared N3O.Umbraco.ReactRuntime import map.
//
// Ported from the AngularJS "N3O.Umbraco.Blocks.Preview" controller (previewGridBlock endpoint).
@customElement(elementName)
export class N3oBlockPreviewElement extends UmbElementMixin(HTMLElement) implements UmbBlockEditorCustomViewElement {
    // Properties provided by the block editor custom-view contract (UmbBlockEditorCustomViewElement).
    // Setters re-render React so the preview reloads when the block's data or settings change.
    #content?: UmbBlockEditorCustomViewElement['content'];
    #settings?: UmbBlockEditorCustomViewElement['settings'];

    get content(): UmbBlockEditorCustomViewElement['content'] {
        return this.#content;
    }

    set content(value: UmbBlockEditorCustomViewElement['content']) {
        this.#content = value;
        this.#onDataChanged();
    }

    get settings(): UmbBlockEditorCustomViewElement['settings'] {
        return this.#settings;
    }

    set settings(value: UmbBlockEditorCustomViewElement['settings']) {
        this.#settings = value;
        this.#onDataChanged();
    }

    #root?: Root;
    #mount: HTMLDivElement;

    #loaded = false;
    #markup = '';

    #nodeKey: string | undefined;
    #documentTypeKey: string | undefined;
    #culture = '';
    #contentKey: string | undefined;
    #reloadHandle: ReturnType<typeof setTimeout> | undefined;
    #blockManager: UmbBlockManagerContext | undefined;

    constructor() {
        super();

        const shadow = this.attachShadow({ mode: 'open' });
        this.#mount = document.createElement('div');
        shadow.appendChild(this.#mount);

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

    connectedCallback(): void {
        super.connectedCallback();

        this.#root ??= createRoot(this.#mount);
        this.#render();

        // Defer until contexts have resolved on the next frame, mirroring the original loadPreview() call.
        this.#scheduleReload(0);
    }

    disconnectedCallback(): void {
        super.disconnectedCallback();

        if (this.#reloadHandle !== undefined) {
            clearTimeout(this.#reloadHandle);
            this.#reloadHandle = undefined;
        }

        this.#root?.unmount();
        this.#root = undefined;
    }

    // Re-render the preview when the block's data or settings change (matches the $watch debouncing).
    #onDataChanged(): void {
        if (this.#loaded) {
            this.#scheduleReload(500);
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

        this.#markup = markup;
        this.#loaded = true;
        this.#render();
    }

    #render(): void {
        this.#root?.render(
            createElement(BlockPreviewApp, {
                loaded: this.#loaded,
                markup: this.#markup,
            }),
        );
    }
}

export default N3oBlockPreviewElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oBlockPreviewElement;
    }
}
