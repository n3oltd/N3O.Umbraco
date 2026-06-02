import { LitElement, css, customElement, html, unsafeCSS } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import {
    UmbPropertyValueChangeEvent,
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import Handsontable from 'handsontable';
// Import Handsontable CSS as an inlined string so Vite bundles it into the JS output
// rather than emitting a separate .css file. This mirrors the original shadow-root <link> approach:
// the styles are scoped to the component and loaded without a separate network request.
import handsontableStyles from 'handsontable/dist/handsontable.full.min.css?inline';

const elementName = 'n3o-cells';

// Property editor that wraps Handsontable. The stored value is a 2D array (JSON). The data type
// configuration carries a `gridConfiguration` JSON string describing the grid (columns, default data, etc.).
@customElement(elementName)
export class N3oCellsElement extends UmbElementMixin(LitElement) implements UmbPropertyEditorUiElement {
    #value: unknown[][] | undefined;
    #config: UmbPropertyEditorConfigCollection | undefined;
    #hot: Handsontable | undefined;

    get value(): unknown[][] | undefined {
        return this.#value;
    }

    set value(value: unknown[][] | undefined) {
        const oldValue = this.#value;
        this.#value = value;
        this.requestUpdate('value', oldValue);
    }

    // Set by Umbraco as a UmbPropertyEditorConfigCollection.
    public set config(config: UmbPropertyEditorConfigCollection | undefined) {
        this.#config = config;
    }

    public get config(): UmbPropertyEditorConfigCollection | undefined {
        return this.#config;
    }

    override firstUpdated(): void {
        const container = this.shadowRoot?.querySelector('#grid') as HTMLElement | null;

        if (!container) {
            return;
        }

        // gridConfiguration is a JSON string stored as a prevalue on the data type.
        const localConfig = JSON.parse(this.#config?.getValueByAlias('gridConfiguration') ?? '{}') as Record<string, unknown>;

        let data = this.#value;

        if (!data) {
            data = localConfig.data as unknown[][] | undefined;
        }

        const globalConfig: Handsontable.GridSettings = {
            licenseKey: 'non-commercial-and-evaluation',
            height: 'auto',
            width: 'auto',
            data: data,
            afterChange: (_change: Handsontable.CellChange[] | null, source: Handsontable.ChangeSource) => {
                if (source !== 'loadData') {
                    this.#value = this.#hot?.getData() as unknown[][];
                    this.dispatchEvent(new UmbPropertyValueChangeEvent());
                }
            },
        };

        this.#hot = new Handsontable(container, { ...localConfig, ...globalConfig });
    }

    override disconnectedCallback(): void {
        super.disconnectedCallback();

        if (this.#hot) {
            this.#hot.destroy();
            this.#hot = undefined;
        }
    }

    override render() {
        return html`
            <div id="grid"></div>
        `;
    }

    static override styles = [
        // Handsontable CSS is inlined into the bundle and injected into the shadow root,
        // matching the original behaviour of the <link> tag in the render() template.
        unsafeCSS(handsontableStyles),
        css`
            :host {
                display: block;
                width: 100%;
            }
        `,
    ];
}

export default N3oCellsElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oCellsElement;
    }
}
