import styles from './text-resource-editor-app.css?inline';

export interface TextResourceEntry {
    source: string;
    custom: string | null | undefined;
}

interface TextResourceEditorAppProps {
    value: TextResourceEntry[];
    onChange: (value: TextResourceEntry[]) => void;
}

// React UI for the text resource override property editor. Controlled by the host web component:
// `value` (an array of { source, custom } entries) comes in as a prop and is the single source of
// truth — edits are pushed back out via `onChange` (the host then raises UmbPropertyValueChangeEvent).
// Each entry shows the read-only source text, a delete affordance, and an input bound to `custom`.
export function TextResourceEditorApp({ value, onChange }: TextResourceEditorAppProps) {
    if (!value.length) {
        return null;
    }

    function deleteEntry(index: number): void {
        if (!confirm('Are you sure you wish to delete this entry?')) {
            return;
        }

        onChange(value.filter((_, i) => i !== index));
    }

    function updateCustom(index: number, custom: string): void {
        onChange(value.map((entry, i) => (i === index ? { ...entry, custom } : entry)));
    }

    return (
        <uui-box headline="Text resources">
            <div className="n3o-text-resource-editor">
                {value.map((entry, index) => (
                    <div className="row-wrapper" key={`${entry.source}-${index}`}>
                        <div className="row-1">
                            [
                            <a className="delete" onClick={() => deleteEntry(index)}>
                                x
                            </a>
                            ] <span className="text">{entry.source}</span>
                        </div>
                        <div className="row-2">
                            <uui-input
                                type="text"
                                class="custom"
                                value={entry.custom ?? ''}
                                onInput={(e: Event) =>
                                    updateCustom(index, (e.target as HTMLInputElement).value)
                                }
                            ></uui-input>
                        </div>
                    </div>
                ))}
            </div>

            <style>{styles}</style>
        </uui-box>
    );
}
