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

@customElement(elementName)
export class N3oCellsElement extends HTMLElement implements UmbPropertyEditorUiElement {
    #root?: Root;
    #mount: HTMLDivElement;
    #value: CellsValue;
    #gridConfiguration: Record<string, unknown> = {};
    #gridConfigurationJson?: string;

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

        // Reparsing would hand the grid a new object every time and rebuild it, losing selection and scroll.
        if (gridConfiguration !== this.#gridConfigurationJson) {
            this.#gridConfigurationJson = gridConfiguration;

            try {
                this.#gridConfiguration = gridConfiguration
                    ? (JSON.parse(gridConfiguration) as Record<string, unknown>)
                    : {};
            } catch {
                this.#gridConfiguration = {};
                console.error('[Cells] gridConfiguration is not valid JSON; falling back to an empty grid.');
            }
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
