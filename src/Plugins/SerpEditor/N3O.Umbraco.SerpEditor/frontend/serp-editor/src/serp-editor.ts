import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import {
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import { UmbAuthFetchMixin } from '@n3oltd/backoffice-core';
import type { AuthFetch } from '@n3oltd/backoffice-core';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { SerpEditorApp, type SerpValue } from './serp-editor-app';

const elementName = 'n3o-serp-editor';

@customElement(elementName)
export class N3oSerpEditorElement extends UmbAuthFetchMixin(UmbElementMixin(HTMLElement)) implements UmbPropertyEditorUiElement {
    #root?: Root;
    #mount: HTMLDivElement;
    #value: SerpValue = { title: '', description: '' };
    #maxCharsTitle = 60;
    #maxCharsDescription = 160;

    constructor() {
        super();
        const shadow = this.attachShadow({ mode: 'open' });
        this.#mount = document.createElement('div');
        shadow.appendChild(this.#mount);
    }

    get value(): SerpValue {
        return this.#value;
    }

    set value(value: SerpValue | undefined) {
        this.#value = value ?? { title: '', description: '' };
        this.#render();
    }

    public set config(config: UmbPropertyEditorConfigCollection | undefined) {
        const maxCharsTitle = Number.parseInt(String(config?.getValueByAlias('maxCharsTitle') ?? ''), 10);
        const maxCharsDescription = Number.parseInt(String(config?.getValueByAlias('maxCharsDescription') ?? ''),
                                                    10);

        if (!Number.isNaN(maxCharsTitle) && maxCharsTitle > 0) {
            this.#maxCharsTitle = maxCharsTitle;
        }

        if (!Number.isNaN(maxCharsDescription) && maxCharsDescription > 0) {
            this.#maxCharsDescription = maxCharsDescription;
        }

        this.#render();
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
            createElement(SerpEditorApp, {
                value: this.#value,
                maxCharsTitle: this.#maxCharsTitle,
                maxCharsDescription: this.#maxCharsDescription,
                authFetch: this.authFetch,
                onChange: (value: SerpValue) => {
                    this.#value = value;
                    this.dispatchEvent(new UmbChangeEvent());
                },
            }),
        );
    }
}

export default N3oSerpEditorElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oSerpEditorElement;
    }
}
