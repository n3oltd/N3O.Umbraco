import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { UMB_BLOCK_ENTRY_CONTEXT, UMB_BLOCK_MANAGER_CONTEXT } from '@umbraco-cms/backoffice/block';
import type { UmbBlockManagerContext, UmbBlockLayoutBaseModel, UmbBlockDataModel, UmbBlockExposeModel } from '@umbraco-cms/backoffice/block';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';

import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { UMB_NOTIFICATION_CONTEXT, type UmbNotificationContext } from '@umbraco-cms/backoffice/notification';

import { UmbAuthFetchMixin, UmbElementMixin } from '@n3oltd/backoffice-core';
import type { AuthFetch } from '@n3oltd/backoffice-core';

import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { BlockPreviewApp } from './block-preview-app';
import type { PreviewState } from './types';

const elementName = 'n3o-block-preview';
const previewEndpoint = '/umbraco/backoffice/api/blockPreviewBackoffice/previewGridBlock';
const editDebounceMs = 500;
const previewFailedMessage = 'Failed getting block preview markup';

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
    #layout?: UmbBlockEditorCustomViewElement['layout'];

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

    // Resizing a block changes its layout entry rather than its content, and the preview is rendered from the
    // whole grid value, so column and row spans have to re-render it too.
    get layout(): UmbBlockEditorCustomViewElement['layout'] {
        return this.#layout;
    }

    set layout(value: UmbBlockEditorCustomViewElement['layout']) {
        this.#layout = value;
        this.#onDataChanged();
    }

    #root?: Root;
    #mount: HTMLDivElement;

    #state: PreviewState = { status: 'loading' };
    #notificationContext?: UmbNotificationContext;
    #inFlight?: AbortController;

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

        this.consumeContext(UMB_NOTIFICATION_CONTEXT, (context) => {
            this.#notificationContext = context ?? undefined;
        });

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            if (!context) {
                return;
            }

            // Re-subscribing replays the current value, and consumeContext can run more than once, so each
            // observer reloads only when the value it feeds into the request has actually moved. The abort is
            // client-side, so a redundant request still costs a server-side Razor render.
            this.observe(context.unique, (unique) => {
                if (unique === this.#nodeKey) {
                    return;
                }

                this.#nodeKey = unique;
                this.#scheduleReload(0);
            }, '_observeUnique');

            this.observe(
                context.splitView.activeVariantsInfo,
                (infos) => {
                    const culture = infos[0]?.culture ?? '';

                    if (culture === this.#culture) {
                        return;
                    }

                    this.#culture = culture;
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
                if (key === this.#contentKey) {
                    return;
                }

                this.#contentKey = key;
                this.#scheduleReload(0);
            }, '_observeContentKey');

            this.observe(context.contentElementTypeKey, (key) => {
                if (key === this.#contentElementTypeKey) {
                    return;
                }

                this.#contentElementTypeKey = key;
                this.#scheduleReload(0);
            }, '_observeContentElementTypeKey');
        });

        this.consumeContext(UMB_BLOCK_MANAGER_CONTEXT, (context) => {
            if (!context || context === this.#blockManager) {
                return;
            }

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

        clearTimeout(this.#reloadHandle);
        this.#reloadHandle = undefined;

        this.#inFlight?.abort();
        this.#inFlight = undefined;

        this.#root?.unmount();
        this.#root = undefined;
    }

    #onDataChanged(): void {
        this.#scheduleReload(editDebounceMs);
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

        this.#inFlight?.abort();

        const abort = new AbortController();
        this.#inFlight = abort;

        try {
            const response = await this.authFetch(url, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', Accept: 'application/json' },
                body: JSON.stringify(blockData),
                signal: abort.signal,
            });

            if (!response.ok) {
                throw new Error(`Preview request failed with status ${response.status}`);
            }

            const payload: unknown = await response.json();

            if (typeof payload !== 'string') {
                throw new Error('Preview response was not markup');
            }

            this.#setState({ status: 'ready', markup: payload });
        } catch (error) {
            if (abort.signal.aborted) {
                return;
            }

            console.error('Block preview failed', error);

            // A block that keeps failing reloads on every edit, so only the transition into failure is
            // announced; backoffice toasts stack and would otherwise pile up as the editor types.
            if (this.#state.status !== 'error') {
                this.#notificationContext?.peek('danger', {
                    data: { headline: 'Block preview', message: previewFailedMessage },
                });
            }

            this.#setState({ status: 'error', message: previewFailedMessage });
        } finally {
            if (this.#inFlight === abort) {
                this.#inFlight = undefined;
            }
        }
    }

    #setState(state: PreviewState): void {
        this.#state = state;
        this.#render();
    }

    #render(): void {
        this.#root?.render(createElement(BlockPreviewApp, { state: this.#state }));
    }
}

export default N3oBlockPreviewElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oBlockPreviewElement;
    }
}
