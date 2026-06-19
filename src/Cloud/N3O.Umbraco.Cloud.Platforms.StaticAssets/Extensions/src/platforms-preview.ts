import { LitElement, css, customElement, html, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import type { UmbDocumentDetailModel, UmbDocumentWorkspaceContext } from '@umbraco-cms/backoffice/document';
import { UmbAuthFetchMixin } from '@n3o/backoffice-core';
import type { AuthFetch } from '@n3o/backoffice-core';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { PlatformsPreviewApp } from './platforms-preview-app';

const elementName = 'n3o-platforms-preview';

// Web-component SHELL for the Platforms preview workspace view. Umbraco's backoffice only loads custom
// elements, so this thin Lit element owns the Umbraco contract — it consumes UMB_DOCUMENT_WORKSPACE_CONTEXT
// and observes the document `unique` — then mounts the React UI (PlatformsPreviewApp) into its shadow
// root, passing the document `unique` plus a getter for the current in-memory content. The Lit base is
// only for context plumbing; React renders the UI. React itself is NOT bundled here — it is external and
// resolved at runtime from the shared N3O.Umbraco.ReactRuntime import map.
@customElement(elementName)
export class N3oPlatformsPreviewElement extends UmbAuthFetchMixin(UmbElementMixin(LitElement)) {
    #workspaceContext: UmbDocumentWorkspaceContext | undefined;
    #unique: string | null | undefined;
    #root?: Root;
    #mount: HTMLDivElement;

    constructor() {
        super();

        this.#mount = document.createElement('div');

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            this.#workspaceContext = context;

            this.observe(context?.unique, (unique) => {
                this.#unique = unique;
                this.#render();
            });
        });
    }

    override connectedCallback(): void {
        super.connectedCallback();

        this.#render();
    }

    override disconnectedCallback(): void {
        super.disconnectedCallback();

        this.#root?.unmount();
        this.#root = undefined;
    }

    // Lit owns the shadow root; we host a single mount div in it and let React render into that.
    override render() {
        if (this.#mount.parentNode == null) {
            return html`${this.#mount}`;
        }

        return nothing;
    }

    override updated(): void {
        // The shadow root and mount div exist after the first Lit render; create the React root then.
        if (!this.#root && this.#mount.isConnected) {
            this.#root = createRoot(this.#mount);
            this.#render();
        }
    }

    // Re-render when the shared authenticated fetch becomes available / changes (mixin hook).
    authFetchChanged(_authFetch: AuthFetch | null): void {
        this.#render();
    }

    #getContent = (): UmbDocumentDetailModel | undefined => {
        return this.#workspaceContext?.getData();
    };

    #render(): void {
        this.#root?.render(
            createElement(PlatformsPreviewApp, {
                unique: this.#unique,
                getContent: this.#getContent,
                authFetch: this.authFetch,
            }),
        );
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
