import { useState } from 'react';
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
    const [pendingDelete, setPendingDelete] = useState<string | null>(null);

    if (!value.length) {
        return null;
    }

    function requestDelete(source: string): void {
        setPendingDelete(source);
    }

    function confirmDelete(source: string): void {
        setPendingDelete(null);
        onChange(value.filter((entry) => entry.source !== source));
    }

    function cancelDelete(): void {
        setPendingDelete(null);
    }

    function updateCustom(source: string, custom: string): void {
        onChange(value.map((entry) => (entry.source === source ? { ...entry, custom } : entry)));
    }

    return (
        <uui-box headline="Text resources">
            <div className="n3o-text-resource-editor">
                {value.map((entry) => (
                    <div className="row-wrapper" key={entry.source}>
                        <div className="row-1">
                            {pendingDelete === entry.source ? (
                                <>
                                    <span>Delete this entry? </span>
                                    <button
                                        type="button"
                                        className="delete-confirm"
                                        onClick={() => confirmDelete(entry.source)}
                                    >
                                        Yes
                                    </button>
                                    {' '}
                                    <button
                                        type="button"
                                        className="delete-cancel"
                                        onClick={cancelDelete}
                                    >
                                        No
                                    </button>
                                </>
                            ) : (
                                <>
                                    [
                                    <button
                                        type="button"
                                        className="delete"
                                        aria-label={`Delete ${entry.source}`}
                                        onClick={() => requestDelete(entry.source)}
                                    >
                                        x
                                    </button>
                                    ] <span className="text">{entry.source}</span>
                                </>
                            )}
                        </div>
                        <div className="row-2">
                            <input
                                type="text"
                                className="custom"
                                value={entry.custom ?? ''}
                                onChange={(e) =>
                                    updateCustom(entry.source, e.currentTarget.value)
                                }
                            />
                        </div>
                    </div>
                ))}
            </div>

            <style>{styles}</style>
        </uui-box>
    );
}
