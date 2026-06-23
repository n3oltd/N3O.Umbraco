import styles from './import-data-editor-app.css?inline';

export interface ImportField {
    name: string;
    value: string | null;
    sourceValue: string | null;
    isFile: boolean;
}

export interface ImportDataValue {
    reference: string;
    fields: ImportField[];
}

interface ImportDataEditorAppProps {
    value: ImportDataValue | undefined;
    onTextChange: (index: number, value: string) => void;
    onFileSelected: (index: number, file: File) => void;
}

export function ImportDataEditorApp({ value, onTextChange, onFileSelected }: ImportDataEditorAppProps) {
    const fields = value?.fields ?? [];

    return (
        <div className="n3o-import-fields-editor">
            {fields.map((field, index) => (
                <div className="row-wrapper" key={field.name}>
                    <div className="row-1">
                        <span className="text">{field.name}</span>
                    </div>

                    <div className="row-2">
                        <input
                            type="text"
                            className="custom"
                            value={field.value ?? ''}
                            placeholder={field.sourceValue ?? ''}
                            onChange={(e) => onTextChange(index, e.currentTarget.value)}
                        />

                        {field.isFile ? (
                            <input
                                type="file"
                                onChange={(e) => {
                                    const file = (e.target as HTMLInputElement).files?.[0];
                                    if (file) {
                                        onFileSelected(index, file);
                                    }
                                }}
                            />
                        ) : null}
                    </div>
                </div>
            ))}

            <style>{styles}</style>
        </div>
    );
}
