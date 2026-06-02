import { LitElement, html, css, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbPropertyValueChangeEvent } from '@umbraco-cms/backoffice/property-editor';

const elementName = 'n3o-import-data-editor';

// Property editor for import data. Renders the list of fields stored in the value; each field has a
// text input (placeholder = sourceValue) and, when isFile is set, a file input that uploads the chosen
// file via the temp-upload endpoint and then attaches it to the queued import.
class N3oImportDataEditorElement extends UmbElementMixin(LitElement) {
    static properties = {
        value: { type: Object },
    };

    #value;

    get value() {
        return this.#value;
    }

    set value(value) {
        const oldValue = this.#value;
        this.#value = value;
        this.requestUpdate('value', oldValue);
    }

    // Set by Umbraco (UmbPropertyEditorConfigCollection); unused by this editor but accepted so the
    // platform can assign it without warnings.
    set config(config) {
        this._config = config;
    }

    get config() {
        return this._config;
    }

    #onTextInput(index, event) {
        this.#value.fields[index].value = event.target.value;
        this.#dispatchChange();
    }

    async #uploadResource(reference, index) {
        const uploadInput = this.shadowRoot.getElementById(`fileInput_${index}`);

        if (uploadInput.files.length === 0) {
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
            this.#value.fields[index].value = uploadInput.files[0].name;
            this.requestUpdate('value');
            this.#dispatchChange();
        } else {
            alert('Failed to upload specified file, please contact support for assistance');
        }
    }

    async #getStorageToken(uploadInput) {
        const data = new FormData();
        data.append('file', uploadInput.files[0]);

        const res = await fetch('/umbraco/api/Storage/tempUpload', {
            method: 'POST',
            body: data,
        });

        return await res.json();
    }

    #dispatchChange() {
        this.dispatchEvent(new UmbPropertyValueChangeEvent());
    }

    render() {
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
                                    @input=${(event) => this.#onTextInput(index, event)} />

                                ${field.isFile
                                    ? html`<input
                                          type="file"
                                          id=${`fileInput_${index}`}
                                          @change=${() => this.#uploadResource(this.#value.reference, index)} />`
                                    : nothing}
                            </div>
                        </div>
                    `
                )}
            </div>
        `;
    }

    static styles = css`
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

customElements.define(elementName, N3oImportDataEditorElement);

export default N3oImportDataEditorElement;
export { N3oImportDataEditorElement };
