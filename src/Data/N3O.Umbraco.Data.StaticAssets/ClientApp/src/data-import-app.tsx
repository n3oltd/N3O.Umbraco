import { useEffect, useRef, useState } from 'react';
import type { AuthFetch } from './auth-fetch';

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
    authFetch: AuthFetch | null;
}

// React UI for the multi-step CSV/ZIP import workspace view. Ported from the Lit component (originally
// the AngularJS "Import" content app). Steps: choose content type, select importable properties,
// download a template, upload CSV (+ optional ZIP assets) and queue the import. The current document key
// is supplied by the host shell (from the document workspace context) as a prop. Reuses the same backend
// endpoints verbatim.
export function DataImportApp({ contentKey, authFetch }: DataImportAppProps) {
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
        if (!contentKey || !authFetch) {
            return;
        }

        let active = true;

        const init = async (): Promise<void> => {
            const types = await getContentTypes(contentKey);

            const res = await authFetch('/umbraco/backoffice/api/Imports/lookups/datePatterns', {
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
    }, [contentKey, authFetch]);

    const getContentTypes = async (contentId: string): Promise<ContentType[]> => {
        const res = await authFetch!(`/umbraco/api/ContentTypes/${contentId}/relations?type=child`, {
            headers: { Accept: 'application/json' },
        });

        return (await res.json()) as ContentType[];
    };

    const refreshProperties = async (selected: ContentType | null): Promise<void> => {
        if (!selected) {
            setImportableProperties([]);
            return;
        }

        const res = await authFetch!(`/umbraco/backoffice/api/Imports/importableProperties/${selected.alias}`, {
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

        const getTemplateRes = await authFetch!(`/umbraco/backoffice/api/Imports/template/${contentType!.alias}`, {
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

        const result = await authFetch!(
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

        const res = await authFetch!('/umbraco/api/Storage/tempUpload', {
            method: 'POST',
            body: data,
        });

        return await res.json();
    };

    const selectedPropertyCount = importableProperties.filter((p) => p.selected).length;

    const renderForm = () => (
        <>
            <uui-box headline="1. Choose what to import">
                <umb-property-layout
                    label="Content type"
                    description="The child type that rows in your CSV will be imported as."
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
                    label="Date pattern"
                    description="How dates in your CSV are formatted, so they can be parsed correctly."
                    mandatory>
                    <div slot="editor">
                        <select
                            className="nativeSelect"
                            value={datePattern?.id ?? ''}
                            onChange={onDatePatternChange}
                            disabled={processing || datePatterns.length === 0}>
                            {datePatterns.map((item) => (
                                <option key={item.id} value={item.id}>
                                    {item.name}
                                </option>
                            ))}
                        </select>
                    </div>
                </umb-property-layout>

                <umb-property-layout
                    label="Move updated content"
                    description="When enabled, existing content that is updated will be moved beneath the current item.">
                    <div slot="editor">
                        <label className="toggleOption">
                            <input
                                type="checkbox"
                                checked={moveUpdatedContentToCurrentLocation}
                                onChange={(e) => setMoveUpdatedContentToCurrentLocation(e.target.checked)}
                                disabled={processing}
                            />
                            <span>Move updated content to the current location</span>
                        </label>
                    </div>
                </umb-property-layout>
            </uui-box>

            <uui-box headline="2. Select properties">
                <div slot="header-actions" className="selectionCount">
                    {selectedPropertyCount} selected
                </div>

                {!contentType ? (
                    <p className="emptyState">Select a content type above to choose which properties to import.</p>
                ) : importableProperties.length === 0 ? (
                    <p className="emptyState">This content type has no importable properties.</p>
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
                            {importableProperties.map((property) => (
                                <label key={property.alias} className="checkOption">
                                    <input
                                        type="checkbox"
                                        checked={!!property.selected}
                                        onChange={(e) => onPropertyToggle(property, e.target.checked)}
                                        disabled={processing}
                                    />
                                    <span>{property.columnTitle}</span>
                                </label>
                            ))}
                        </div>
                    </>
                )}
            </uui-box>

            <uui-box headline="3. Download template">
                <p className="boxHint">
                    Download a CSV template containing a column for each selected property, then fill it in
                    with your data.
                </p>
                <button
                    type="button"
                    className="btn btn--secondary"
                    disabled={!contentType || selectedPropertyCount === 0 || processing}
                    onClick={() => void getTemplate()}>
                    <uui-icon name="icon-download-alt"></uui-icon>
                    Download template
                </button>
            </uui-box>

            <uui-box headline="4. Upload &amp; queue">
                <umb-property-layout
                    label="CSV file"
                    description="The completed CSV file containing the rows to import."
                    mandatory>
                    <div slot="editor">
                        <input type="file" id="csvFile" accept=".csv" ref={csvFileRef} disabled={processing} />
                    </div>
                </umb-property-layout>

                <umb-property-layout
                    label="ZIP assets file"
                    description="Optional. A ZIP archive of media/assets referenced by the CSV.">
                    <div slot="editor">
                        <input type="file" id="zipFile" accept=".zip" ref={zipFileRef} disabled={processing} />
                    </div>
                </umb-property-layout>

                {processing ? (
                    <div className="progress">
                        <uui-loader-bar></uui-loader-bar>
                        <span>Queueing import…</span>
                    </div>
                ) : null}

                <div className="actions">
                    <button
                        type="button"
                        className="btn btn--primary btn--positive"
                        disabled={!contentType || processing}
                        onClick={() => void doImport()}>
                        {processing ? 'Importing…' : 'Import'}
                    </button>
                </div>
            </uui-box>
        </>
    );

    // The imports queue is a Umbraco UI Builder dashboard (alias "imports") on the Content section.
    // In the v17 Bellissima backoffice that resolves to the path-based route below
    // (section pathname "content" + dashboard pathname "imports"); there is no hash route.
    const renderSuccess = () => (
        <uui-box headline="Import queued">
            <div className="statusBox statusBox--positive">
                <uui-icon name="icon-check"></uui-icon>
                <span>Your CSV file has been queued and will be processed shortly.</span>
            </div>
            <div className="actions">
                <a className="btn btn--primary" href="/umbraco/section/content/dashboard/imports">
                    View import queue
                </a>
                <button type="button" className="btn btn--secondary" onClick={startOver}>
                    Import another file
                </button>
            </div>
        </uui-box>
    );

    const renderError = () => (
        <uui-box headline="Import failed">
            <div className="statusBox statusBox--danger">
                <uui-icon name="icon-alert"></uui-icon>
                <div>
                    {errorMessages && errorMessages.length > 0 ? (
                        <ul className="errorList">
                            {errorMessages.map((message, index) => (
                                <li key={index}>{message}</li>
                            ))}
                        </ul>
                    ) : (
                        <span>Something went wrong while queueing the import.</span>
                    )}
                </div>
            </div>
            <div className="actions">
                <button type="button" className="btn btn--secondary" onClick={startOver}>
                    Start over
                </button>
            </div>
        </uui-box>
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
        padding: var(--uui-size-space-4);
    }
    .n3o-data-import uui-box {
        --uui-box-default-padding: var(--uui-size-space-4);
        margin-bottom: var(--uui-size-space-3);
    }
    .n3o-data-import .nativeSelect {
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
    .n3o-data-import .nativeSelect:focus {
        outline: none;
        border-color: var(--uui-color-focus);
        box-shadow: 0 0 0 1px var(--uui-color-focus);
    }
    .n3o-data-import .nativeSelect:disabled {
        opacity: 0.5;
        cursor: not-allowed;
    }
    .n3o-data-import input[type='file'] {
        font: inherit;
    }
    .n3o-data-import .toggleOption,
    .n3o-data-import .checkOption {
        display: flex;
        align-items: center;
        gap: var(--uui-size-space-2);
        cursor: pointer;
    }
    .n3o-data-import .toggleOption input,
    .n3o-data-import .checkOption input {
        cursor: pointer;
    }
    .n3o-data-import .selectionCount {
        font-size: var(--uui-type-small-size);
        color: var(--uui-color-text-alt);
    }
    .n3o-data-import .selectionActions {
        display: flex;
        gap: var(--uui-size-space-2);
        margin-bottom: var(--uui-size-space-3);
    }
    .n3o-data-import .checkboxGrid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
        gap: var(--uui-size-space-1) var(--uui-size-space-5);
    }
    .n3o-data-import .emptyState {
        margin: 0;
        color: var(--uui-color-text-alt);
        font-style: italic;
    }
    .n3o-data-import .boxHint {
        margin: 0 0 var(--uui-size-space-4);
        color: var(--uui-color-text-alt);
    }
    .n3o-data-import .progress {
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-space-2);
        margin: var(--uui-size-space-4) 0;
        color: var(--uui-color-text-alt);
    }
    .n3o-data-import .statusBox {
        display: flex;
        align-items: flex-start;
        gap: var(--uui-size-space-3);
        padding: var(--uui-size-space-4) var(--uui-size-space-5);
        border-radius: var(--uui-border-radius);
        margin-bottom: var(--uui-size-space-4);
    }
    .n3o-data-import .statusBox--positive {
        background: var(--uui-color-positive);
        color: var(--uui-color-positive-contrast);
    }
    .n3o-data-import .statusBox--danger {
        background: var(--uui-color-danger);
        color: var(--uui-color-danger-contrast);
    }
    .n3o-data-import .errorList {
        margin: 0;
        padding-left: var(--uui-size-space-4);
    }
    .n3o-data-import .actions {
        display: flex;
        gap: var(--uui-size-space-3);
        align-items: center;
        margin-top: var(--uui-size-space-4);
    }
    .n3o-data-import .btn {
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
        text-decoration: none;
    }
    .n3o-data-import .btn--compact {
        height: var(--uui-size-9, 30px);
        padding: 0 var(--uui-size-space-3);
        font-size: var(--uui-type-small-size);
    }
    .n3o-data-import .btn--secondary {
        background: var(--uui-color-surface);
        color: var(--uui-color-text);
        border-color: var(--uui-color-border);
    }
    .n3o-data-import .btn--secondary:hover:not(:disabled) {
        background: var(--uui-color-surface-emphasis);
        border-color: var(--uui-color-border-emphasis);
    }
    .n3o-data-import .btn--primary {
        background: var(--uui-color-default);
        color: var(--uui-color-default-contrast);
    }
    .n3o-data-import .btn--primary.btn--positive {
        background: var(--uui-color-positive);
        color: var(--uui-color-positive-contrast);
    }
    .n3o-data-import .btn--primary:hover:not(:disabled) {
        background: var(--uui-color-positive-emphasis, var(--uui-color-positive));
    }
    .n3o-data-import .btn:disabled {
        opacity: 0.5;
        cursor: not-allowed;
    }
`;
