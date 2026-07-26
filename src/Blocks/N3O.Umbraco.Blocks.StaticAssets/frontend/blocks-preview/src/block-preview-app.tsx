import styles from './block-preview-app.css?inline';

interface BlockPreviewAppProps {
    loaded: boolean;
    markup: string;
}

export function BlockPreviewApp({ loaded, markup }: BlockPreviewAppProps) {
    return (
        <>
            {!loaded ? (
                <div className="preview-alert preview-alert-info">
                    <uui-loader style={{ color: '#fff' }}></uui-loader>
                    Loading preview...
                </div>
            ) : (
                <div className="block-preview-frame" dangerouslySetInnerHTML={{ __html: markup }} />
            )}

            <style>{styles}</style>
        </>
    );
}
