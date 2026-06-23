import { useRef, useState } from 'react';
import type { AuthFetch } from '@n3oltd/backoffice-core';
import { useImportLookups } from './use-import-lookups';
import { ImportForm } from './import-form';
import { ImportSuccess } from './import-success';
import { ImportError } from './import-error';
import type { ContentType, ImportableProperty } from './types';
import styles from './data-import-app.css?inline';

interface DataImportAppProps {
    contentKey: string | null;
    authFetch: AuthFetch | null;
}

export function DataImportApp({ contentKey, authFetch }: DataImportAppProps) {
    const [show, setShow] = useState<string>('form');
    const [processing, setProcessing] = useState<boolean>(false);
    const [contentType, setContentType] = useState<ContentType | null>(null);
    const [moveUpdatedContentToCurrentLocation, setMoveUpdatedContentToCurrentLocation] = useState<boolean>(false);
    const [importableProperties, setImportableProperties] = useState<ImportableProperty[]>([]);
    const [errorMessages, setErrorMessages] = useState<string[] | null>(null);

    const csvFileRef = useRef<HTMLInputElement>(null);
    const zipFileRef = useRef<HTMLInputElement>(null);

    const importAbortRef = useRef<AbortController | null>(null);

    const { contentTypes, datePatterns, datePattern, setDatePattern } = useImportLookups(contentKey, authFetch);

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

    const cancelImport = (): void => {
        importAbortRef.current?.abort();
        importAbortRef.current = null;
    };

    const startOver = (): void => {
        cancelImport();
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
        link.dispatchEvent(new MouseEvent('click', { bubbles: false, cancelable: false }));
        link.parentNode?.removeChild(link);
        window.URL.revokeObjectURL(blobUrl);
    };

    const getStorageToken = async (input: HTMLInputElement, signal: AbortSignal): Promise<unknown> => {
        if (!input.files || input.files.length === 0) {
            return null;
        }

        const data = new FormData();
        data.append('file', input.files[0]);

        const res = await authFetch!('/umbraco/api/Storage/tempUpload', {
            method: 'POST',
            body: data,
            signal,
        });

        return await res.json();
    };

    const doImport = async (): Promise<void> => {
        cancelImport();
        const controller = new AbortController();
        importAbortRef.current = controller;
        const { signal } = controller;

        setProcessing(true);

        try {
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

            const csvStorageToken = await getStorageToken(csvFile, signal);
            if (signal.aborted) return;

            const zipStorageToken = zipFile ? await getStorageToken(zipFile, signal) : null;
            if (signal.aborted) return;

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
                    signal,
                }
            );

            if (signal.aborted) return;

            if (result.status === 200) {
                setShow('success');
                setProcessing(false);
            } else {
                processingError((await result.json()) as string | string[]);
            }
        } catch (err) {
            if (err instanceof DOMException && err.name === 'AbortError') return;
            processingError('An unexpected error occurred while queueing the import.');
        } finally {
            if (importAbortRef.current === controller) {
                importAbortRef.current = null;
            }
        }
    };

    const selectedPropertyCount = importableProperties.filter((p) => p.selected).length;

    return (
        <div className="n3o-data-import">
            {show === 'success' ? (
                <ImportSuccess onStartOver={startOver} />
            ) : show === 'error' ? (
                <ImportError errorMessages={errorMessages} onStartOver={startOver} />
            ) : (
                <ImportForm
                    processing={processing}
                    contentTypes={contentTypes}
                    contentType={contentType}
                    datePatterns={datePatterns}
                    datePattern={datePattern}
                    moveUpdatedContentToCurrentLocation={moveUpdatedContentToCurrentLocation}
                    importableProperties={importableProperties}
                    selectedPropertyCount={selectedPropertyCount}
                    csvFileRef={csvFileRef}
                    zipFileRef={zipFileRef}
                    onContentTypeChange={onContentTypeChange}
                    onDatePatternChange={onDatePatternChange}
                    onMoveUpdatedChange={setMoveUpdatedContentToCurrentLocation}
                    onPropertyToggle={onPropertyToggle}
                    onSelectAllProperties={selectAllProperties}
                    onClearSelectedProperties={clearSelectedProperties}
                    onGetTemplate={() => void getTemplate()}
                    onImport={() => void doImport()}
                />
            )}
            <style>{styles}</style>
        </div>
    );
}
