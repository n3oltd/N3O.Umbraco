import { useEffect, useState } from 'react';
import type { AuthFetch } from '@n3o/backoffice-core';
import type { ContentType, DatePattern } from './types';

export interface ImportLookups {
    contentTypes: ContentType[];
    datePatterns: DatePattern[];
    datePattern: DatePattern | null;
    setDatePattern: (pattern: DatePattern | null) => void;
}

export function useImportLookups(contentKey: string | null, authFetch: AuthFetch | null): ImportLookups {
    const [contentTypes, setContentTypes] = useState<ContentType[]>([]);
    const [datePatterns, setDatePatterns] = useState<DatePattern[]>([]);
    const [datePattern, setDatePattern] = useState<DatePattern | null>(null);

    useEffect(() => {
        if (!contentKey || !authFetch) {
            return;
        }

        let active = true;

        const init = async (): Promise<void> => {
            const typesRes = await authFetch(`/umbraco/backoffice/api/ContentTypes/${contentKey}/relations?type=child`, {
                headers: { Accept: 'application/json' },
            });
            const types = (await typesRes.json()) as ContentType[];

            const patternsRes = await authFetch('/umbraco/backoffice/api/Imports/lookups/datePatterns', {
                headers: { Accept: 'application/json' },
            });
            const patterns = (await patternsRes.json()) as DatePattern[];

            if (active) {
                setContentTypes(types);
                setDatePatterns(patterns);
                setDatePattern(patterns[0] ?? null);
            }
        };

        void init();

        return () => {
            active = false;
        };
    }, [contentKey, authFetch]);

    return { contentTypes, datePatterns, datePattern, setDatePattern };
}
