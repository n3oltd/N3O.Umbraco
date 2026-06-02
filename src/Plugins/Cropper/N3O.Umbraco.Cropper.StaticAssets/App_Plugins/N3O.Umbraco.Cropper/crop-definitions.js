import { LitElement, html, css } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbPropertyValueChangeEvent } from '@umbraco-cms/backoffice/property-editor';

// Configuration UI for the Cropper property editor's `cropDefinitions` prevalue. Edits an array of
// crop definitions ({ label, alias, width, height, filters }) with add/delete, mirroring the original
// AngularJS prevalue editor.
class N3oCropperCropDefinitionsElement extends UmbElementMixin(LitElement) {
    static properties = {
        value: { type: Array },
    };

    #value;

    get value() {
        return this.#value;
    }

    set value(v) {
        const oldValue = this.#value;
        this.#value = v;
        this.requestUpdate('value', oldValue);
    }

    connectedCallback() {
        super.connectedCallback();

        if (!this.#value) {
            this.#value = [];
            this.#addCropDefinition();
        }
    }

    #notify() {
        this.dispatchEvent(new UmbPropertyValueChangeEvent());
    }

    #addCropDefinition() {
        this.#value = [
            ...(this.#value ?? []),
            {
                label: '',
                alias: '',
                width: null,
                height: null,
                filters: null,
            },
        ];

        this.requestUpdate();
        this.#notify();
    }

    #deleteCropDefinition(index) {
        this.#value = this.#value.filter((_, i) => i !== index);
        this.requestUpdate();
        this.#notify();
    }

    #updateField(index, field, value) {
        this.#value[index][field] = value;
        this.#notify();
    }

    render() {
        return html`
            ${(this.#value ?? []).map(
                (cropDefinition, index) => html`
                    <table>
                        <tr>
                            <td>Label</td>
                            <td>
                                <input
                                    type="text"
                                    required
                                    .value=${cropDefinition.label ?? ''}
                                    @input=${(e) => this.#updateField(index, 'label', e.target.value)} />
                            </td>
                        </tr>
                        <tr>
                            <td>Alias</td>
                            <td>
                                <input
                                    type="text"
                                    required
                                    .value=${cropDefinition.alias ?? ''}
                                    @input=${(e) => this.#updateField(index, 'alias', e.target.value)} />
                            </td>
                        </tr>
                        <tr>
                            <td>Width</td>
                            <td>
                                <input
                                    type="number"
                                    required
                                    .value=${cropDefinition.width ?? ''}
                                    @input=${(e) =>
                                        this.#updateField(index, 'width', e.target.value === '' ? null : Number(e.target.value))} />
                            </td>
                        </tr>
                        <tr>
                            <td>Height&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <input
                                    type="number"
                                    required
                                    .value=${cropDefinition.height ?? ''}
                                    @input=${(e) =>
                                        this.#updateField(index, 'height', e.target.value === '' ? null : Number(e.target.value))} />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">Filters</td>
                            <td>
                                <textarea
                                    cols="20"
                                    rows="6"
                                    .value=${cropDefinition.filters ?? ''}
                                    @input=${(e) => this.#updateField(index, 'filters', e.target.value)}></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <p>
                                    <a @click=${() => this.#deleteCropDefinition(index)} class="cursor delete">Delete</a>
                                </p>
                            </td>
                        </tr>
                    </table>
                    <hr />
                `
            )}

            <p>
                <br />
                <a @click=${() => this.#addCropDefinition()} class="cursor add">Add</a>
            </p>
        `;
    }

    static styles = css`
        :host {
            display: block;
        }

        .cursor {
            cursor: pointer;
            text-decoration: none;
        }

        .delete {
            color: #ff0000;
        }

        .add {
            font-weight: bold;
        }

        hr {
            color: #666;
        }
    `;
}

customElements.define('n3o-cropper-crop-definitions', N3oCropperCropDefinitionsElement);

export default N3oCropperCropDefinitionsElement;
export { N3oCropperCropDefinitionsElement };
