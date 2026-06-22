import { useEffect, useRef, useState } from 'react';
import type { AuthFetch } from '@n3oltd/backoffice-core';
import type { ContentType, ContentMetadata, ExportProgressResponse, CreateExportResponse } from './types';

export interface ExportServerData {
    contentTypes: ContentType[];
    metadatas: ContentMetadata[];
}

export function useExportServerData(
    contentKey: string | null,
    authFetch: AuthFetch | null,
): ExportServerData {
    const [contentTypes, setContentTypes] = useState<ContentType[]>([]);
    const [metadatas, setMetadatas] = useState<ContentMetadata[]>([]);

    useEffect(() => {
        if (!contentKey || !authFetch) {
            return;
        }

        let active = true;

        const load = async (): Promise<void> => {
            const [typesRes, metaRes] = await Promise.all([
                authFetch(`/umbraco/backoffice/api/ContentTypes/${contentKey}/relations?type=descendant`, {
                    headers: { Accept: 'application/json' },
                }),
                authFetch('/umbraco/backoffice/api/Exports/lookups/contentMetadata', {
                    headers: { Accept: 'application/json' },
                }),
            ]);

            const types = (await typesRes.json()) as ContentType[];
            const metadata = (await metaRes.json()) as ContentMetadata[];

            for (const m of metadata) {
                m.selected = m.autoSelected;
            }

            metadata.sort((a, b) => a.displayOrder - b.displayOrder);

            if (active) {
                setContentTypes(types);
                setMetadatas(metadata);
            }
        };

        void load();

        return () => {
            active = false;
        };
    }, [contentKey, authFetch]);

    return { contentTypes, metadatas };
}

export interface ExportRun {
    processing: boolean;
    progress: string;
    errorMessage: string | null;
    doExport: (
        contentKey: string | null,
        contentTypeAlias: string,
        format: string,
        includeUnpublished: boolean,
        selectedMetadataIds: string[],
        selectedPropertyAliases: string[],
    ) => Promise<void>;
}

export function useExportRun(authFetch: AuthFetch | null): ExportRun {
    const [processing, setProcessing] = useState<boolean>(false);
    const [progress, setProgress] = useState<string>('');
    const [errorMessage, setErrorMessage] = useState<string | null>(null);

    // Task 1: generation counter replaces the shared/reset cancelledRef pattern.
    // Each new doExport() increments the counter; the poll closure captures the value at call-time
    // and bails out if a newer generation has started. The setTimeout id is captured so the
    // pending tick is cancelled immediately when a new export starts or the component unmounts.
    const generationRef = useRef<number>(0);
    const pollTimerRef = useRef<ReturnType<typeof setTimeout> | undefined>(undefined);

    // Cancel any pending poll tick when the component unmounts.
    useEffect(() => {
        return () => {
            clearTimeout(pollTimerRef.current);
            generationRef.current += 1;
        };
    }, []);

    const processingError = (message: string): void => {
        setProcessing(false);
        setProgress('');
        setErrorMessage(message);
    };

    // Task 1: poll() captures the current generation at call time (incremented by doExport before calling
    // poll). Stale ticks from a previous export are discarded when generationRef.current !== gen.
    // The setTimeout id is captured in pollTimerRef so doExport can clearTimeout on the next invocation.
    const poll = (exportId: string): Promise<ExportProgressResponse> => {
        const gen = generationRef.current;

        const executePoll = async (
            resolve: (value: ExportProgressResponse) => void,
            reject: (reason?: unknown) => void
        ): Promise<void> => {
            if (generationRef.current !== gen) {
                reject(new Error('poll cancelled'));
                return;
            }

            // Task 2: authFetch is guarded by canExport before doExport() is called; safe to use directly.
            const getProgress = await authFetch!(`/umbraco/backoffice/api/Exports/export/${exportId}/progress`, {
                headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
                method: 'GET',
            });

            // Task 4: use response.ok instead of comparing === 200.
            if (!getProgress.ok) {
                const progressRes = (await getProgress.json()) as ExportProgressResponse;
                processingError(String(progressRes));
                reject(progressRes);
                return;
            }

            const progressRes = (await getProgress.json()) as ExportProgressResponse;

            if (progressRes.isComplete === true) {
                resolve(progressRes);
            } else {
                setProgress(progressRes.text);
                // Task 1: capture the timer id so it can be cleared on cancel.
                pollTimerRef.current = setTimeout(() => void executePoll(resolve, reject), 2500);
            }
        };

        return new Promise(executePoll);
    };

    const doExport = async (
        contentKey: string | null,
        contentTypeAlias: string,
        format: string,
        includeUnpublished: boolean,
        selectedMetadataIds: string[],
        selectedPropertyAliases: string[],
    ): Promise<void> => {
        // Task 1: cancel any in-flight poll timer before starting a new export.
        clearTimeout(pollTimerRef.current);
        generationRef.current += 1;

        setProcessing(true);
        setProgress('');
        setErrorMessage(null);

        if (!selectedPropertyAliases.length && !selectedMetadataIds.length) {
            processingError('At least one property or metadata field must be selected');
            return;
        }

        const req = {
            format,
            includeUnpublished,
            metadata: selectedMetadataIds,
            properties: selectedPropertyAliases,
        };

        // Task 2: authFetch is guarded by canExport (!! authFetch check); non-null assertion is safe here.
        const createExport = await authFetch!(
            `/umbraco/backoffice/api/Exports/export/${contentKey}/${contentTypeAlias}`,
            {
                headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
                method: 'POST',
                body: JSON.stringify(req),
            }
        );

        // Task 4: use response.ok instead of comparing === 200.
        if (!createExport.ok) {
            const createRes = (await createExport.json()) as CreateExportResponse;
            processingError(String(createRes));
            return;
        }

        const createRes = (await createExport.json()) as CreateExportResponse;

        poll(createRes.id)
            .then(async (res) => {
                const exportFile = await authFetch!(`/umbraco/backoffice/api/Exports/export/${res.id}/file`, {
                    headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
                    method: 'GET',
                });

                // Task 4: use response.ok instead of comparing === 200.
                if (!exportFile.ok) {
                    processingError(String(await exportFile.json()));
                    return;
                }

                const blob = await exportFile.blob();
                const header = exportFile.headers.get('Content-Disposition') ?? '';
                const parts = header.split(';');
                const filename = (parts[1] ?? '').split('=')[1]?.replaceAll('"', '') ?? 'export';
                const newBlob = new Blob([blob]);
                const blobUrl = window.URL.createObjectURL(newBlob);
                const link = document.createElement('a');
                link.href = blobUrl;
                link.setAttribute('download', filename);
                document.body.appendChild(link);
                link.click();
                link.parentNode?.removeChild(link);
                window.URL.revokeObjectURL(blobUrl);

                setProcessing(false);
                setProgress('');
            })
            .catch(() => {
                // error already handled in poll
            });
    };

    return { processing, progress, errorMessage, doExport };
}