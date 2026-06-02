import { LitElement, html, css } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbPropertyValueChangeEvent } from '@umbraco-cms/backoffice/property-editor';
// Handsontable is a UMD bundle; importing it attaches `Handsontable` to the global (window).
import './handsontable.full.min.js';

const elementName = 'n3o-cells';

// Property editor that wraps Handsontable. The stored value is a 2D array (JSON). The data type
// configuration carries a `gridConfiguration` JSON string describing the grid (columns, default data, etc.).
class N3oCellsElement extends UmbElementMixin(LitElement) {
    static properties = {
        value: { type: Array },
    };

    #value;
    #config;
    #hot;

    get value() {
        return this.#value;
    }

    set value(value) {
        const oldValue = this.#value;
        this.#value = value;
        this.requestUpdate('value', oldValue);
    }

    // Set by Umbraco as a UmbPropertyEditorConfigCollection.
    set config(config) {
        this.#config = config;
    }

    get config() {
        return this.#config;
    }

    firstUpdated() {
        const container = this.shadowRoot.querySelector('#grid');

        if (!container || typeof Handsontable === 'undefined') {
            return;
        }

        const localConfig = JSON.parse(this.#config?.getValueByAlias('gridConfiguration') ?? '{}');

        let data = this.#value;

        if (!data) {
            data = localConfig.data;
        }

        const globalConfig = {
            licenseKey: 'non-commercial-and-evaluation',
            height: 'auto',
            width: 'auto',
            data: data,
            afterChange: (change, source) => {
                if (source !== 'loadData') {
                    this.#value = this.#hot.getData();
                    this.dispatchEvent(new UmbPropertyValueChangeEvent());
                }
            },
        };

        this.#hot = new Handsontable(container, { ...localConfig, ...globalConfig });
    }

    disconnectedCallback() {
        super.disconnectedCallback();

        if (this.#hot) {
            this.#hot.destroy();
            this.#hot = undefined;
        }
    }

    render() {
        return html`
            <link rel="stylesheet" href="/App_Plugins/N3O.Umbraco.Cells/handsontable.full.min.css" />
            <div id="grid"></div>
        `;
    }

    static styles = css`
        :host {
            display: block;
            width: 100%;
        }
    `;
}

customElements.define(elementName, N3oCellsElement);

export default N3oCellsElement;
export { N3oCellsElement };
