import { UuiButton } from '@n3oltd/backoffice-ui';

interface ImportErrorProps {
    errorMessages: string[] | null;
    onStartOver: () => void;
}

export function ImportError({ errorMessages, onStartOver }: ImportErrorProps) {
    return (
        <uui-box headline="Import failed">
            {errorMessages && errorMessages.length > 0 ? (
                <ul className="errorList">
                    {errorMessages.map((message) => (
                        <li key={message}>{message}</li>
                    ))}
                </ul>
            ) : (
                <p className="boxHint">Something went wrong while queueing the import.</p>
            )}
            <div className="actions">
                <UuiButton label="Start over" look="secondary" onClick={onStartOver} />
            </div>
        </uui-box>
    );
}
