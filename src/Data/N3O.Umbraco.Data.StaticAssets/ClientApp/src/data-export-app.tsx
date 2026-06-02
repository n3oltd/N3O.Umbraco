import { useEffect, useState } from 'react';

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
}

// React UI for the content-export workspace view. Exports a document's descendants of a chosen content
// type to Excel/CSV. Ported from the Lit component; reuses the same backend endpoints verbatim. The
// current document key is supplied by the host shell (from the document workspace context) as a prop.
export function DataExportApp({ contentKey }: DataExportAppProps) {
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
        if (!contentKey) {
            return;
        }

        let active = true;

        const init = async (): Promise<void> => {
            const types = await getContentTypes(contentKey);

            const metadata = (await fetch('/umbraco/backoffice/api/Exports/lookups/contentMetadata', {
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
    }, [contentKey]);

    const getContentTypes = async (contentId: string): Promise<ContentType[]> => {
        const response = await fetch(`/umbraco/api/ContentTypes/${contentId}/relations?type=descendant`, {
            headers: { Accept: 'application/json' },
        });

        return (await response.json()) as ContentType[];
    };

    const refreshProperties = async (selected: ContentType | null): Promise<void> => {
        if (!selected) {
            setExportableProperties([]);
            return;
        }

        const res = (await fetch(`/umbraco/backoffice/api/Exports/exportableProperties/${selected.alias}`, {
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
            const getProgress = await fetch(`/umbraco/backoffice/api/Exports/export/${exportId}/progress`, {
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

        const createExport = await fetch(
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
                const exportFile = await fetch(`/umbraco/backoffice/api/Exports/export/${res.id}/file`, {
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

    return (
        <div className="n3o-data-export">
            <uui-box headline="Options">
                <umb-property-layout label="Content Type" mandatory>
                    <div slot="editor">
                        <select disabled={processing} onChange={onContentTypeChange}>
                            <option value="" selected={!contentType}></option>
                            {contentTypes.map((item) => (
                                <option value={item.alias} key={item.alias}>
                                    {item.name}
                                </option>
                            ))}
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
                                checked={format === 'excel'}
                                disabled={processing}
                                onChange={() => setFormat('excel')}
                            />
                            Excel
                        </label>
                        <br />
                        <label>
                            <input
                                type="radio"
                                name="format"
                                value="csv"
                                checked={format === 'csv'}
                                disabled={processing}
                                onChange={() => setFormat('csv')}
                            />
                            CSV
                        </label>
                    </div>
                </umb-property-layout>

                <umb-property-layout label="Include Unpublished" mandatory>
                    <div slot="editor">
                        <input
                            type="checkbox"
                            checked={includeUnpublished}
                            disabled={processing}
                            onChange={(e) => setIncludeUnpublished(e.target.checked)}
                        />
                    </div>
                </umb-property-layout>
            </uui-box>

            <uui-box headline="Metadata">
                <div className="listTable">
                    <a className="umb-outline" onClick={selectAllMetadatas}>
                        Select All
                    </a>{' '}
                    |{' '}
                    <a className="umb-outline" onClick={clearSelectedMetadatas}>
                        Clear Selection
                    </a>

                    <br />
                    <br />

                    <ul className="selectionCheckBoxes">
                        {metadatas.map((metadata) => (
                            <li key={metadata.id}>
                                <label>
                                    <input
                                        type="checkbox"
                                        checked={!!metadata.selected}
                                        onChange={(e) => toggleMetadata(metadata, e.target.checked)}
                                    />
                                    &nbsp;{metadata.name}
                                </label>
                            </li>
                        ))}
                    </ul>
                </div>
            </uui-box>

            <uui-box headline="Properties">
                <div className="listTable">
                    <a className="umb-outline" onClick={selectAllProperties}>
                        Select All
                    </a>{' '}
                    |{' '}
                    <a className="umb-outline" onClick={clearSelectedProperties}>
                        Clear Selection
                    </a>

                    <br />
                    <br />

                    <ul className="selectionCheckBoxes">
                        {exportableProperties.map((property) => (
                            <li key={property.alias}>
                                <label>
                                    <input
                                        type="checkbox"
                                        checked={!!property.selected}
                                        onChange={(e) => toggleProperty(property, e.target.checked)}
                                    />
                                    &nbsp;{property.columnTitle}
                                </label>
                            </li>
                        ))}
                    </ul>
                </div>

                {errorMessage ? <em className="text-error">{errorMessage}</em> : null}
            </uui-box>

            <div className="actions">
                <uui-button
                    look="primary"
                    disabled={processing}
                    onClick={() => void doExport()}
                    label={processing ? progress || 'Export' : 'Export'}
                ></uui-button>
            </div>

            <style>{styles}</style>
        </div>
    );
}

const styles = `
    .n3o-data-export {
        display: block;
        padding: var(--uui-size-layout-1);
    }
    .n3o-data-export uui-box {
        margin-bottom: var(--uui-size-layout-1);
    }
    .n3o-data-export .listTable {
        overflow: hidden;
    }
    .n3o-data-export ul.selectionCheckBoxes {
        list-style: none;
        column-count: 4;
        column-gap: 0.5em;
        display: block;
        padding: 0;
        margin: 0;
    }
    .n3o-data-export .umb-outline {
        cursor: pointer;
        color: var(--uui-color-interactive);
    }
    .n3o-data-export .text-error {
        color: var(--uui-color-danger);
        display: block;
        margin-top: var(--uui-size-space-3);
    }
    .n3o-data-export .actions {
        margin-top: var(--uui-size-layout-1);
    }
`;
