import { customElement } from '@umbraco-cms/backoffice/external/lit';
import {
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { TextResourceEditorApp, type TextResourceEntry } from './text-resource-editor-app';

const elementName = 'n3o-text-resource-editor';

@customElement(elementName)
export class N3oTextResourceEditorElement extends HTMLElement implements UmbPropertyEditorUiElement {
    #root?: Root;
    #mount: HTMLDivElement;
    #value: TextResourceEntry[] = [];

    constructor() {
        super();
        const shadow = this.attachShadow({ mode: 'open' });
        this.#mount = document.createElement('div');
        shadow.appendChild(this.#mount);
    }

    get value(): TextResourceEntry[] {
        return this.#value;
    }

    set value(value: TextResourceEntry[] | undefined) {
        this.#value = Array.isArray(value) ? value : [];
        this.#render();
    }

    public set config(_config: UmbPropertyEditorConfigCollection | undefined) {}

    connectedCallback(): void {
        this.#root ??= createRoot(this.#mount);
        this.#render();
    }

    disconnectedCallback(): void {
        this.#root?.unmount();
        this.#root = undefined;
    }

    #render(): void {
        this.#root?.render(
            createElement(TextResourceEditorApp, {
                value: this.#value,
                onChange: (value: TextResourceEntry[]) => {
                    this.#value = value;
                    this.dispatchEvent(new UmbChangeEvent());
                },
            }),
        );
    }
}

export default N3oTextResourceEditorElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oTextResourceEditorElement;
    }
}
