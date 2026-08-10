import { useEffect, useRef } from 'react';
import Handsontable from 'handsontable';
// Handsontable CSS is imported as an inlined string so Vite bundles it into the JS output
// rather than emitting a separate .css file. It is injected into the shadow root via a <style>
// tag below, matching the original Lit component's behaviour (scoped, no extra network request).
import handsontableStyles from 'handsontable/dist/handsontable.full.min.css?inline';

export type CellsValue = unknown[][] | undefined;

interface N3oCellsAppProps {
    value: CellsValue;
    // Parsed `gridConfiguration` prevalue (columns, default data, etc.).
    gridConfiguration: Record<string, unknown>;
    onChange: (value: unknown[][]) => void;
}

// React UI for the Cells (Handsontable) property editor. Controlled by the host web component:
// `value` comes in as a prop, edits are pushed back out via `onChange` (the host then raises
// UmbPropertyValueChangeEvent). Handsontable is a non-React imperative library, so it is created
// inside a useEffect against a container ref and destroyed in the effect cleanup. It is NOT
// re-created on every render — the effect depends only on the inputs that require a fresh grid.
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

        // Fall back to the configured default data when the host has no stored value.
        const data = (value ?? (gridConfiguration.data as unknown[][] | undefined)) as unknown[][] | undefined;

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
