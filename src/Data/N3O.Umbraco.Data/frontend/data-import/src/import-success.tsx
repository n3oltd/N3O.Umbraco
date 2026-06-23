interface ImportSuccessProps {
    onStartOver: () => void;
}

export function ImportSuccess({ onStartOver }: ImportSuccessProps) {
    return (
        <uui-box headline="Import queued">
            <div className="statusBox statusBox--positive">
                <uui-icon name="icon-check"></uui-icon>
                <span>Your CSV file has been queued and will be processed shortly.</span>
            </div>
            <div className="actions">
                <a className="btn btn--primary" href="/umbraco/section/content/dashboard/imports">
                    View import queue
                </a>
                <button type="button" className="btn btn--secondary" onClick={onStartOver}>
                    Import another file
                </button>
            </div>
        </uui-box>
    );
}
