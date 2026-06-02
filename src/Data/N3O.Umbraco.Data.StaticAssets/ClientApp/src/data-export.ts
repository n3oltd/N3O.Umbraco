import { LitElement, css, customElement, html, nothing, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';

const elementName = 'n3o-data-export';

interface ContentType {
    alias: string;
    name: string;
}

interface ContentMetadata {
    id: string;
    name: string;
    autoSelected: boolean;
    displayOrder: number;
    selected: boolean;
}

interface ExportableProperty {
    alias: string;
    columnTitle: string;
    selected: boolean;
}

interface ExportProgressResponse {
    isComplete: boolean;
    text: string;
    id: string;
}

interface CreateExportResponse {
    id: string;
}

// Content app (workspace view) that exports a document's descendants of a chosen content type to
// Excel/CSV. Ported from the AngularJS "N3O.Umbraco.Data.Export" controller. Reuses the same backend
// endpoints verbatim. The current document key is taken from the document workspace context.
@customElement(elementName)
export class N3oDataExportElement extends UmbElementMixin(LitElement) {
    @state()
    private _contentKey: string | null = null;

    @state()
    private _contentTypes: ContentType[] = [];

    @state()
    private _contentType: ContentType | null = null;

    @state()
    private _format: string = 'excel';

    @state()
    private _includeUnpublished: boolean = false;

    @state()
    private _metadatas: ContentMetadata[] = [];

    @state()
    private _exportableProperties: ExportableProperty[] = [];

    @state()
    private _processing: boolean = false;

    @state()
    private _progress: string = '';

    @state()
    private _errorMessage: string | null = null;

    constructor() {
        super();

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            if (!context) {
                return;
            }

            this.observe(
                context.unique,
                (unique) => {
                    if (unique && unique !== this._contentKey) {
                        this._contentKey = unique;
                        void this.#init(unique);
                    }
                },
                '_observeUnique'
            );
        });
    }

    async #init(contentKey: string): Promise<void> {
        this._contentTypes = await this.#getContentTypes(contentKey);

        const res = await fetch('/umbraco/backoffice/api/Exports/lookups/contentMetadata', {
            headers: { Accept: 'application/json' },
        }).then((r) => r.json()) as ContentMetadata[];

        for (const metadata of res) {
            metadata.selected = metadata.autoSelected;
        }

        res.sort((a, b) => a.displayOrder - b.displayOrder);

        this._metadatas = res;
    }

    async #getContentTypes(contentId: string): Promise<ContentType[]> {
        const response = await fetch(`/umbraco/api/ContentTypes/${contentId}/relations?type=descendant`, {
            headers: { Accept: 'application/json' },
        });

        return (await response.json()) as ContentType[];
    }

    async #refreshProperties(): Promise<void> {
        if (!this._contentType) {
            this._exportableProperties = [];
            return;
        }

        const res = await fetch(
            `/umbraco/backoffice/api/Exports/exportableProperties/${this._contentType.alias}`,
            { headers: { Accept: 'application/json' } }
        ).then((r) => r.json()) as ExportableProperty[];

        for (const property of res) {
            property.selected = false;
        }

        this._exportableProperties = res;
    }

    #onContentTypeChange(event: Event): void {
        const alias = (event.target as HTMLSelectElement).value;
        this._contentType = this._contentTypes.find((x) => x.alias === alias) ?? null;
        void this.#refreshProperties();
    }

    #poll(exportId: string): Promise<ExportProgressResponse> {
        const executePoll = async (
            resolve: (value: ExportProgressResponse) => void,
            reject: (reason?: unknown) => void
        ): Promise<void> => {
            const getProgress = await fetch(`/umbraco/backoffice/api/Exports/export/${exportId}/progress`, {
                headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
                method: 'GET',
            });

            const progressRes = (await getProgress.json()) as ExportProgressResponse;

            if (getProgress.status !== 200) {
                this.#processingError(String(progressRes));
                reject(progressRes);
                return;
            }

            if (progressRes.isComplete === true) {
                resolve(progressRes);
            } else {
                this._progress = progressRes.text;
                setTimeout(() => void executePoll(resolve, reject), 2500);
            }
        };

        return new Promise(executePoll);
    }

    async #export(): Promise<void> {
        this._processing = true;
        this._progress = '';
        this._errorMessage = null;

        if (!this._contentType) {
            this.#processingError('Please select a content type');
            return;
        }

        const selectedMetadataIds = this._metadatas.filter((x) => x.selected).map((x) => x.id);
        const selectedPropertyAliases = this._exportableProperties.filter((x) => x.selected).map((x) => x.alias);

        if (!selectedPropertyAliases.length && !selectedMetadataIds.length) {
            this.#processingError('At least one property or metadata field must be selected');
            return;
        }

        const req = {
            format: this._format,
            includeUnpublished: this._includeUnpublished,
            metadata: selectedMetadataIds,
            properties: selectedPropertyAliases,
        };

        const createExport = await fetch(
            `/umbraco/backoffice/api/Exports/export/${this._contentKey}/${this._contentType.alias}`,
            {
                headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
                method: 'POST',
                body: JSON.stringify(req),
            }
        );

        const createRes = (await createExport.json()) as CreateExportResponse;

        if (createExport.status !== 200) {
            this.#processingError(String(createRes));
            return;
        }

        this.#poll(createRes.id).then(async (res) => {
            const exportFile = await fetch(`/umbraco/backoffice/api/Exports/export/${res.id}/file`, {
                headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
                method: 'GET',
            });

            if (exportFile.status !== 200) {
                this.#processingError(String(await exportFile.json()));
                return;
            }

            const blob = await exportFile.blob();
            const header = exportFile.headers.get('Content-Disposition') ?? '';
            const parts = header.split(';');
            const filename = (parts[1] ?? '').split('=')[1]?.replaceAll('"', '') ?? 'export';
            const newBlob = new Blob([blob]);
            const blobUrl = window.URL.createObjectURL(newBlob);
            const link = document.createElement('a');
            link.href = blobUrl;
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();
            link.parentNode?.removeChild(link);
            window.URL.revokeObjectURL(blobUrl);

            this._processing = false;
            this._progress = '';
        }).catch(() => {
            // error already handled in #poll
        });
    }

    #processingError(message: string): void {
        this._processing = false;
        this._progress = '';
        this._errorMessage = message;
    }

    #selectAllMetadatas(): void {
        this._metadatas = this._metadatas.map((m) => ({ ...m, selected: true }));
    }

    #clearSelectedMetadatas(): void {
        this._metadatas = this._metadatas.map((m) => ({ ...m, selected: false }));
    }

    #selectAllProperties(): void {
        this._exportableProperties = this._exportableProperties.map((p) => ({ ...p, selected: true }));
    }

    #clearSelectedProperties(): void {
        this._exportableProperties = this._exportableProperties.map((p) => ({ ...p, selected: false }));
    }

    #toggleMetadata(metadata: ContentMetadata, checked: boolean): void {
        this._metadatas = this._metadatas.map((m) => (m === metadata ? { ...m, selected: checked } : m));
    }

    #toggleProperty(property: ExportableProperty, checked: boolean): void {
        this._exportableProperties = this._exportableProperties.map((p) =>
            p === property ? { ...p, selected: checked } : p
        );
    }

    override render() {
        return html`
            <uui-box headline="Options">
                <umb-property-layout label="Content Type" mandatory>
                    <div slot="editor">
                        <select
                            ?disabled=${this._processing}
                            @change=${this.#onContentTypeChange}>
                            <option value="" ?selected=${!this._contentType}></option>
                            ${this._contentTypes.map(
                                (item) => html`<option value=${item.alias}>${item.name}</option>`
                            )}
                        </select>
                    </div>
                </umb-property-layout>

                <umb-property-layout label="Format" mandatory>
                    <div slot="editor">
                        <label>
                            <input
                                type="radio"
                                name="format"
                                value="excel"
                                .checked=${this._format === 'excel'}
                                ?disabled=${this._processing}
                                @change=${() => (this._format = 'excel')} />
                            Excel
                        </label>
                        <br />
                        <label>
                            <input
                                type="radio"
                                name="format"
                                value="csv"
                                .checked=${this._format === 'csv'}
                                ?disabled=${this._processing}
                                @change=${() => (this._format = 'csv')} />
                            CSV
                        </label>
                    </div>
                </umb-property-layout>

                <umb-property-layout label="Include Unpublished" mandatory>
                    <div slot="editor">
                        <input
                            type="checkbox"
                            .checked=${this._includeUnpublished}
                            ?disabled=${this._processing}
                            @change=${(e: Event) => (this._includeUnpublished = (e.target as HTMLInputElement).checked)} />
                    </div>
                </umb-property-layout>
            </uui-box>

            <uui-box headline="Metadata">
                <div class="listTable">
                    <a class="umb-outline" @click=${this.#selectAllMetadatas}>Select All</a> |
                    <a class="umb-outline" @click=${this.#clearSelectedMetadatas}>Clear Selection</a>

                    <br /><br />

                    <ul class="selectionCheckBoxes">
                        ${this._metadatas.map(
                            (metadata) => html`
                                <li>
                                    <label>
                                        <input
                                            type="checkbox"
                                            .checked=${!!metadata.selected}
                                            @change=${(e: Event) => this.#toggleMetadata(metadata, (e.target as HTMLInputElement).checked)} />
                                        &nbsp;${metadata.name}
                                    </label>
                                </li>
                            `
                        )}
                    </ul>
                </div>
            </uui-box>

            <uui-box headline="Properties">
                <div class="listTable">
                    <a class="umb-outline" @click=${this.#selectAllProperties}>Select All</a> |
                    <a class="umb-outline" @click=${this.#clearSelectedProperties}>Clear Selection</a>

                    <br /><br />

                    <ul class="selectionCheckBoxes">
                        ${this._exportableProperties.map(
                            (property) => html`
                                <li>
                                    <label>
                                        <input
                                            type="checkbox"
                                            .checked=${!!property.selected}
                                            @change=${(e: Event) => this.#toggleProperty(property, (e.target as HTMLInputElement).checked)} />
                                        &nbsp;${property.columnTitle}
                                    </label>
                                </li>
                            `
                        )}
                    </ul>
                </div>

                ${this._errorMessage
                    ? html`<em class="text-error">${this._errorMessage}</em>`
                    : nothing}
            </uui-box>

            <div class="actions">
                <uui-button
                    look="primary"
                    ?disabled=${this._processing}
                    @click=${this.#export}
                    label=${this._processing ? this._progress || 'Export' : 'Export'}>
                </uui-button>
            </div>
        `;
    }

    static override styles = css`
        :host {
            display: block;
            padding: var(--uui-size-layout-1);
        }

        uui-box {
            margin-bottom: var(--uui-size-layout-1);
        }

        .listTable {
            overflow: hidden;
        }

        ul.selectionCheckBoxes {
            list-style: none;
            column-count: 4;
            column-gap: 0.5em;
            display: block;
            padding: 0;
            margin: 0;
        }

        .umb-outline {
            cursor: pointer;
            color: var(--uui-color-interactive);
        }

        .text-error {
            color: var(--uui-color-danger);
            display: block;
            margin-top: var(--uui-size-space-3);
        }

        .actions {
            margin-top: var(--uui-size-layout-1);
        }
    `;
}

export default N3oDataExportElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oDataExportElement;
    }
}
