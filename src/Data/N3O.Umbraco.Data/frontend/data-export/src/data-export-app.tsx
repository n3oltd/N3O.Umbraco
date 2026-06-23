import { useEffect, useState } from 'react';
import { UuiButton } from '@n3oltd/backoffice-ui';
import type { AuthFetch } from '@n3oltd/backoffice-core';
import styles from './data-export-app.css?inline';
import { useExportServerData, useExportRun } from './use-export';
import { ExportOptions } from './export-options';
import { SelectableFieldList } from './selectable-field-list';
import type { ContentType, ContentMetadata, ExportableProperty, Notify } from './types';

interface DataExportAppProps {
    contentKey: string | null;
    authFetch: AuthFetch | null;
    notify: Notify;
}

export function DataExportApp({ contentKey, authFetch, notify }: DataExportAppProps) {
    const { contentTypes, metadatas: initialMetadatas } = useExportServerData(contentKey, authFetch);
    const { processing, progress, doExport } = useExportRun(authFetch, notify);

    const [contentType, setContentType] = useState<ContentType | null>(null);
    const [format, setFormat] = useState<string>('excel');
    const [includeUnpublished, setIncludeUnpublished] = useState<boolean>(false);
    const [metadatas, setMetadatas] = useState<ContentMetadata[]>([]);
    const [exportableProperties, setExportableProperties] = useState<ExportableProperty[]>([]);

    useEffect(() => {
        setMetadatas(initialMetadatas);
    }, [initialMetadatas]);

    const refreshProperties = async (selected: ContentType | null): Promise<void> => {
        if (!selected || !authFetch) {
            setExportableProperties([]);
            return;
        }

        const res = (await authFetch(`/umbraco/backoffice/api/Exports/exportableProperties/${selected.alias}`, {
            headers: { Accept: 'application/json' },
        }).then((r) => r.json())) as ExportableProperty[];

        for (const property of res) {
            property.selected = false;
        }

        setExportableProperties(res);
    };

    const onContentTypeChange = (alias: string): void => {
        const selected = contentTypes.find((x) => x.alias === alias) ?? null;
        setContentType(selected);
        void refreshProperties(selected);
    };

    const selectAllMetadatas = (): void => setMetadatas((prev) => prev.map((m) => ({ ...m, selected: true })));
    const clearSelectedMetadatas = (): void => setMetadatas((prev) => prev.map((m) => ({ ...m, selected: false })));
    const selectAllProperties = (): void => setExportableProperties((prev) => prev.map((p) => ({ ...p, selected: true })));
    const clearSelectedProperties = (): void => setExportableProperties((prev) => prev.map((p) => ({ ...p, selected: false })));
    const toggleMetadata = (metadata: ContentMetadata, checked: boolean): void => setMetadatas((prev) => prev.map((m) => (m === metadata ? { ...m, selected: checked } : m)));
    const toggleProperty = (property: ExportableProperty, checked: boolean): void => setExportableProperties((prev) => prev.map((p) => (p === property ? { ...p, selected: checked } : p)));

    const selectedMetadataCount = metadatas.filter((m) => m.selected).length;
    const selectedPropertyCount = exportableProperties.filter((p) => p.selected).length;
    const hasSelection = selectedMetadataCount > 0 || selectedPropertyCount > 0;
    const canExport = !!contentType && hasSelection && !processing && !!authFetch;

    return (
        <div className="n3o-data-export">
            <ExportOptions
                contentTypes={contentTypes}
                contentType={contentType}
                format={format}
                includeUnpublished={includeUnpublished}
                processing={processing}
                onContentTypeChange={onContentTypeChange}
                onFormatChange={setFormat}
                onIncludeUnpublishedChange={setIncludeUnpublished}
            />

            <SelectableFieldList
                headline="Metadata fields"
                selectedCount={selectedMetadataCount}
                items={metadatas}
                getKey={(m) => m.id}
                getLabel={(m) => m.name}
                getChecked={(m) => m.selected}
                processing={processing}
                onToggle={toggleMetadata}
                onSelectAll={selectAllMetadatas}
                onClear={clearSelectedMetadatas}
                emptyState={<p className="emptyState">No metadata fields are available.</p>}
            />

            <SelectableFieldList
                headline="Properties"
                selectedCount={selectedPropertyCount}
                items={exportableProperties}
                getKey={(p) => p.alias}
                getLabel={(p) => p.columnTitle}
                getChecked={(p) => p.selected}
                processing={processing}
                onToggle={toggleProperty}
                onSelectAll={selectAllProperties}
                onClear={clearSelectedProperties}
                emptyState={
                    !contentType ? (
                        <p className="emptyState">Select a content type to see its exportable properties.</p>
                    ) : (
                        <p className="emptyState">This content type has no exportable properties.</p>
                    )
                }
            />

            {!processing && contentType && !hasSelection ? (
                <p className="hint">Select at least one metadata field or property to export.</p>
            ) : null}

            {processing ? (
                <div className="progress">
                    <uui-loader-bar></uui-loader-bar>
                    <span>{progress || 'Preparing export…'}</span>
                </div>
            ) : null}

            <div className="actions">
                <UuiButton
                    label={processing ? 'Exporting…' : 'Export'}
                    look="primary"
                    color="positive"
                    disabled={!canExport}
                    onClick={() => void doExport(
                        contentKey,
                        contentType?.alias ?? '',
                        format,
                        includeUnpublished,
                        metadatas.filter((x) => x.selected).map((x) => x.id),
                        exportableProperties.filter((x) => x.selected).map((x) => x.alias),
                    )}
                />
            </div>

            <style>{styles}</style>
        </div>
    );
}
