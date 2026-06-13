import styles from './import-notices-viewer-app.css?inline';

export interface ImportNoticesValue {
    errors: string[];
    warnings: string[];
}

interface ImportNoticesViewerAppProps {
    value: ImportNoticesValue | undefined;
}

// React UI for the read-only import notices viewer. Controlled by the host web component: `value`
// comes in as a prop. Display only - no change event. Renders errors then warnings, or a placeholder
// when there are none.
export function ImportNoticesViewerApp({ value }: ImportNoticesViewerAppProps) {
    const errors = value?.errors ?? null;
    const warnings = value?.warnings ?? null;

    const hasErrors = !!errors && errors.length > 0;
    const hasWarnings = !!warnings && warnings.length > 0;

    return (
        <div className="n3o-import-errors-viewer">
            {hasErrors ? (
                <>
                    <p>
                        <em className="text-error">Errors</em>
                    </p>
                    {errors!.map((error, index) => (
                        <div className="row-wrapper" key={`error-${index}`}>
                            <div className="row">{error}</div>
                        </div>
                    ))}
                </>
            ) : null}

            {hasWarnings ? (
                <>
                    <p>
                        <em className="text-warning">Warnings</em>
                    </p>
                    {warnings!.map((warning, index) => (
                        <div className="row-wrapper" key={`warning-${index}`}>
                            <div className="row">{warning}</div>
                        </div>
                    ))}
                </>
            ) : null}

            {!hasErrors && !hasWarnings ? (
                <div className="row-wrapper">
                    <div className="row">No warnings or errors</div>
                </div>
            ) : null}

            <style>{styles}</style>
        </div>
    );
}
