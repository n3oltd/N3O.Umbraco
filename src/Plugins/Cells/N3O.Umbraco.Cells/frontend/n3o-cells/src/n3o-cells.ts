import { customElement } from '@umbraco-cms/backoffice/external/lit';
import {
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { N3oCellsApp, type CellsValue } from './n3o-cells-app';

const elementName = 'n3o-cells';

// Umbraco's backoffice loads custom elements only, which is why the React UI needs this shell. React
// is external, resolved at runtime from the shared N3O.Umbraco.ReactRuntime import map; Handsontable
// is bundled, since only React is shared.
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

    public set config(config: UmbPropertyEditorConfigCollection | undefined) {
        const gridConfiguration = config?.getValueByAlias<string>('gridConfiguration');

        // The setting is a free-text area, so an editor can save it blank or with malformed JSON.
        // An unusable grid configuration must not take the whole property editor down with it.
        try {
            this.#gridConfiguration = gridConfiguration
                ? (JSON.parse(gridConfiguration) as Record<string, unknown>)
                : {};
        } catch {
            this.#gridConfiguration = {};
            console.error('[Cells] gridConfiguration is not valid JSON; falling back to an empty grid.');
        }

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
                    this.dispatchEvent(new UmbChangeEvent());
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
