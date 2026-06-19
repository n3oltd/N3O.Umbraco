import styles from './block-preview-app.css?inline';

interface BlockPreviewAppProps {
    loaded: boolean;
    markup: string;
}

// React UI for the block grid custom-view preview. Controlled by the host web component: the host
// consumes the Umbraco block contexts, POSTs the block grid value to the preview endpoint and passes
// the server-rendered HTML markup in via the `markup` prop. The markup is server-rendered preview HTML
// (same trust level as the original iframe document.write) so it is bound via dangerouslySetInnerHTML.
// Hybrid UI: uui-loader for the backoffice-standard loading chrome + the bespoke preview surface.
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
