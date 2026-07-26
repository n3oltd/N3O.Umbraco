import { useCallback, useEffect, useRef, useState } from 'react';
import styles from './block-preview-app.css?inline';
import type { PreviewState } from './types';

interface BlockPreviewAppProps {
    state: PreviewState;
}

// The preview is drawn slightly smaller than life so more of the block fits in the editor. transform does not
// affect layout, so the surface is given the scaled height and the frame is widened to compensate; otherwise
// every block reserves its full unscaled height and leaves a gap beneath.
const previewScale = 0.9;

// The markup is a whole rendered page, so it goes in an iframe rather than into the backoffice document:
// scripts a block needs in order to render run, and the site's own stylesheet applies as it does on the live
// page. srcDoc keeps the frame same-origin, so its content height can be measured to size the frame.
function PreviewFrame({ markup }: { markup: string }) {
    const frameRef = useRef<HTMLIFrameElement>(null);
    const [height, setHeight] = useState(0);

    const measure = useCallback(() => {
        const body = frameRef.current?.contentDocument?.body;

        if (body) {
            setHeight(body.scrollHeight);
        }
    }, []);

    useEffect(() => {
        const body = frameRef.current?.contentDocument?.body;

        if (!body) {
            return;
        }

        const observer = new ResizeObserver(measure);
        observer.observe(body);

        return () => observer.disconnect();
    }, [markup, measure]);

    return (
        <div className="block-preview-surface" style={{ height: `${Math.round(height * previewScale)}px` }}>
            <iframe
                ref={frameRef}
                className="block-preview-frame"
                title="Block preview"
                srcDoc={markup}
                style={{
                    height: `${height}px`,
                    width: `${100 / previewScale}%`,
                    transform: `scale(${previewScale})`,
                }}
                onLoad={measure}
            />
        </div>
    );
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

            {state.status === 'ready' ? <PreviewFrame markup={state.markup} /> : null}

            <style>{styles}</style>
        </>
    );
}
