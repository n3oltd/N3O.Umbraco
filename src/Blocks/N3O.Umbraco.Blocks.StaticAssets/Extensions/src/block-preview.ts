import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { UMB_BLOCK_ENTRY_CONTEXT, UMB_BLOCK_MANAGER_CONTEXT } from '@umbraco-cms/backoffice/block';
import type { UmbBlockManagerContext, UmbBlockLayoutBaseModel, UmbBlockDataModel, UmbBlockExposeModel } from '@umbraco-cms/backoffice/block';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';

import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import type { UmbActiveVariant } from '@umbraco-cms/backoffice/workspace';

import { UmbAuthFetchMixin, UmbElementMixin } from '@n3oltd/backoffice-core';
import type { AuthFetch } from '@n3oltd/backoffice-core';

import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { BlockPreviewApp } from './block-preview-app';

const elementName = 'n3o-block-preview';

interface BlockGridValue {
    layout: { 'Umbraco.BlockGrid': UmbBlockLayoutBaseModel[] };
    contentData: UmbBlockDataModel[];
    settingsData: UmbBlockDataModel[];
    expose: UmbBlockExposeModel[];
}

@customElement(elementName)
export class N3oBlockPreviewElement extends UmbAuthFetchMixin(UmbElementMixin(HTMLElement)) implements UmbBlockEditorCustomViewElement {
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
            this.observe(context.contentElementTypeKey, (key) => { this.#documentTypeKey = key; }, '_observeContentElementTypeKey');
        });

        this.consumeContext(UMB_BLOCK_MANAGER_CONTEXT, (context) => {
            this.#blockManager = context;
        });
    }

    authFetchChanged(_authFetch: AuthFetch | null): void {
        this.#scheduleReload(0);
    }

    connectedCallback(): void {
        super.connectedCallback();

        this.#root ??= createRoot(this.#mount);
        this.#render();

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

        if (!blockData || !this.#documentTypeKey || !this.authFetch) {
            return;
        }

        const nodeKey = this.#nodeKey ?? '';
        const contentUdi = this.#toElementUdi(this.#contentKey);
        const culture = this.#culture ?? '';

        const url = `/umbraco/backoffice/api/blockPreviewBackoffice/previewGridBlock/?nodeKey=${nodeKey}&documentTypeKey=${this.#documentTypeKey}&contentUdi=${contentUdi}&culture=${culture}`;

        const response = await this.authFetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(blockData),
        });

        if (!response.ok) {
            return;
        }

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
