import type { ContentType } from './types';

interface ExportOptionsProps {
    contentTypes: ContentType[];
    contentType: ContentType | null;
    format: string;
    includeUnpublished: boolean;
    processing: boolean;
    onContentTypeChange: (event: React.ChangeEvent<HTMLSelectElement>) => void;
    onFormatChange: (value: string) => void;
    onIncludeUnpublishedChange: (checked: boolean) => void;
}

export function ExportOptions({
    contentTypes,
    contentType,
    format,
    includeUnpublished,
    processing,
    onContentTypeChange,
    onFormatChange,
    onIncludeUnpublishedChange,
}: ExportOptionsProps) {
    return (
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
                                onChange={(e) => onFormatChange(e.target.value)}
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
                                onChange={(e) => onFormatChange(e.target.value)}
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
                            onChange={(e) => onIncludeUnpublishedChange(e.target.checked)}
                            disabled={processing}
                        />
                        <span>Include unpublished content</span>
                    </label>
                </div>
            </umb-property-layout>
        </uui-box>
    );
}