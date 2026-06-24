import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { UMB_NOTIFICATION_CONTEXT, type UmbNotificationContext } from '@umbraco-cms/backoffice/notification';
import { UmbAuthFetchMixin } from '@n3oltd/backoffice-core';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { DataExportApp } from './data-export-app';
import type { AuthFetch } from '@n3oltd/backoffice-core';
import type { Notify } from './types';

const elementName = 'n3o-data-export';

@customElement(elementName)
export class N3oDataExportElement extends UmbAuthFetchMixin(UmbElementMixin(HTMLElement)) {
    #root?: Root;
    #mount: HTMLDivElement;
    #contentKey: string | null = null;
    #notificationContext?: UmbNotificationContext;
    #notify: Notify = (color, headline, message) => {
        this.#notificationContext?.peek(color, { data: { headline, message } });
    };

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
            createElement(DataExportApp, {
                contentKey: this.#contentKey,
                authFetch: this.authFetch,
                notify: this.#notify,
            }),
        );
    }
}

export default N3oDataExportElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oDataExportElement;
    }
}
