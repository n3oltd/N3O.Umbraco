import { customElement } from '@umbraco-cms/backoffice/external/lit';
import {
    UmbPropertyValueChangeEvent,
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { ImportDataEditorApp, type ImportDataValue } from './import-data-editor-app';

const elementName = 'n3o-import-data-editor';

// Web-component SHELL for the import data property editor. Umbraco's backoffice only loads custom
// elements, so this thin element owns the Umbraco contract (value/config + UmbPropertyValueChangeEvent)
// and the file-upload side effects, and mounts the React UI (ImportDataEditorApp) into its shadow root.
// React is NOT bundled here — it is external and resolved at runtime from the shared N3O.Umbraco.React
// import map. The host holds the single source of truth for `value`.
@customElement(elementName)
export class N3oImportDataEditorElement extends HTMLElement implements UmbPropertyEditorUiElement {
    #root?: Root;
    #mount: HTMLDivElement;
    #value: ImportDataValue | undefined = undefined;
    // Config is set by Umbraco (UmbPropertyEditorConfigCollection); unused by this editor but accepted
    // so the platform can assign it without warnings.
    #config: UmbPropertyEditorConfigCollection | undefined = undefined;

    constructor() {
        super();
        const shadow = this.attachShadow({ mode: 'open' });
        this.#mount = document.createElement('div');
        shadow.appendChild(this.#mount);
    }

    get value(): ImportDataValue | undefined {
        return this.#value;
    }

    set value(value: ImportDataValue | undefined) {
        this.#value = value;
        this.#render();
    }

    public set config(config: UmbPropertyEditorConfigCollection | undefined) {
        this.#config = config;
    }

    public get config(): UmbPropertyEditorConfigCollection | undefined {
        return this.#config;
    }

    connectedCallback(): void {
        this.#root ??= createRoot(this.#mount);
        this.#render();
    }

    disconnectedCallback(): void {
        this.#root?.unmount();
        this.#root = undefined;
    }

    #onTextChange(index: number, value: string): void {
        if (!this.#value) {
            return;
        }
        this.#value.fields[index].value = value;
        this.#dispatchChange();
    }

    async #uploadResource(index: number, file: File): Promise<void> {
        if (!this.#value) {
            return;
        }

        const reference = this.#value.reference;
        const storageToken = await this.#getStorageToken(file);

        const req = { file: storageToken };

        const res = await fetch(`/umbraco/backoffice/api/Imports/queued/${reference}/files`, {
            method: 'POST',
            headers: {
                'Accept': '*/*',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(req),
        });

        if (res.status === 200) {
            this.#value.fields[index].value = file.name;
            this.#render();
            this.#dispatchChange();
        } else {
            alert('Failed to upload specified file, please contact support for assistance');
        }
    }

    async #getStorageToken(file: File): Promise<unknown> {
        const data = new FormData();
        data.append('file', file);

        const res = await fetch('/umbraco/api/Storage/tempUpload', {
            method: 'POST',
            body: data,
        });

        return await res.json();
    }

    #dispatchChange(): void {
        this.dispatchEvent(new UmbPropertyValueChangeEvent());
    }

    #render(): void {
        this.#root?.render(
            createElement(ImportDataEditorApp, {
                value: this.#value,
                onTextChange: (index: number, value: string) => this.#onTextChange(index, value),
                onFileSelected: (index: number, file: File) => void this.#uploadResource(index, file),
            }),
        );
    }
}

export default N3oImportDataEditorElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oImportDataEditorElement;
    }
}
