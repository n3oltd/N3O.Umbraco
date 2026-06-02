import { useEffect, useRef, useState } from 'react';

interface ContentType {
    alias: string;
    name: string;
}

interface DatePattern {
    id: string;
    name: string;
}

interface ImportableProperty {
    alias: string;
    columnTitle: string;
    selected: boolean;
}

interface DataImportAppProps {
    contentKey: string | null;
}

// React UI for the multi-step CSV/ZIP import workspace view. Ported from the Lit component (originally
// the AngularJS "Import" content app). Steps: choose content type, select importable properties,
// download a template, upload CSV (+ optional ZIP assets) and queue the import. The current document key
// is supplied by the host shell (from the document workspace context) as a prop. Reuses the same backend
// endpoints verbatim.
export function DataImportApp({ contentKey }: DataImportAppProps) {
    const [show, setShow] = useState<string>('form');
    const [processing, setProcessing] = useState<boolean>(false);
    const [contentTypes, setContentTypes] = useState<ContentType[]>([]);
    const [contentType, setContentType] = useState<ContentType | null>(null);
    const [datePatterns, setDatePatterns] = useState<DatePattern[]>([]);
    const [datePattern, setDatePattern] = useState<DatePattern | null>(null);
    const [moveUpdatedContentToCurrentLocation, setMoveUpdatedContentToCurrentLocation] = useState<boolean>(false);
    const [importableProperties, setImportableProperties] = useState<ImportableProperty[]>([]);
    const [errorMessages, setErrorMessages] = useState<string[] | null>(null);

    const csvFileRef = useRef<HTMLInputElement>(null);
    const zipFileRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if (!contentKey) {
            return;
        }

        let active = true;

        const init = async (): Promise<void> => {
            const types = await getContentTypes(contentKey);

            const res = await fetch('/umbraco/backoffice/api/Imports/lookups/datePatterns', {
                headers: { Accept: 'application/json' },
            });
            const patterns = (await res.json()) as DatePattern[];

            if (active) {
                setContentTypes(types);
                setDatePatterns(patterns);
                setDatePattern(patterns[0] ?? null);
            }
        };

        void init();

        return () => {
            active = false;
        };
    }, [contentKey]);

    const getContentTypes = async (contentId: string): Promise<ContentType[]> => {
        const res = await fetch(`/umbraco/api/ContentTypes/${contentId}/relations?type=child`, {
            headers: { Accept: 'application/json' },
        });

        return (await res.json()) as ContentType[];
    };

    const refreshProperties = async (selected: ContentType | null): Promise<void> => {
        if (!selected) {
            setImportableProperties([]);
            return;
        }

        const res = await fetch(`/umbraco/backoffice/api/Imports/importableProperties/${selected.alias}`, {
            headers: { Accept: 'application/json' },
        });
        const properties = (await res.json()) as ImportableProperty[];

        for (const property of properties) {
            property.selected = false;
        }

        setImportableProperties(properties);
    };

    const startOver = (): void => {
        setProcessing(false);
        setContentType(null);
        setErrorMessages(null);
        setImportableProperties([]);
        setShow('form');
    };

    const onContentTypeChange = (event: React.ChangeEvent<HTMLSelectElement>): void => {
        const alias = event.target.value;
        const selected = contentTypes.find((x) => x.alias === alias) ?? null;
        setContentType(selected);
        void refreshProperties(selected);
    };

    const onDatePatternChange = (event: React.ChangeEvent<HTMLSelectElement>): void => {
        const id = event.target.value;
        setDatePattern(datePatterns.find((x) => x.id === id) ?? null);
    };

    const onPropertyToggle = (property: ImportableProperty, checked: boolean): void => {
        setImportableProperties((prev) => prev.map((p) => (p === property ? { ...p, selected: checked } : p)));
    };

    const selectAllProperties = (): void =>
        setImportableProperties((prev) => prev.map((p) => ({ ...p, selected: true })));

    const clearSelectedProperties = (): void =>
        setImportableProperties((prev) => prev.map((p) => ({ ...p, selected: false })));

    const processingError = (messages: string | string[]): void => {
        const list = Array.isArray(messages) ? messages : [messages];
        setProcessing(false);
        setErrorMessages(list);
        setShow('error');
    };

    const getTemplate = async (): Promise<void> => {
        const selectedPropertyAliases = importableProperties.filter((x) => x.selected).map((x) => x.alias);

        if (!selectedPropertyAliases.length) {
            processingError('At least one property must be selected');
            return;
        }

        const req = { properties: selectedPropertyAliases };

        const getTemplateRes = await fetch(`/umbraco/backoffice/api/Imports/template/${contentType!.alias}`, {
            method: 'POST',
            headers: {
                Accept: '*/*',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(req),
        });

        const blob = await getTemplateRes.blob();
        const header = getTemplateRes.headers.get('Content-Disposition') ?? '';
        const parts = header.split(';');
        const filename = (parts[1] ?? '').split('=')[1]?.replaceAll('"', '') ?? 'template.csv';

        const newBlob = new Blob([blob]);
        const blobUrl = window.URL.createObjectURL(newBlob);
        const link = document.createElement('a');
        link.href = blobUrl;
        link.setAttribute('download', filename);
        document.body.appendChild(link);
        link.click();
        link.parentNode?.removeChild(link);
        window.URL.revokeObjectURL(blobUrl);
    };

    const doImport = async (): Promise<void> => {
        setProcessing(true);

        const csvFile = csvFileRef.current;
        const zipFile = zipFileRef.current;

        if (!csvFile || !csvFile.value || csvFile.value.split('.')[1]?.toLowerCase() !== 'csv') {
            processingError('A valid CSV file must be specified');
            return;
        }

        if (zipFile && zipFile.value && zipFile.value.split('.')[1]?.toLowerCase() !== 'zip') {
            processingError('The selected file is not a valid ZIP file');
            return;
        }

        const csvStorageToken = await getStorageToken(csvFile);
        const zipStorageToken = zipFile ? await getStorageToken(zipFile) : null;

        const req = {
            datePattern: datePattern?.id,
            moveUpdatedContentToCurrentLocation,
            csvFile: csvStorageToken,
            zipFile: zipStorageToken,
        };

        const result = await fetch(
            `/umbraco/backoffice/api/Imports/queue/${contentKey}/${contentType!.alias}`,
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
            setShow('success');
            setProcessing(false);
        } else {
            processingError((await result.json()) as string | string[]);
        }
    };

    const getStorageToken = async (input: HTMLInputElement): Promise<unknown> => {
        if (!input.files || input.files.length === 0) {
            return null;
        }

        const data = new FormData();
        data.append('file', input.files[0]);

        const res = await fetch('/umbraco/api/Storage/tempUpload', {
            method: 'POST',
            body: data,
        });

        return await res.json();
    };

    const renderForm = () => (
        <>
            <div className="umb-group-panel">
                <div className="umb-group-panel__header">Options</div>

                <div className="umb-group-panel__content">
                    <div className="control-group">
                        <label>
                            Content Type <strong className="required">*</strong>
                        </label>
                        <select onChange={onContentTypeChange} disabled={processing}>
                            <option value="" selected={!contentType}></option>
                            {contentTypes.map((item) => (
                                <option value={item.alias} key={item.alias}>
                                    {item.name}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className="control-group">
                        <label>
                            Date Pattern <strong className="required">*</strong>
                        </label>
                        <select onChange={onDatePatternChange} disabled={processing}>
                            {datePatterns.map((item) => (
                                <option value={item.id} key={item.id}>
                                    {item.name}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className="control-group">
                        <label>Move Updated Content to Current Location</label>
                        <input
                            type="checkbox"
                            checked={moveUpdatedContentToCurrentLocation}
                            onChange={(e) => setMoveUpdatedContentToCurrentLocation(e.target.checked)}
                            disabled={processing}
                        />
                    </div>

                    <div className="control-group">
                        <label>
                            CSV File <strong className="required">*</strong>
                        </label>
                        <input type="file" id="csvFile" ref={csvFileRef} disabled={processing} />
                    </div>

                    <div className="control-group">
                        <label>ZIP Assets File (optional)</label>
                        <input type="file" id="zipFile" ref={zipFileRef} disabled={processing} />
                    </div>
                </div>
            </div>

            {contentType ? (
                <div className="umb-group-panel">
                    <div className="umb-group-panel__header">Properties</div>

                    <div className="umb-group-panel__content">
                        <div className="listTable">
                            <a className="link" onClick={selectAllProperties}>
                                Select All
                            </a>{' '}
                            |{' '}
                            <a className="link" onClick={clearSelectedProperties}>
                                Clear Selection
                            </a>

                            <ul className="selectionCheckBoxes">
                                {importableProperties.map((property) => (
                                    <li key={property.alias}>
                                        <label>
                                            <input
                                                type="checkbox"
                                                value={property.alias}
                                                checked={!!property.selected}
                                                onChange={(e) => onPropertyToggle(property, e.target.checked)}
                                            />
                                            &nbsp;{property.columnTitle}
                                        </label>
                                    </li>
                                ))}
                            </ul>
                        </div>
                    </div>
                </div>
            ) : null}

            <div className="actions">
                {contentType ? (
                    <uui-button look="secondary" label="Download Template" onClick={() => void getTemplate()}>
                        Download Template
                    </uui-button>
                ) : null}
                <uui-button look="primary" label="Import" disabled={processing} onClick={() => void doImport()}>
                    {processing ? 'Please wait...' : 'Import'}
                </uui-button>
            </div>
        </>
    );

    // FLAG: The href below uses a legacy AngularJS hash route (/umbraco#/content?dashboard=imports).
    // This route does not exist in Umbraco 17 Bellissima; the link will not navigate correctly.
    // A new Bellissima route path for the imports dashboard will be needed when it is available.
    const renderSuccess = () => (
        <div className="umb-group-panel">
            <div className="umb-group-panel__header">Processing</div>

            <div className="umb-group-panel__content">
                <p>CSV file is processing and will appear shortly.</p>
                <p className="actions">
                    <uui-button look="primary" href="/umbraco#/content?dashboard=imports">
                        View Import Queue
                    </uui-button>
                    <uui-button look="secondary" label="Import Another File" onClick={startOver}>
                        Import Another File
                    </uui-button>
                </p>
            </div>
        </div>
    );

    const renderError = () => (
        <div className="umb-group-panel">
            <div className="umb-group-panel__header">Error</div>

            <div className="umb-group-panel__content">
                {errorMessages ? (
                    <ul>
                        {errorMessages.map((message, index) => (
                            <li className="text-error" key={index}>
                                {message}
                            </li>
                        ))}
                    </ul>
                ) : null}
                <p>
                    <uui-button look="secondary" label="Start Over" onClick={startOver}>
                        Start Over
                    </uui-button>
                </p>
            </div>
        </div>
    );

    return (
        <div className="n3o-data-import">
            {show === 'success' ? renderSuccess() : show === 'error' ? renderError() : renderForm()}
            <style>{styles}</style>
        </div>
    );
}

const styles = `
    .n3o-data-import {
        display: block;
        padding: var(--uui-size-layout-1);
    }
    .n3o-data-import .umb-group-panel {
        background: var(--uui-color-surface);
        border: 1px solid var(--uui-color-border);
        border-radius: var(--uui-border-radius);
        margin-bottom: var(--uui-size-space-5);
    }
    .n3o-data-import .umb-group-panel__header {
        padding: var(--uui-size-space-4) var(--uui-size-space-5);
        border-bottom: 1px solid var(--uui-color-border);
        font-weight: bold;
    }
    .n3o-data-import .umb-group-panel__content {
        padding: var(--uui-size-space-5);
    }
    .n3o-data-import .control-group {
        margin-bottom: var(--uui-size-space-4);
    }
    .n3o-data-import .control-group label {
        display: block;
        margin-bottom: var(--uui-size-space-2);
        font-weight: bold;
    }
    .n3o-data-import .required {
        color: var(--uui-color-danger);
    }
    .n3o-data-import select {
        min-width: 250px;
        padding: var(--uui-size-space-2);
    }
    .n3o-data-import .listTable .link {
        cursor: pointer;
        color: var(--uui-color-interactive);
    }
    .n3o-data-import .selectionCheckBoxes {
        list-style: none;
        padding: 0;
        margin-top: var(--uui-size-space-4);
    }
    .n3o-data-import .selectionCheckBoxes li {
        margin-bottom: var(--uui-size-space-2);
    }
    .n3o-data-import .actions {
        display: flex;
        gap: var(--uui-size-space-3);
        align-items: center;
    }
    .n3o-data-import .text-error {
        color: var(--uui-color-danger);
    }
`;
