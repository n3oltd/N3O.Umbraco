import { useEffect, useRef } from 'react';
import type {
    UmbDocumentDetailModel,
    UmbDocumentVariantModel,
    UmbDocumentValueModel,
} from '@umbraco-cms/backoffice/document';

// Response shape from /umbraco/backoffice/api/platformsBackOffice/previewHtml/...
export interface PreviewHtmlResponse {
    eTag: string;
    html: string;
}

interface PlatformsPreviewAppProps {
    // `unique` drives a re-run of the preview effect when the workspace document changes.
    unique: string | null | undefined;
    // Reads the current in-memory document on each poll (mirrors the original getData() call).
    getContent: () => UmbDocumentDetailModel | undefined;
}

function getApiReq(
    values: Array<UmbDocumentValueModel>,
    contentTypeAlias: string | undefined,
): Record<string, unknown> {
    const req: Record<string, unknown> = {};

    values.forEach((property) => {
        req[property.alias] = property.value;
    });

    req['contentTypeAlias'] = contentTypeAlias;

    return req;
}

// React UI for the Platforms preview workspace view. The preview is a bespoke surface: it builds a
// request from the current variant's property values, posts it to the platforms back office preview
// endpoint, and renders the returned HTML in an iframe loading the tenant's platforms.js. Refreshed
// every 10 seconds; unchanged responses (same eTag) are skipped. The iframe DOM is built imperatively
// inside the container ref, preserving the original Lit behaviour exactly.
export function PlatformsPreviewApp({ unique, getContent }: PlatformsPreviewAppProps) {
    const containerRef = useRef<HTMLDivElement | null>(null);
    const previousETagRef = useRef<string | null>(null);

    useEffect(() => {
        let active = true;

        const loadPreview = async (): Promise<void> => {
            const content = getContent();

            if (!content) {
                return;
            }

            // WORKSPACE-CONTEXT SHAPE FLAG:
            // The TS type UmbDocumentDetailModel has no `contentType`, `contentTypeAlias`, `parent`, or `parentId`
            // properties. The original JS accessed these as legacy/undocumented fields on the in-memory model.
            // `documentType.unique` is the GUID not an alias; there is no typed way to get the content-type alias
            // via getData(). These casts preserve the runtime behaviour without changing the API calls.
            // They should be verified once the content-type alias requirement is clarified upstream.
            const rawContent = content as UmbDocumentDetailModel & {
                contentType?: { alias?: string };
                contentTypeAlias?: string;
                parent?: { unique?: string };
                parentId?: string;
            };

            const contentTypeAlias: string | undefined = rawContent.contentType?.alias ?? rawContent.contentTypeAlias;

            const variants: Array<UmbDocumentVariantModel> = content.variants ?? [];
            const variant: UmbDocumentVariantModel | undefined =
                variants.find((v) => v.culture == null || v.segment == null) ?? variants[0];

            const values: Array<UmbDocumentValueModel> = content.values ?? [];
            const apiReq = getApiReq(values, contentTypeAlias);

            apiReq['name'] = variant?.name;
            apiReq['key'] = content.unique;
            apiReq['parentId'] = rawContent.parent?.unique ?? rawContent.parentId;

            const subscriptionCodeRes = await fetch('/umbraco/backoffice/api/cloudBackOffice/subscription/code');
            const subscriptionCode = (await subscriptionCodeRes.json()) as string;

            const apiRes = await fetch(`/umbraco/backoffice/api/platformsBackOffice/previewHtml/${contentTypeAlias}`, {
                method: 'POST',
                headers: {
                    accept: 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(apiReq),
            });

            const res = (await apiRes.json()) as PreviewHtmlResponse;

            if (res.eTag === previousETagRef.current) {
                return;
            }

            previousETagRef.current = res.eTag;

            const container = containerRef.current;

            if (!container || !active) {
                return;
            }

            container.innerHTML = '';

            const iframe = document.createElement('iframe');
            iframe.style.width = '100%';
            iframe.style.aspectRatio = '16 / 9';
            iframe.style.border = '0';
            iframe.style.transform = 'scale(0.9)';
            iframe.style.transformOrigin = '0 0';
            iframe.style.display = 'none';

            container.appendChild(iframe);

            // iframe.contentWindow is non-null immediately after appending a same-origin iframe.
            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
            const doc = iframe.contentWindow!.document;
            doc.open();
            doc.write(res.html);
            doc.close();

            const script = doc.createElement('script');
            script.src = `https://cdn.n3o.cloud/connect-${subscriptionCode}/platforms-js/platforms.js`;
            script.type = 'module';

            doc.body.appendChild(script);

            window.setInterval(() => {
                iframe.style.display = 'block';
                container.style.display = 'block';
            }, 2000);
        };

        void loadPreview();

        const intervalId = window.setInterval(() => {
            void loadPreview();
        }, 10000);

        return () => {
            active = false;
            window.clearInterval(intervalId);
        };
    }, [unique, getContent]);

    return <div ref={containerRef} id="platformsPreviewContainer" style={{ display: 'none' }} />;
}
