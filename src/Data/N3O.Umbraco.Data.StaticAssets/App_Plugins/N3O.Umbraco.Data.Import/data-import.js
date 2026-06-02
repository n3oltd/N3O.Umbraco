import { LitElement, html, css, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';

const elementName = 'n3o-data-import';

// Multi-step CSV/ZIP import form, ported from the AngularJS "Import" content app.
// Steps: choose content type, select importable properties, download a template,
// upload CSV (+ optional ZIP assets) and queue the import.
class N3oDataImportElement extends UmbElementMixin(LitElement) {
    static properties = {
        _show: { state: true },
        _processing: { state: true },
        _contentTypes: { state: true },
        _contentType: { state: true },
        _datePatterns: { state: true },
        _datePattern: { state: true },
        _moveUpdatedContentToCurrentLocation: { state: true },
        _importableProperties: { state: true },
        _errorMessages: { state: true },
    };

    #contentKey;

    constructor() {
        super();

        this.#startOver();
        this._contentTypes = [];
        this._datePatterns = [];
        this._importableProperties = [];
        this._moveUpdatedContentToCurrentLocation = false;

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            if (!context) {
                return;
            }

            this.observe(
                context.unique,
                (unique) => {
                    if (unique && unique !== this.#contentKey) {
                        this.#contentKey = unique;
                        this.#init();
                    }
                },
                '_observeUnique'
            );
        });
    }

    #startOver() {
        this._processing = false;
        this._contentType = null;
        this._errorMessages = null;
        this._importableProperties = [];
        this._show = 'form';
    }

    async #init() {
        this._contentTypes = await this.#getContentTypes(this.#contentKey);

        const res = await fetch('/umbraco/backoffice/api/Imports/lookups/datePatterns', {
            headers: { Accept: 'application/json' },
        });
        const datePatterns = await res.json();

        this._datePatterns = datePatterns;
        this._datePattern = datePatterns[0];
    }

    async #getContentTypes(contentId) {
        const res = await fetch(`/umbraco/api/ContentTypes/${contentId}/relations?type=child`, {
            headers: { Accept: 'application/json' },
        });

        return await res.json();
    }

    async #refreshProperties() {
        if (!this._contentType) {
            this._importableProperties = [];
            return;
        }

        const res = await fetch(`/umbraco/backoffice/api/Imports/importableProperties/${this._contentType.alias}`, {
            headers: { Accept: 'application/json' },
        });
        const properties = await res.json();

        for (const property of properties) {
            property.selected = false;
        }

        this._importableProperties = properties;
    }

    #onContentTypeChange(event) {
        const alias = event.target.value;
        this._contentType = this._contentTypes.find((x) => x.alias === alias) ?? null;
        this.#refreshProperties();
    }

    #onDatePatternChange(event) {
        const id = event.target.value;
        this._datePattern = this._datePatterns.find((x) => x.id === id) ?? null;
    }

    #onMoveChange(event) {
        this._moveUpdatedContentToCurrentLocation = event.target.checked;
    }

    #onPropertyToggle(property, event) {
        property.selected = event.target.checked;
        this.requestUpdate();
    }

    #selectAllProperties() {
        for (const property of this._importableProperties) {
            property.selected = true;
        }
        this.requestUpdate();
    }

    #clearSelectedProperties() {
        for (const property of this._importableProperties) {
            property.selected = false;
        }
        this.requestUpdate();
    }

    async #getTemplate() {
        const selectedPropertyAliases = this._importableProperties.filter((x) => x.selected).map((x) => x.alias);

        if (!selectedPropertyAliases.length) {
            this.#processingError('At least one property must be selected');
            return;
        }

        const req = { properties: selectedPropertyAliases };

        const getTemplate = await fetch(`/umbraco/backoffice/api/Imports/template/${this._contentType.alias}`, {
            method: 'POST',
            headers: {
                Accept: '*/*',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(req),
        });

        const blob = await getTemplate.blob();
        const header = getTemplate.headers.get('Content-Disposition');
        const parts = header.split(';');
        const filename = parts[1].split('=')[1].replaceAll('"', '');

        const newBlob = new Blob([blob]);
        const blobUrl = window.URL.createObjectURL(newBlob);
        const link = document.createElement('a');
        link.href = blobUrl;
        link.setAttribute('download', filename);
        document.body.appendChild(link);
        link.click();
        link.parentNode.removeChild(link);
        window.URL.revokeObjectURL(blobUrl);
    }

    async #import() {
        this._processing = true;

        const csvFile = this.renderRoot.getElementById('csvFile');
        const zipFile = this.renderRoot.getElementById('zipFile');

        if (!csvFile.value || csvFile.value.split('.')[1].toLowerCase() != 'csv') {
            this.#processingError('A valid CSV file must be specified');
            return;
        }

        if (zipFile.value && zipFile.value.split('.')[1].toLowerCase() != 'zip') {
            this.#processingError('The selected file is not a valid ZIP file');
            return;
        }

        const csvStorageToken = await this.#getStorageToken(csvFile);
        const zipStorageToken = await this.#getStorageToken(zipFile);

        const req = {
            datePattern: this._datePattern.id,
            moveUpdatedContentToCurrentLocation: this._moveUpdatedContentToCurrentLocation,
            csvFile: csvStorageToken,
            zipFile: zipStorageToken,
        };

        const result = await fetch(
            `/umbraco/backoffice/api/Imports/queue/${this.#contentKey}/${this._contentType.alias}`,
            {
                method: 'POST',
                headers: {
                    Accept: '*/*',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(req),
            }
        );

        if (result.status === 200) {
            this._show = 'success';
            this._processing = false;
        } else {
            this.#processingError(await result.json());
        }
    }

    async #getStorageToken(input) {
        if (input.files.length === 0) {
            return null;
        }

        const data = new FormData();
        data.append('file', input.files[0]);

        const res = await fetch('/umbraco/api/Storage/tempUpload', {
            method: 'POST',
            body: data,
        });

        return await res.json();
    }

    #processingError(messages) {
        if (!Array.isArray(messages)) {
            messages = [messages];
        }

        this._processing = false;
        this._errorMessages = messages;
        this._show = 'error';
    }

    #renderForm() {
        return html`
            <div class="umb-group-panel">
                <div class="umb-group-panel__header">Options</div>

                <div class="umb-group-panel__content">
                    <div class="control-group">
                        <label>Content Type <strong class="required">*</strong></label>
                        <select @change=${this.#onContentTypeChange} ?disabled=${this._processing}>
                            <option value="" ?selected=${!this._contentType}></option>
                            ${this._contentTypes.map(
                                (item) => html`<option value=${item.alias}>${item.name}</option>`
                            )}
                        </select>
                    </div>

                    <div class="control-group">
                        <label>Date Pattern <strong class="required">*</strong></label>
                        <select @change=${this.#onDatePatternChange} ?disabled=${this._processing}>
                            ${this._datePatterns.map(
                                (item) => html`<option value=${item.id}>${item.name}</option>`
                            )}
                        </select>
                    </div>

                    <div class="control-group">
                        <label>Move Updated Content to Current Location</label>
                        <input
                            type="checkbox"
                            .checked=${this._moveUpdatedContentToCurrentLocation}
                            @change=${this.#onMoveChange}
                            ?disabled=${this._processing} />
                    </div>

                    <div class="control-group">
                        <label>CSV File <strong class="required">*</strong></label>
                        <input type="file" id="csvFile" ?disabled=${this._processing} />
                    </div>

                    <div class="control-group">
                        <label>ZIP Assets File (optional)</label>
                        <input type="file" id="zipFile" ?disabled=${this._processing} />
                    </div>
                </div>
            </div>

            ${this._contentType
                ? html`
                      <div class="umb-group-panel">
                          <div class="umb-group-panel__header">Properties</div>

                          <div class="umb-group-panel__content">
                              <div class="listTable">
                                  <a class="link" @click=${this.#selectAllProperties}>Select All</a> |
                                  <a class="link" @click=${this.#clearSelectedProperties}>Clear Selection</a>

                                  <ul class="selectionCheckBoxes">
                                      ${this._importableProperties.map(
                                          (property) => html`
                                              <li>
                                                  <label>
                                                      <input
                                                          type="checkbox"
                                                          .value=${property.alias}
                                                          .checked=${!!property.selected}
                                                          @change=${(e) => this.#onPropertyToggle(property, e)} />
                                                      &nbsp;${property.columnTitle}
                                                  </label>
                                              </li>
                                          `
                                      )}
                                  </ul>
                              </div>
                          </div>
                      </div>
                  `
                : nothing}

            <div class="actions">
                ${this._contentType
                    ? html`<uui-button look="secondary" label="Download Template" @click=${this.#getTemplate}>
                          Download Template
                      </uui-button>`
                    : nothing}
                <uui-button
                    look="primary"
                    label="Import"
                    ?disabled=${this._processing}
                    @click=${this.#import}>
                    ${this._processing ? 'Please wait...' : 'Import'}
                </uui-button>
            </div>
        `;
    }

    #renderSuccess() {
        return html`
            <div class="umb-group-panel">
                <div class="umb-group-panel__header">Processing</div>

                <div class="umb-group-panel__content">
                    <p>CSV file is processing and will appear shortly.</p>
                    <p class="actions">
                        <uui-button look="primary" href="/umbraco#/content?dashboard=imports">
                            View Import Queue
                        </uui-button>
                        <uui-button look="secondary" label="Import Another File" @click=${() => this.#startOver()}>
                            Import Another File
                        </uui-button>
                    </p>
                </div>
            </div>
        `;
    }

    #renderError() {
        return html`
            <div class="umb-group-panel">
                <div class="umb-group-panel__header">Error</div>

                <div class="umb-group-panel__content">
                    ${this._errorMessages
                        ? html`<ul>
                              ${this._errorMessages.map((message) => html`<li class="text-error">${message}</li>`)}
                          </ul>`
                        : nothing}
                    <p>
                        <uui-button look="secondary" label="Start Over" @click=${() => this.#startOver()}>
                            Start Over
                        </uui-button>
                    </p>
                </div>
            </div>
        `;
    }

    render() {
        switch (this._show) {
            case 'success':
                return this.#renderSuccess();
            case 'error':
                return this.#renderError();
            default:
                return this.#renderForm();
        }
    }

    static styles = css`
        :host {
            display: block;
            padding: var(--uui-size-layout-1);
        }

        .umb-group-panel {
            background: var(--uui-color-surface);
            border: 1px solid var(--uui-color-border);
            border-radius: var(--uui-border-radius);
            margin-bottom: var(--uui-size-space-5);
        }

        .umb-group-panel__header {
            padding: var(--uui-size-space-4) var(--uui-size-space-5);
            border-bottom: 1px solid var(--uui-color-border);
            font-weight: bold;
        }

        .umb-group-panel__content {
            padding: var(--uui-size-space-5);
        }

        .control-group {
            margin-bottom: var(--uui-size-space-4);
        }

        .control-group label {
            display: block;
            margin-bottom: var(--uui-size-space-2);
            font-weight: bold;
        }

        .required {
            color: var(--uui-color-danger);
        }

        select {
            min-width: 250px;
            padding: var(--uui-size-space-2);
        }

        .listTable .link {
            cursor: pointer;
            color: var(--uui-color-interactive);
        }

        .selectionCheckBoxes {
            list-style: none;
            padding: 0;
            margin-top: var(--uui-size-space-4);
        }

        .selectionCheckBoxes li {
            margin-bottom: var(--uui-size-space-2);
        }

        .actions {
            display: flex;
            gap: var(--uui-size-space-3);
            align-items: center;
        }

        .text-error {
            color: var(--uui-color-danger);
        }
    `;
}

customElements.define(elementName, N3oDataImportElement);

export default N3oDataImportElement;
export { N3oDataImportElement };
