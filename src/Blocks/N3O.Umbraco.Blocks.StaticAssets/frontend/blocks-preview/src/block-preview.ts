import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { UMB_BLOCK_ENTRY_CONTEXT, UMB_BLOCK_MANAGER_CONTEXT } from '@umbraco-cms/backoffice/block';
import type { UmbBlockManagerContext, UmbBlockLayoutBaseModel, UmbBlockDataModel, UmbBlockExposeModel } from '@umbraco-cms/backoffice/block';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';

import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';

import { UmbAuthFetchMixin, UmbElementMixin } from '@n3oltd/backoffice-core';
import type { AuthFetch } from '@n3oltd/backoffice-core';

import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { BlockPreviewApp } from './block-preview-app';

const elementName = 'n3o-block-preview';
const previewEndpoint = '/umbraco/backoffice/api/blockPreviewBackoffice/previewGridBlock';
const editDebounceMs = 500;

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

    #nodeKey: string | null = null;
    #contentElementTypeKey: string | undefined;
    #culture = '';
    #contentKey: string | undefined;
    #reloadHandle: ReturnType<typeof setTimeout> | undefined;
    #blockManager: UmbBlockManagerContext | undefined;

    constructor() {
        super();

        const shadow = this.attachShadow({ mode: 'open' });
        this.#mount = document.createElement('div');
        shadow.appendChild(this.#mount);

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            if (!context) {
                return;
            }

            this.observe(context.unique, (unique) => {
                this.#nodeKey = unique;
                this.#scheduleReload(0);
            }, '_observeUnique');

            this.observe(
                context.splitView.activeVariantsInfo,
                (infos) => {
                    this.#culture = infos[0]?.culture ?? '';
                    this.#scheduleReload(0);
                },
                '_observeCulture'
            );
        });

        this.consumeContext(UMB_BLOCK_ENTRY_CONTEXT, (context) => {
            if (!context) {
                return;
            }

            this.observe(context.contentKey, (key) => {
                this.#contentKey = key;
                this.#scheduleReload(0);
            }, '_observeContentKey');

            this.observe(context.contentElementTypeKey, (key) => {
                this.#contentElementTypeKey = key;
                this.#scheduleReload(0);
            }, '_observeContentElementTypeKey');
        });

        this.consumeContext(UMB_BLOCK_MANAGER_CONTEXT, (context) => {
            this.#blockManager = context;
            this.#scheduleReload(0);
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
            this.#scheduleReload(editDebounceMs);
        }
    }

    #scheduleReload(delay: number): void {
        clearTimeout(this.#reloadHandle);

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

    // documentTypeKey is the wire name of the query parameter; the value the endpoint wants is the block's
    // element type key.
    #buildPreviewUrl(contentKey: string, contentElementTypeKey: string): string {
        const query = new URLSearchParams({
            nodeKey: this.#nodeKey ?? '',
            documentTypeKey: contentElementTypeKey,
            contentUdi: `umb://element/${contentKey.replaceAll('-', '')}`,
            culture: this.#culture,
        });

        return `${previewEndpoint}?${query}`;
    }

    async #loadPreview(): Promise<void> {
        const blockData = this.#buildBlockData();

        if (!blockData || !this.#contentKey || !this.#contentElementTypeKey || !this.authFetch) {
            return;
        }

        const url = this.#buildPreviewUrl(this.#contentKey, this.#contentElementTypeKey);

        const response = await this.authFetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', Accept: 'application/json' },
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
