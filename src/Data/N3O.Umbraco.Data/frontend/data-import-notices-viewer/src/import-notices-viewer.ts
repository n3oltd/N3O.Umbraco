import { customElement } from '@umbraco-cms/backoffice/external/lit';
import {
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { ImportNoticesViewerApp, type ImportNoticesValue } from './import-notices-viewer-app';

const elementName = 'n3o-import-notices-viewer';

@customElement(elementName)
export class N3oImportNoticesViewerElement extends HTMLElement implements UmbPropertyEditorUiElement {
    #root?: Root;
    #mount: HTMLDivElement;
    #value: ImportNoticesValue | undefined = undefined;

    constructor() {
        super();
        const shadow = this.attachShadow({ mode: 'open' });
        this.#mount = document.createElement('div');
        shadow.appendChild(this.#mount);
    }

    get value(): ImportNoticesValue | undefined {
        return this.#value;
    }

    set value(v: ImportNoticesValue | undefined) {
        this.#value = v;
        this.#render();
    }

    public set config(_config: UmbPropertyEditorConfigCollection | undefined) { }

    public get config(): UmbPropertyEditorConfigCollection | undefined {
        return undefined;
    }

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
            createElement(ImportNoticesViewerApp, {
                value: this.#value,
            }),
        );
    }
}

export default N3oImportNoticesViewerElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oImportNoticesViewerElement;
    }
}
