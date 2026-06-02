import { LitElement, css, customElement, html, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import {
    UmbPropertyValueChangeEvent,
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';

const elementName = 'n3o-import-data-editor';

interface ImportField {
    name: string;
    value: string | null;
    sourceValue: string | null;
    isFile: boolean;
}

interface ImportDataValue {
    reference: string;
    fields: ImportField[];
}

// Property editor for import data. Renders the list of fields stored in the value; each field has a
// text input (placeholder = sourceValue) and, when isFile is set, a file input that uploads the chosen
// file via the temp-upload endpoint and then attaches it to the queued import.
@customElement(elementName)
export class N3oImportDataEditorElement extends UmbElementMixin(LitElement) implements UmbPropertyEditorUiElement {
    #value: ImportDataValue | undefined = undefined;
    // Config is set by Umbraco (UmbPropertyEditorConfigCollection); unused by this editor but accepted
    // so the platform can assign it without warnings.
    #config: UmbPropertyEditorConfigCollection | undefined = undefined;

    get value(): ImportDataValue | undefined {
        return this.#value;
    }

    set value(value: ImportDataValue | undefined) {
        const oldValue = this.#value;
        this.#value = value;
        this.requestUpdate('value', oldValue);
    }

    public set config(config: UmbPropertyEditorConfigCollection | undefined) {
        this.#config = config;
    }

    public get config(): UmbPropertyEditorConfigCollection | undefined {
        return this.#config;
    }

    #onTextInput(index: number, event: Event): void {
        if (!this.#value) {
            return;
        }
        this.#value.fields[index].value = (event.target as HTMLInputElement).value;
        this.#dispatchChange();
    }

    async #uploadResource(reference: string, index: number): Promise<void> {
        const uploadInput = this.shadowRoot?.getElementById(`fileInput_${index}`) as HTMLInputElement | null;

        if (!uploadInput || !uploadInput.files || uploadInput.files.length === 0) {
            return;
        }

        const storageToken = await this.#getStorageToken(uploadInput);

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
            this.#value!.fields[index].value = uploadInput.files[0].name;
            this.requestUpdate('value');
            this.#dispatchChange();
        } else {
            alert('Failed to upload specified file, please contact support for assistance');
        }
    }

    async #getStorageToken(uploadInput: HTMLInputElement): Promise<unknown> {
        const data = new FormData();
        data.append('file', uploadInput.files![0]);

        const res = await fetch('/umbraco/api/Storage/tempUpload', {
            method: 'POST',
            body: data,
        });

        return await res.json();
    }

    #dispatchChange(): void {
        this.dispatchEvent(new UmbPropertyValueChangeEvent());
    }

    override render() {
        const fields = this.#value?.fields ?? [];

        return html`
            <div class="n3o-import-fields-editor">
                ${fields.map(
                    (field, index) => html`
                        <div class="row-wrapper">
                            <div class="row-1">
                                <span class="text">${field.name}</span>
                            </div>

                            <div class="row-2">
                                <input
                                    type="text"
                                    class="custom"
                                    .value=${field.value ?? ''}
                                    placeholder=${field.sourceValue ?? ''}
                                    @input=${(event: Event) => this.#onTextInput(index, event)} />

                                ${field.isFile
                                    ? html`<input
                                          type="file"
                                          id=${`fileInput_${index}`}
                                          @change=${() => void this.#uploadResource(this.#value!.reference, index)} />`
                                    : nothing}
                            </div>
                        </div>
                    `
                )}
            </div>
        `;
    }

    static override styles = css`
        .n3o-import-fields-editor .row-wrapper {
            margin-bottom: 40px;
            width: 100%;
        }

        .n3o-import-fields-editor .row-1 {
            display: block;
            width: 90%;
        }

        .n3o-import-fields-editor .row-2 {
            display: block;
            width: 90%;
        }

        .n3o-import-fields-editor .text {
            font-weight: bold;
        }

        .n3o-import-fields-editor .custom {
            width: 100%;
            margin-top: 10px;
        }
    `;
}

export default N3oImportDataEditorElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oImportDataEditorElement;
    }
}
