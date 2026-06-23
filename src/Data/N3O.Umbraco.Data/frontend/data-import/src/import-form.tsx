import { UuiButton, UuiCheckbox, UuiSelect, UuiToggle } from '@n3oltd/backoffice-ui';
import type { ContentType, DatePattern, ImportableProperty } from './types';

interface ImportFormProps {
    processing: boolean;
    contentTypes: ContentType[];
    contentType: ContentType | null;
    datePatterns: DatePattern[];
    datePattern: DatePattern | null;
    moveUpdatedContentToCurrentLocation: boolean;
    importableProperties: ImportableProperty[];
    selectedPropertyCount: number;
    csvFileRef: React.RefObject<HTMLInputElement | null>;
    zipFileRef: React.RefObject<HTMLInputElement | null>;
    onContentTypeChange: (alias: string) => void;
    onDatePatternChange: (id: string) => void;
    onMoveUpdatedChange: (checked: boolean) => void;
    onPropertyToggle: (property: ImportableProperty, checked: boolean) => void;
    onSelectAllProperties: () => void;
    onClearSelectedProperties: () => void;
    onGetTemplate: () => void;
    onImport: () => void;
}

export function ImportForm({
    processing,
    contentTypes,
    contentType,
    datePatterns,
    datePattern,
    moveUpdatedContentToCurrentLocation,
    importableProperties,
    selectedPropertyCount,
    csvFileRef,
    zipFileRef,
    onContentTypeChange,
    onDatePatternChange,
    onMoveUpdatedChange,
    onPropertyToggle,
    onSelectAllProperties,
    onClearSelectedProperties,
    onGetTemplate,
    onImport,
}: ImportFormProps) {
    return (
        <>
            <uui-box headline="1. Choose what to import">
                <umb-property-layout
                    label="Content type"
                    description="The child type that rows in your CSV will be imported as."
                    mandatory>
                    <div slot="editor">
                        <UuiSelect
                            options={contentTypes.map((item) => ({ name: item.name, value: item.alias }))}
                            value={contentType?.alias ?? ''}
                            placeholder="Select a content type…"
                            disabled={processing || contentTypes.length === 0}
                            onChange={onContentTypeChange}
                        />
                    </div>
                </umb-property-layout>

                <umb-property-layout
                    label="Date pattern"
                    description="How dates in your CSV are formatted, so they can be parsed correctly."
                    mandatory>
                    <div slot="editor">
                        <UuiSelect
                            options={datePatterns.map((item) => ({ name: item.name, value: item.id }))}
                            value={datePattern?.id ?? ''}
                            disabled={processing || datePatterns.length === 0}
                            onChange={onDatePatternChange}
                        />
                    </div>
                </umb-property-layout>

                <umb-property-layout
                    label="Move updated content"
                    description="When enabled, existing content that is updated will be moved beneath the current item.">
                    <div slot="editor">
                        <UuiToggle
                            label="Move updated content to the current location"
                            checked={moveUpdatedContentToCurrentLocation}
                            disabled={processing}
                            onChange={onMoveUpdatedChange}
                        />
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
                            <UuiButton label="Select all" look="secondary" compact disabled={processing} onClick={onSelectAllProperties} />
                            <UuiButton label="Clear" look="secondary" compact disabled={processing} onClick={onClearSelectedProperties} />
                        </div>

                        <div className="checkboxGrid">
                            {importableProperties.map((property) => (
                                <UuiCheckbox
                                    key={property.alias}
                                    label={property.columnTitle}
                                    checked={!!property.selected}
                                    disabled={processing}
                                    onChange={(checked) => onPropertyToggle(property, checked)}
                                />
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
                <UuiButton
                    label="Download template"
                    look="secondary"
                    disabled={!contentType || selectedPropertyCount === 0 || processing}
                    onClick={onGetTemplate}>
                    <uui-icon name="icon-download-alt"></uui-icon>
                    Download template
                </UuiButton>
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
                    <UuiButton
                        label={processing ? 'Importing…' : 'Import'}
                        look="primary"
                        color="positive"
                        disabled={!contentType || processing}
                        onClick={onImport}
                    />
                </div>
            </uui-box>
        </>
    );
}
