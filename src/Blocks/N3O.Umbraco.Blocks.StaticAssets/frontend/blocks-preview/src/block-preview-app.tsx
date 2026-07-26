import { useCallback, useEffect, useRef, useState } from 'react';
import styles from './block-preview-app.css?inline';
import type { PreviewState } from './types';

interface BlockPreviewAppProps {
    state: PreviewState;
}

// transform does not affect layout, so the surface carries the scaled height and the frame is widened to
// compensate.
const previewScale = 0.9;

// The markup is a whole rendered page: in a frame its scripts run and the site stylesheet applies as it does
// live. srcDoc keeps the frame same-origin, so its content height is measurable.
function PreviewFrame({ markup }: { markup: string }) {
    const frameRef = useRef<HTMLIFrameElement>(null);
    const observerRef = useRef<ResizeObserver | null>(null);
    const [height, setHeight] = useState(0);

    // srcDoc navigates the frame and the navigation is queued, so during an effect contentDocument is still
    // the outgoing document, or initially about:blank. load is when it is the document being measured.
    const observeFrame = useCallback(() => {
        observerRef.current?.disconnect();

        const body = frameRef.current?.contentDocument?.body;

        if (!body) {
            return;
        }

        // Sizing the frame resizes the content viewport, so any vh or percentage height feeds back into
        // scrollHeight. The threshold settles that instead of letting it oscillate.
        const observer = new ResizeObserver(() => {
            setHeight((current) => (Math.abs(body.scrollHeight - current) > 1 ? body.scrollHeight : current));
        });

        observer.observe(body);
        observerRef.current = observer;

        setHeight(body.scrollHeight);
    }, []);

    useEffect(() => () => observerRef.current?.disconnect(), []);

    return (
        <div className="block-preview-surface" style={{ height: `${Math.ceil(height * previewScale)}px` }}>
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
                onLoad={observeFrame}
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
