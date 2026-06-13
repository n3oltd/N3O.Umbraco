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

const styles = `
    :host {
        display: block;
    }

    .block-preview-frame {
        display: block;
        width: 100%;
        border: none;
        transform: scale(0.9);
        transform-origin: top left;
    }

    .preview-alert {
        background-color: #f0ac00;
        border: 1px solid transparent;
        border-radius: 0;
        margin-bottom: 20px;
        padding: 8px 35px 8px 14px;
        position: relative;
    }

    .preview-alert uui-loader {
        margin-right: 16px;
    }

    .preview-alert,
    .preview-alert a,
    .preview-alert h4 {
        color: #fff;
    }

    .preview-alert pre {
        white-space: normal;
    }

    .preview-alert-warning {
        background-color: #f0ac00;
        border-color: transparent;
        color: #fff;
    }

    .preview-alert-info {
        background-color: #3544b1;
        border-color: transparent;
        color: #fff;
    }

    .preview-alert-danger,
    .preview-alert-error {
        background-color: #d42054;
        border-color: transparent;
        color: #fff;
    }
`;
