import { useEffect, useRef } from 'react';
import type {
    UmbDocumentDetailModel,
    UmbDocumentVariantModel,
    UmbDocumentValueModel,
} from '@umbraco-cms/backoffice/document';
import type { AuthFetch } from '@n3oltd/backoffice-core';

export interface PreviewHtmlResponse {
    eTag: string;
    html: string;
}

interface PlatformsPreviewAppProps {
    unique: string | null | undefined;
    getContent: () => UmbDocumentDetailModel | undefined;
    authFetch: AuthFetch | null;
}

function getApiReq(
    values: Array<UmbDocumentValueModel>,
    documentTypeUnique: string | undefined,
): Record<string, unknown> {
    const req: Record<string, unknown> = {};

    values.forEach((property) => {
        req[property.alias] = property.value;
    });

    req['contentTypeAlias'] = documentTypeUnique;

    return req;
}

export function PlatformsPreviewApp({ unique, getContent, authFetch }: PlatformsPreviewAppProps) {
    const containerRef = useRef<HTMLDivElement | null>(null);
    const previousETagRef = useRef<string | null>(null);

    useEffect(() => {
        let active = true;

        const loadPreview = async (): Promise<void> => {
            if (!authFetch) {
                return;
            }

            const content = getContent();

            if (!content) {
                return;
            }

            const documentTypeUnique: string | undefined = content.documentType?.unique;

            const rawContent = content as UmbDocumentDetailModel & {
                parent?: { unique?: string };
                parentId?: string;
            };

            const variants: Array<UmbDocumentVariantModel> = content.variants ?? [];
            const variant: UmbDocumentVariantModel | undefined =
                variants.find((v) => v.culture == null || v.segment == null) ?? variants[0];

            const values: Array<UmbDocumentValueModel> = content.values ?? [];
            const apiReq = getApiReq(values, documentTypeUnique);

            apiReq['name'] = variant?.name;
            apiReq['key'] = content.unique;
            apiReq['parentId'] = rawContent.parent?.unique ?? rawContent.parentId;

            const subscriptionCodeRes = await authFetch('/umbraco/backoffice/api/cloudBackOffice/subscription/code');
            const subscriptionCode = (await subscriptionCodeRes.json()) as string;

            const apiRes = await authFetch(`/umbraco/backoffice/api/platformsBackOffice/previewHtml/${content.unique}`, {
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

            const doc = iframe.contentWindow!.document;
            doc.open();
            doc.write(res.html);
            doc.close();

            const script = doc.createElement('script');
            script.src = `https://cdn.n3o.cloud/connect-${subscriptionCode}/platforms-js/platforms.js`;
            script.type = 'module';

            doc.body.appendChild(script);

            window.setTimeout(() => {
                if (!active) {
                    return;
                }

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
    }, [unique, getContent, authFetch]);

    return <div ref={containerRef} id="platformsPreviewContainer" style={{ display: 'none' }} />;
}
