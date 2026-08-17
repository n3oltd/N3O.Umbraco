import { useState } from 'react';
import styles from './text-resource-editor-app.css?inline';

export interface TextResourceEntry {
    source: string;
    custom?: string | null;
}

type TextResourceEditorAppProps = {
    value: TextResourceEntry[];
    onChange: (value: TextResourceEntry[]) => void;
};

export function TextResourceEditorApp({ value, onChange }: TextResourceEditorAppProps) {
    const [pendingDelete, setPendingDelete] = useState<number | null>(null);

    if (!value.length) {
        return null;
    }

    function requestDelete(index: number): void {
        setPendingDelete(index);
    }

    function confirmDelete(index: number): void {
        setPendingDelete(null);
        onChange(value.filter((_, entryIndex) => entryIndex !== index));
    }

    function cancelDelete(): void {
        setPendingDelete(null);
    }

    function updateCustom(index: number, custom: string): void {
        onChange(value.map((entry, entryIndex) => (entryIndex === index ? { ...entry, custom } : entry)));
    }

    return (
        <>
            <div className="n3o-text-resource-editor">
                {value.map((entry, index) => (
                    <div className="row-wrapper" key={index}>
                        <div className="row-1">
                            {pendingDelete === index ? (
                                <>
                                    <span>Delete this entry? </span>
                                    <button
                                        type="button"
                                        className="delete-confirm"
                                        onClick={() => confirmDelete(index)}
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
                                        onClick={() => requestDelete(index)}
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
                                    updateCustom(index, e.currentTarget.value)
                                }
                            />
                        </div>
                    </div>
                ))}
            </div>

            <style>{styles}</style>
        </>
    );
}
