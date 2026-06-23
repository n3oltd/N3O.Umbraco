import { UuiButton, UuiCheckbox } from '@n3oltd/backoffice-ui';

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
                        <UuiButton label="Select all" look="secondary" compact disabled={processing} onClick={onSelectAll} />
                        <UuiButton label="Clear" look="secondary" compact disabled={processing} onClick={onClear} />
                    </div>

                    <div className="checkboxGrid">
                        {items.map((item) => (
                            <UuiCheckbox
                                key={getKey(item)}
                                label={getLabel(item)}
                                checked={!!getChecked(item)}
                                disabled={processing}
                                onChange={(checked) => onToggle(item, checked)}
                            />
                        ))}
                    </div>
                </>
            )}
        </uui-box>
    );
}
