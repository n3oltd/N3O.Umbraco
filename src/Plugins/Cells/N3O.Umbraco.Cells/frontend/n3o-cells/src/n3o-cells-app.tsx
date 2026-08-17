import { useEffect, useRef } from 'react';
import Handsontable from 'handsontable';
// ?inline keeps the stylesheet in the JS bundle instead of a separate .css file, so it can be injected
// into the shadow root below. A linked stylesheet would not reach inside the shadow boundary.
import handsontableStyles from 'handsontable/dist/handsontable.full.min.css?inline';

export type CellsValue = unknown[][] | undefined;

interface N3oCellsAppProps {
    value: CellsValue;
    // Parsed `gridConfiguration` prevalue (columns, default data, etc.).
    gridConfiguration: Record<string, unknown>;
    onChange: (value: unknown[][]) => void;
}

// Handsontable is imperative and owns its own DOM, so it lives in an effect against a container ref
// rather than being described by the render tree.
export function N3oCellsApp({ value, gridConfiguration, onChange }: N3oCellsAppProps) {
    const containerRef = useRef<HTMLDivElement>(null);
    // Keep the latest onChange in a ref so the change hook always calls the current callback
    // without forcing the grid to be re-initialised when the parent passes a new function identity.
    const onChangeRef = useRef(onChange);
    onChangeRef.current = onChange;

    useEffect(() => {
        const container = containerRef.current;

        if (!container) {
            return;
        }

        const data =(value ?? (gridConfiguration.data as unknown[][] | undefined)) as unknown[][] | undefined;

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

        return () => {
            hot.destroy();
        };
        // Re-initialise only when the grid configuration changes. The host's `value` updates flow
        // through Handsontable's own change hooks (single source of truth = the host element), so
        // depending on `value` here would needlessly destroy/recreate the grid on every edit.
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [gridConfiguration]);

    return (
        <>
            <div id="grid" ref={containerRef}></div>
            <style>{handsontableStyles}</style>
        </>
    );
}
