import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { UmbAuthFetchMixin } from '@n3o/backoffice-core';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { DataImportApp } from './data-import-app';
import type { AuthFetch } from '@n3o/backoffice-core';

const elementName = 'n3o-data-import';

// Web-component SHELL for the CSV/ZIP import workspace view. Umbraco's backoffice only loads custom
// elements; this thin element keeps a Lit base (UmbElementMixin) ONLY for context plumbing — it consumes
// UMB_DOCUMENT_WORKSPACE_CONTEXT to obtain the current document key (`unique`) and passes it as a prop
// into the React UI (DataImportApp), which renders the multi-step form and talks to the backend. React
// is NOT bundled here — it is external and resolved at runtime from the shared N3O.Umbraco.ReactRuntime
// import map. The authenticated fetch is likewise shared: UmbAuthFetchMixin (from @n3o/backoffice-core) gives
// us `this.authFetch` (rebuilt from UMB_AUTH_CONTEXT) with no per-plugin auth boilerplate.
@customElement(elementName)
export class N3oDataImportElement extends UmbAuthFetchMixin(UmbElementMixin(HTMLElement)) {
    #root?: Root;
    #mount: HTMLDivElement;
    #contentKey: string | null = null;

    constructor() {
        super();

        const shadow = this.attachShadow({ mode: 'open' });
        this.#mount = document.createElement('div');
        shadow.appendChild(this.#mount);

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            if (!context) {
                return;
            }

            this.observe(
                context.unique,
                (unique) => {
                    if (unique && unique !== this.#contentKey) {
                        this.#contentKey = unique;
                        this.#render();
                    }
                },
                '_observeUnique'
            );
        });
    }

    // Re-render when the shared authenticated fetch becomes available / changes (mixin hook).
    authFetchChanged(_authFetch: AuthFetch | null): void {
        this.#render();
    }

    connectedCallback(): void {
        super.connectedCallback?.();
        this.#root ??= createRoot(this.#mount);
        this.#render();
    }

    disconnectedCallback(): void {
        super.disconnectedCallback?.();
        this.#root?.unmount();
        this.#root = undefined;
    }

    #render(): void {
        this.#root?.render(
            createElement(DataImportApp, {
                contentKey: this.#contentKey,
                authFetch: this.authFetch,
            }),
        );
    }
}

export default N3oDataImportElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oDataImportElement;
    }
}
