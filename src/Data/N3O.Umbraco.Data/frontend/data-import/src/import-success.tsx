import { UuiButton } from '@n3oltd/backoffice-ui';

interface ImportSuccessProps {
    onStartOver: () => void;
}

export function ImportSuccess({ onStartOver }: ImportSuccessProps) {
    return (
        <uui-box headline="Import queued">
            <p className="boxHint">Your CSV file has been queued and will be processed shortly.</p>
            <div className="actions">
                <UuiButton label="View import queue" look="primary" href="/umbraco/section/content/dashboard/imports" />
                <UuiButton label="Import another file" look="secondary" onClick={onStartOver} />
            </div>
        </uui-box>
    );
}
