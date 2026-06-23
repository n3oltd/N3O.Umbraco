import { UuiButton } from '@n3oltd/backoffice-ui';

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
                <UuiButton label="View import queue" look="primary" href="/umbraco/section/content/dashboard/imports" />
                <UuiButton label="Import another file" look="secondary" onClick={onStartOver} />
            </div>
        </uui-box>
    );
}
