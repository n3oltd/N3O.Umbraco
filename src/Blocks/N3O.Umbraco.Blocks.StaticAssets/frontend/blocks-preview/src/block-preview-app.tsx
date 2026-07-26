import styles from './block-preview-app.css?inline';
import type { PreviewState } from './types';

interface BlockPreviewAppProps {
    state: PreviewState;
}

export function BlockPreviewApp({ state }: BlockPreviewAppProps) {
    return (
        <>
            {state.status === 'loading' ? (
                <div className="preview-alert preview-alert-info">
                    <uui-loader style={{ color: '#fff' }}></uui-loader>
                    Loading preview...
                </div>
            ) : null}

            {state.status === 'error' ? (
                <div className="preview-alert preview-alert-error">{state.message}</div>
            ) : null}

            {state.status === 'ready' ? (
                <div className="block-preview-frame" dangerouslySetInnerHTML={{ __html: state.markup }} />
            ) : null}

            <style>{styles}</style>
        </>
    );
}
