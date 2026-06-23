import { UuiRadio, UuiRadioGroup, UuiSelect, UuiToggle } from '@n3oltd/backoffice-ui';
import type { ContentType } from './types';

interface ExportOptionsProps {
    contentTypes: ContentType[];
    contentType: ContentType | null;
    format: string;
    includeUnpublished: boolean;
    processing: boolean;
    onContentTypeChange: (alias: string) => void;
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
                label="File format"
                description="Choose the file format for the exported data."
                mandatory>
                <div slot="editor">
                    <UuiRadioGroup name="format" value={format} disabled={processing} onChange={onFormatChange}>
                        <UuiRadio label="Excel (.xlsx)" value="excel" />
                        <UuiRadio label="CSV (.csv)" value="csv" />
                    </UuiRadioGroup>
                </div>
            </umb-property-layout>

            <umb-property-layout
                label="Include unpublished"
                description="When enabled, unpublished content is included in the export.">
                <div slot="editor">
                    <UuiToggle
                        label="Include unpublished content"
                        checked={includeUnpublished}
                        disabled={processing}
                        onChange={onIncludeUnpublishedChange}
                    />
                </div>
            </umb-property-layout>
        </uui-box>
    );
}
