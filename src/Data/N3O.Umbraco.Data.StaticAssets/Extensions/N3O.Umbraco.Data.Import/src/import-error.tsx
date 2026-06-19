interface ImportErrorProps {
    errorMessages: string[] | null;
    onStartOver: () => void;
}

export function ImportError({ errorMessages, onStartOver }: ImportErrorProps) {
    return (
        <uui-box headline="Import failed">
            <div className="statusBox statusBox--danger">
                <uui-icon name="icon-alert"></uui-icon>
                <div>
                    {errorMessages && errorMessages.length > 0 ? (
                        <ul className="errorList">
                            {errorMessages.map((message) => (
                                <li key={message}>{message}</li>
                            ))}
                        </ul>
                    ) : (
                        <span>Something went wrong while queueing the import.</span>
                    )}
                </div>
            </div>
            <div className="actions">
                <button type="button" className="btn btn--secondary" onClick={onStartOver}>
                    Start over
                </button>
            </div>
        </uui-box>
    );
}
