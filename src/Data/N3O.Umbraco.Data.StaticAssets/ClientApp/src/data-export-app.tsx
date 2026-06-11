import { useEffect, useState } from 'react';
import type { AuthFetch } from './auth-fetch';

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

interface DataExportAppProps {
    contentKey: string | null;
    authFetch: AuthFetch | null;
}

// React UI for the content-export workspace view. Exports a document's descendants of a chosen content
// type to Excel/CSV. Ported from the Lit component; reuses the same backend endpoints verbatim. The
// current document key is supplied by the host shell (from the document workspace context) as a prop.
export function DataExportApp({ contentKey, authFetch }: DataExportAppProps) {
    const [contentTypes, setContentTypes] = useState<ContentType[]>([]);
    const [contentType, setContentType] = useState<ContentType | null>(null);
    const [format, setFormat] = useState<string>('excel');
    const [includeUnpublished, setIncludeUnpublished] = useState<boolean>(false);
    const [metadatas, setMetadatas] = useState<ContentMetadata[]>([]);
    const [exportableProperties, setExportableProperties] = useState<ExportableProperty[]>([]);
    const [processing, setProcessing] = useState<boolean>(false);
    const [progress, setProgress] = useState<string>('');
    const [errorMessage, setErrorMessage] = useState<string | null>(null);

    useEffect(() => {
        if (!contentKey || !authFetch) {
            return;
        }

        let active = true;

        const init = async (): Promise<void> => {
            const types = await getContentTypes(contentKey);

            const metadata = (await authFetch('/umbraco/backoffice/api/Exports/lookups/contentMetadata', {
                headers: { Accept: 'application/json' },
            }).then((r) => r.json())) as ContentMetadata[];

            for (const m of metadata) {
                m.selected = m.autoSelected;
            }

            metadata.sort((a, b) => a.displayOrder - b.displayOrder);

            if (active) {
                setContentTypes(types);
                setMetadatas(metadata);
            }
        };

        void init();

        return () => {
            active = false;
        };
    }, [contentKey, authFetch]);

    const getContentTypes = async (contentId: string): Promise<ContentType[]> => {
        const response = await authFetch!(`/umbraco/api/ContentTypes/${contentId}/relations?type=descendant`, {
            headers: { Accept: 'application/json' },
        });

        return (await response.json()) as ContentType[];
    };

    const refreshProperties = async (selected: ContentType | null): Promise<void> => {
        if (!selected) {
            setExportableProperties([]);
            return;
        }

        const res = (await authFetch!(`/umbraco/backoffice/api/Exports/exportableProperties/${selected.alias}`, {
            headers: { Accept: 'application/json' },
        }).then((r) => r.json())) as ExportableProperty[];

        for (const property of res) {
            property.selected = false;
        }

        setExportableProperties(res);
    };

    const onContentTypeChange = (event: React.ChangeEvent<HTMLSelectElement>): void => {
        const alias = event.target.value;
        const selected = contentTypes.find((x) => x.alias === alias) ?? null;
        setContentType(selected);
        void refreshProperties(selected);
    };

    const processingError = (message: string): void => {
        setProcessing(false);
        setProgress('');
        setErrorMessage(message);
    };

    const poll = (exportId: string): Promise<ExportProgressResponse> => {
        const executePoll = async (
            resolve: (value: ExportProgressResponse) => void,
            reject: (reason?: unknown) => void
        ): Promise<void> => {
            const getProgress = await authFetch!(`/umbraco/backoffice/api/Exports/export/${exportId}/progress`, {
                headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
                method: 'GET',
            });

            const progressRes = (await getProgress.json()) as ExportProgressResponse;

            if (getProgress.status !== 200) {
                processingError(String(progressRes));
                reject(progressRes);
                return;
            }

            if (progressRes.isComplete === true) {
                resolve(progressRes);
            } else {
                setProgress(progressRes.text);
                setTimeout(() => void executePoll(resolve, reject), 2500);
            }
        };

        return new Promise(executePoll);
    };

    const doExport = async (): Promise<void> => {
        setProcessing(true);
        setProgress('');
        setErrorMessage(null);

        if (!contentType) {
            processingError('Please select a content type');
            return;
        }

        const selectedMetadataIds = metadatas.filter((x) => x.selected).map((x) => x.id);
        const selectedPropertyAliases = exportableProperties.filter((x) => x.selected).map((x) => x.alias);

        if (!selectedPropertyAliases.length && !selectedMetadataIds.length) {
            processingError('At least one property or metadata field must be selected');
            return;
        }

        const req = {
            format,
            includeUnpublished,
            metadata: selectedMetadataIds,
            properties: selectedPropertyAliases,
        };

        const createExport = await authFetch!(
            `/umbraco/backoffice/api/Exports/export/${contentKey}/${contentType.alias}`,
            {
                headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
                method: 'POST',
                body: JSON.stringify(req),
            }
        );

        const createRes = (await createExport.json()) as CreateExportResponse;

        if (createExport.status !== 200) {
            processingError(String(createRes));
            return;
        }

        poll(createRes.id)
            .then(async (res) => {
                const exportFile = await authFetch!(`/umbraco/backoffice/api/Exports/export/${res.id}/file`, {
                    headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
                    method: 'GET',
                });

                if (exportFile.status !== 200) {
                    processingError(String(await exportFile.json()));
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

                setProcessing(false);
                setProgress('');
            })
            .catch(() => {
                // error already handled in poll
            });
    };

    const selectAllMetadatas = (): void => setMetadatas((prev) => prev.map((m) => ({ ...m, selected: true })));
    const clearSelectedMetadatas = (): void => setMetadatas((prev) => prev.map((m) => ({ ...m, selected: false })));
    const selectAllProperties = (): void =>
        setExportableProperties((prev) => prev.map((p) => ({ ...p, selected: true })));
    const clearSelectedProperties = (): void =>
        setExportableProperties((prev) => prev.map((p) => ({ ...p, selected: false })));

    const toggleMetadata = (metadata: ContentMetadata, checked: boolean): void =>
        setMetadatas((prev) => prev.map((m) => (m === metadata ? { ...m, selected: checked } : m)));
    const toggleProperty = (property: ExportableProperty, checked: boolean): void =>
        setExportableProperties((prev) => prev.map((p) => (p === property ? { ...p, selected: checked } : p)));

    const selectedMetadataCount = metadatas.filter((m) => m.selected).length;
    const selectedPropertyCount = exportableProperties.filter((p) => p.selected).length;
    const hasSelection = selectedMetadataCount > 0 || selectedPropertyCount > 0;
    const canExport = !!contentType && hasSelection && !processing;

    return (
        <div className="n3o-data-export">
            <uui-box headline="Export options">
                <umb-property-layout
                    label="Content type"
                    description="The descendant type to export. Properties available for export depend on this."
                    mandatory>
                    <div slot="editor">
                        <select
                            className="nativeSelect"
                            value={contentType?.alias ?? ''}
                            onChange={onContentTypeChange}
                            disabled={processing || contentTypes.length === 0}>
                            <option value="" disabled>
                                Select a content type…
                            </option>
                            {contentTypes.map((item) => (
                                <option key={item.alias} value={item.alias}>
                                    {item.name}
                                </option>
                            ))}
                        </select>
                    </div>
                </umb-property-layout>

                <umb-property-layout
                    label="File format"
                    description="Choose the file format for the exported data."
                    mandatory>
                    <div slot="editor">
                        <div className="radioGroup">
                            <label className="radioOption">
                                <input
                                    type="radio"
                                    name="format"
                                    value="excel"
                                    checked={format === 'excel'}
                                    onChange={(e) => setFormat(e.target.value)}
                                    disabled={processing}
                                />
                                <span>Excel (.xlsx)</span>
                            </label>
                            <label className="radioOption">
                                <input
                                    type="radio"
                                    name="format"
                                    value="csv"
                                    checked={format === 'csv'}
                                    onChange={(e) => setFormat(e.target.value)}
                                    disabled={processing}
                                />
                                <span>CSV (.csv)</span>
                            </label>
                        </div>
                    </div>
                </umb-property-layout>

                <umb-property-layout
                    label="Include unpublished"
                    description="When enabled, unpublished content is included in the export.">
                    <div slot="editor">
                        <label className="toggleOption">
                            <input
                                type="checkbox"
                                checked={includeUnpublished}
                                onChange={(e) => setIncludeUnpublished(e.target.checked)}
                                disabled={processing}
                            />
                            <span>Include unpublished content</span>
                        </label>
                    </div>
                </umb-property-layout>
            </uui-box>

            <uui-box headline="Metadata fields">
                <div slot="header-actions" className="selectionCount">
                    {selectedMetadataCount} selected
                </div>

                {metadatas.length === 0 ? (
                    <p className="emptyState">No metadata fields are available.</p>
                ) : (
                    <>
                        <div className="selectionActions">
                            <button
                                type="button"
                                className="btn btn--secondary btn--compact"
                                disabled={processing}
                                onClick={selectAllMetadatas}>
                                Select all
                            </button>
                            <button
                                type="button"
                                className="btn btn--secondary btn--compact"
                                disabled={processing}
                                onClick={clearSelectedMetadatas}>
                                Clear
                            </button>
                        </div>

                        <div className="checkboxGrid">
                            {metadatas.map((metadata) => (
                                <label key={metadata.id} className="checkOption">
                                    <input
                                        type="checkbox"
                                        checked={!!metadata.selected}
                                        onChange={(e) => toggleMetadata(metadata, e.target.checked)}
                                        disabled={processing}
                                    />
                                    <span>{metadata.name}</span>
                                </label>
                            ))}
                        </div>
                    </>
                )}
            </uui-box>

            <uui-box headline="Properties">
                <div slot="header-actions" className="selectionCount">
                    {selectedPropertyCount} selected
                </div>

                {!contentType ? (
                    <p className="emptyState">Select a content type to see its exportable properties.</p>
                ) : exportableProperties.length === 0 ? (
                    <p className="emptyState">This content type has no exportable properties.</p>
                ) : (
                    <>
                        <div className="selectionActions">
                            <button
                                type="button"
                                className="btn btn--secondary btn--compact"
                                disabled={processing}
                                onClick={selectAllProperties}>
                                Select all
                            </button>
                            <button
                                type="button"
                                className="btn btn--secondary btn--compact"
                                disabled={processing}
                                onClick={clearSelectedProperties}>
                                Clear
                            </button>
                        </div>

                        <div className="checkboxGrid">
                            {exportableProperties.map((property) => (
                                <label key={property.alias} className="checkOption">
                                    <input
                                        type="checkbox"
                                        checked={!!property.selected}
                                        onChange={(e) => toggleProperty(property, e.target.checked)}
                                        disabled={processing}
                                    />
                                    <span>{property.columnTitle}</span>
                                </label>
                            ))}
                        </div>
                    </>
                )}
            </uui-box>

            {!processing && contentType && !hasSelection ? (
                <p className="hint">Select at least one metadata field or property to export.</p>
            ) : null}

            {errorMessage ? (
                <div className="errorBox">
                    <uui-icon name="icon-alert"></uui-icon>
                    <span>{errorMessage}</span>
                </div>
            ) : null}

            {processing ? (
                <div className="progress">
                    <uui-loader-bar></uui-loader-bar>
                    <span>{progress || 'Preparing export…'}</span>
                </div>
            ) : null}

            <div className="actions">
                <button
                    type="button"
                    className="btn btn--primary btn--positive"
                    disabled={!canExport}
                    onClick={() => void doExport()}>
                    {processing ? 'Exporting…' : 'Export'}
                </button>
            </div>

            <style>{styles}</style>
        </div>
    );
}

const styles = `
    .n3o-data-export {
        display: block;
        padding: var(--uui-size-space-4);
    }
    .n3o-data-export uui-box {
        --uui-box-default-padding: var(--uui-size-space-4);
        margin-bottom: var(--uui-size-space-3);
    }
    .n3o-data-export .nativeSelect {
        width: 100%;
        max-width: 420px;
        box-sizing: border-box;
        height: var(--uui-size-11, 36px);
        padding: 0 var(--uui-size-space-3);
        font: inherit;
        color: var(--uui-color-text);
        background: var(--uui-color-surface);
        border: 1px solid var(--uui-color-border);
        border-radius: var(--uui-border-radius);
    }
    .n3o-data-export .nativeSelect:focus {
        outline: none;
        border-color: var(--uui-color-focus);
        box-shadow: 0 0 0 1px var(--uui-color-focus);
    }
    .n3o-data-export .nativeSelect:disabled {
        opacity: 0.5;
        cursor: not-allowed;
    }
    .n3o-data-export .radioGroup {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        gap: var(--uui-size-space-2) var(--uui-size-space-5);
    }
    .n3o-data-export .radioOption,
    .n3o-data-export .toggleOption,
    .n3o-data-export .checkOption {
        display: flex;
        align-items: center;
        gap: var(--uui-size-space-2);
        cursor: pointer;
    }
    .n3o-data-export .radioOption input,
    .n3o-data-export .toggleOption input,
    .n3o-data-export .checkOption input {
        cursor: pointer;
    }
    .n3o-data-export .selectionCount {
        font-size: var(--uui-type-small-size);
        color: var(--uui-color-text-alt);
    }
    .n3o-data-export .selectionActions {
        display: flex;
        gap: var(--uui-size-space-2);
        margin-bottom: var(--uui-size-space-3);
    }
    .n3o-data-export .checkboxGrid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
        gap: var(--uui-size-space-1) var(--uui-size-space-5);
    }
    .n3o-data-export .emptyState {
        margin: 0;
        color: var(--uui-color-text-alt);
        font-style: italic;
    }
    .n3o-data-export .hint {
        margin: 0 0 var(--uui-size-space-4);
        color: var(--uui-color-text-alt);
    }
    .n3o-data-export .errorBox {
        display: flex;
        align-items: center;
        gap: var(--uui-size-space-3);
        margin-bottom: var(--uui-size-space-4);
        padding: var(--uui-size-space-4) var(--uui-size-space-5);
        border-radius: var(--uui-border-radius);
        background: var(--uui-color-danger);
        color: var(--uui-color-danger-contrast);
    }
    .n3o-data-export .progress {
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-space-2);
        margin-bottom: var(--uui-size-space-4);
        color: var(--uui-color-text-alt);
    }
    .n3o-data-export .actions {
        margin-top: var(--uui-size-space-4);
    }
    .n3o-data-export .btn {
        font: inherit;
        font-weight: 700;
        line-height: 1;
        display: inline-flex;
        align-items: center;
        gap: var(--uui-size-space-2);
        padding: 0 var(--uui-size-space-4);
        height: var(--uui-size-11, 36px);
        border: 1px solid transparent;
        border-radius: var(--uui-border-radius);
        cursor: pointer;
        box-sizing: border-box;
    }
    .n3o-data-export .btn--compact {
        height: var(--uui-size-9, 30px);
        padding: 0 var(--uui-size-space-3);
        font-size: var(--uui-type-small-size);
    }
    .n3o-data-export .btn--secondary {
        background: var(--uui-color-surface);
        color: var(--uui-color-text);
        border-color: var(--uui-color-border);
    }
    .n3o-data-export .btn--secondary:hover:not(:disabled) {
        background: var(--uui-color-surface-emphasis);
        border-color: var(--uui-color-border-emphasis);
    }
    .n3o-data-export .btn--primary {
        background: var(--uui-color-default);
        color: var(--uui-color-default-contrast);
    }
    .n3o-data-export .btn--primary.btn--positive {
        background: var(--uui-color-positive);
        color: var(--uui-color-positive-contrast);
    }
    .n3o-data-export .btn--primary:hover:not(:disabled) {
        background: var(--uui-color-positive-emphasis, var(--uui-color-positive));
    }
    .n3o-data-export .btn:disabled {
        opacity: 0.5;
        cursor: not-allowed;
    }
`;
