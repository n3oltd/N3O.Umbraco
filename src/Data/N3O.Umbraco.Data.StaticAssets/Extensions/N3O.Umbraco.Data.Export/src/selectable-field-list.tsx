interface SelectableFieldListProps<T> {
    headline: string;
    selectedCount: number;
    items: T[];
    getKey: (item: T) => string;
    getLabel: (item: T) => string;
    getChecked: (item: T) => boolean;
    processing: boolean;
    onToggle: (item: T, checked: boolean) => void;
    onSelectAll: () => void;
    onClear: () => void;
    emptyState: React.ReactNode;
}

export function SelectableFieldList<T>({
    headline,
    selectedCount,
    items,
    getKey,
    getLabel,
    getChecked,
    processing,
    onToggle,
    onSelectAll,
    onClear,
    emptyState,
}: SelectableFieldListProps<T>) {
    return (
        <uui-box headline={headline}>
            <div slot="header-actions" className="selectionCount">
                {selectedCount} selected
            </div>

            {items.length === 0 ? (
                emptyState
            ) : (
                <>
                    <div className="selectionActions">
                        <button
                            type="button"
                            className="btn btn--secondary btn--compact"
                            disabled={processing}
                            onClick={onSelectAll}>
                            Select all
                        </button>
                        <button
                            type="button"
                            className="btn btn--secondary btn--compact"
                            disabled={processing}
                            onClick={onClear}>
                            Clear
                        </button>
                    </div>

                    <div className="checkboxGrid">
                        {items.map((item) => (
                            <label key={getKey(item)} className="checkOption">
                                <input
                                    type="checkbox"
                                    checked={!!getChecked(item)}
                                    onChange={(e) => onToggle(item, e.target.checked)}
                                    disabled={processing}
                                />
                                <span>{getLabel(item)}</span>
                            </label>
                        ))}
                    </div>
                </>
            )}
        </uui-box>
    );
}