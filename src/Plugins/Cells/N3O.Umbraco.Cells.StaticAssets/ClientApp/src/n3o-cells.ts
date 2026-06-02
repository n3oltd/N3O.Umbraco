import { customElement } from '@umbraco-cms/backoffice/external/lit';
import {
    UmbPropertyValueChangeEvent,
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { N3oCellsApp, type CellsValue } from './n3o-cells-app';

const elementName = 'n3o-cells';

// Web-component SHELL for the Cells (Handsontable) property editor. Umbraco's backoffice only loads
// custom elements, so this thin element owns the Umbraco contract (value/config +
// UmbPropertyValueChangeEvent) and mounts the React UI (N3oCellsApp) into its shadow root.
// React itself is NOT bundled here — it is external and resolved at runtime from the shared
// N3O.Umbraco.React import map. Handsontable IS bundled (it is not React).
//
// The stored value is a 2D array (JSON). The data type configuration carries a `gridConfiguration`
// JSON string describing the grid (columns, default data, etc.).
@customElement(elementName)
export class N3oCellsElement extends HTMLElement implements UmbPropertyEditorUiElement {
    #root?: Root;
    #mount: HTMLDivElement;
    #value: CellsValue;
    #gridConfiguration: Record<string, unknown> = {};

    constructor() {
        super();
        const shadow = this.attachShadow({ mode: 'open' });
        const style = document.createElement('style');
        style.textContent = ':host { display: block; width: 100%; }';
        shadow.appendChild(style);
        this.#mount = document.createElement('div');
        shadow.appendChild(this.#mount);
    }

    get value(): CellsValue {
        return this.#value;
    }

    set value(value: CellsValue) {
        this.#value = value;
        this.#render();
    }

    // Config (prevalues) arrives as UmbPropertyEditorConfigCollection. `gridConfiguration` is a JSON
    // string stored as a prevalue on the data type.
    public set config(config: UmbPropertyEditorConfigCollection | undefined) {
        this.#gridConfiguration = JSON.parse(
            config?.getValueByAlias('gridConfiguration') ?? '{}',
        ) as Record<string, unknown>;
        this.#render();
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
            createElement(N3oCellsApp, {
                value: this.#value,
                gridConfiguration: this.#gridConfiguration,
                onChange: (value: unknown[][]) => {
                    this.#value = value;
                    this.dispatchEvent(new UmbPropertyValueChangeEvent());
                },
            }),
        );
    }
}

export default N3oCellsElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oCellsElement;
    }
}
