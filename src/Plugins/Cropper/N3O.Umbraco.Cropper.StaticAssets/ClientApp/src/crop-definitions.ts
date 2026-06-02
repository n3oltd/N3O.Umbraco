import { LitElement, html, css, customElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import {
    UmbPropertyValueChangeEvent,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';

// Configuration UI for the Cropper property editor's `cropDefinitions` prevalue. Edits an array of
// crop definitions ({ label, alias, width, height, filters }) with add/delete, mirroring the original
// AngularJS prevalue editor.

interface CropDefinition {
    label: string;
    alias: string;
    width: number | null;
    height: number | null;
    filters: string | null;
}

const elementName = 'n3o-cropper-crop-definitions';

@customElement(elementName)
export class N3oCropperCropDefinitionsElement extends UmbElementMixin(LitElement) implements UmbPropertyEditorUiElement {
    #value: CropDefinition[] | null = null;

    @property({ type: Array })
    get value(): CropDefinition[] | null {
        return this.#value;
    }
    set value(v: CropDefinition[] | null | undefined) {
        const oldValue = this.#value;
        this.#value = v ?? null;
        this.requestUpdate('value', oldValue);
    }

    override connectedCallback(): void {
        super.connectedCallback();

        if (!this.#value) {
            this.#value = [];
            this.#addCropDefinition();
        }
    }

    #notify(): void {
        this.dispatchEvent(new UmbPropertyValueChangeEvent());
    }

    #addCropDefinition(): void {
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

    #deleteCropDefinition(index: number): void {
        this.#value = (this.#value ?? []).filter((_: CropDefinition, i: number) => i !== index);
        this.requestUpdate();
        this.#notify();
    }

    #updateField<K extends keyof CropDefinition>(index: number, field: K, value: CropDefinition[K]): void {
        (this.#value ?? [])[index][field] = value;
        this.#notify();
    }

    override render() {
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
                                    @input=${(e: Event) =>
                                        this.#updateField(index, 'label', (e.target as HTMLInputElement).value)} />
                            </td>
                        </tr>
                        <tr>
                            <td>Alias</td>
                            <td>
                                <input
                                    type="text"
                                    required
                                    .value=${cropDefinition.alias ?? ''}
                                    @input=${(e: Event) =>
                                        this.#updateField(index, 'alias', (e.target as HTMLInputElement).value)} />
                            </td>
                        </tr>
                        <tr>
                            <td>Width</td>
                            <td>
                                <input
                                    type="number"
                                    required
                                    .value=${cropDefinition.width ?? ''}
                                    @input=${(e: Event) => {
                                        const v = (e.target as HTMLInputElement).value;
                                        this.#updateField(index, 'width', v === '' ? null : Number(v));
                                    }} />
                            </td>
                        </tr>
                        <tr>
                            <td>Height&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <input
                                    type="number"
                                    required
                                    .value=${cropDefinition.height ?? ''}
                                    @input=${(e: Event) => {
                                        const v = (e.target as HTMLInputElement).value;
                                        this.#updateField(index, 'height', v === '' ? null : Number(v));
                                    }} />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">Filters</td>
                            <td>
                                <textarea
                                    cols="20"
                                    rows="6"
                                    .value=${cropDefinition.filters ?? ''}
                                    @input=${(e: Event) =>
                                        this.#updateField(
                                            index,
                                            'filters',
                                            (e.target as HTMLTextAreaElement).value
                                        )}></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <p>
                                    <a @click=${() => this.#deleteCropDefinition(index)} class="cursor delete"
                                        >Delete</a
                                    >
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

    static override styles = css`
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

export default N3oCropperCropDefinitionsElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oCropperCropDefinitionsElement;
    }
}
