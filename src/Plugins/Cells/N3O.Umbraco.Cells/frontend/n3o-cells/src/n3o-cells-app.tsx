import { useEffect, useRef } from 'react';
import Handsontable from 'handsontable';
import handsontableStyles from 'handsontable/dist/handsontable.full.min.css?inline';

export type CellsValue = unknown[][] | undefined;

type N3oCellsAppProps = {
    value: CellsValue;
    gridConfiguration: Record<string, unknown>;
    onChange: (value: unknown[][]) => void;
};

// Handsontable writes edits into the array it is given, so it never receives the stored value itself.
function copyGrid(data: unknown[][] | undefined): unknown[][] | undefined {
    return data ? structuredClone(data) : undefined;
}

export function N3oCellsApp({ value, gridConfiguration, onChange }: N3oCellsAppProps) {
    const containerRef = useRef<HTMLDivElement>(null);
    const hotRef = useRef<Handsontable | null>(null);
    const onChangeRef = useRef(onChange);
    onChangeRef.current = onChange;
    const valueRef = useRef(value);
    valueRef.current = value;

    useEffect(() => {
        const container = containerRef.current;

        if (!container) {
            return;
        }

        const data = copyGrid(valueRef.current ?? (gridConfiguration.data as unknown[][] | undefined));

        const globalConfig: Handsontable.GridSettings = {
            licenseKey: 'non-commercial-and-evaluation',
            height: 'auto',
            width: 'auto',
            data: data,
            afterChange: (_change: Handsontable.CellChange[] | null, source: Handsontable.ChangeSource) => {
                if (source !== 'loadData') {
                    onChangeRef.current(hot.getData() as unknown[][]);
                }
            },
        };

        const hot = new Handsontable(container, { ...gridConfiguration, ...globalConfig });

        hotRef.current = hot;

        return () => {
            hotRef.current = null;

            hot.destroy();
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [gridConfiguration]);

    useEffect(() => {
        const hot = hotRef.current;

        if (!hot || value === undefined) {
            return;
        }

        if (JSON.stringify(hot.getData()) !== JSON.stringify(value)) {
            hot.loadData(structuredClone(value));
        }
    }, [value]);

    return (
        <>
            <div id="grid" ref={containerRef}></div>
            <style>{handsontableStyles}</style>
        </>
    );
}
