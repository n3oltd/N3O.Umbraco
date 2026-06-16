import { useEffect, useState } from 'react';
import type { AuthFetch } from '@n3o/backoffice-core';
import styles from './serp-editor-app.css?inline';

export interface SerpValue {
    title: string;
    description: string;
}

interface TemplateOptionsResponse {
    titleSuffix?: string;
}

interface SerpEditorAppProps {
    value: SerpValue;
    maxCharsTitle: number;
    maxCharsDescription: number;
    authFetch: AuthFetch | null;
    onChange: (value: SerpValue) => void;
}

// Module-level cache: the templateOptions response is site-wide and never changes within a session.
// We cache the resolved suffix so repeated property-editor mounts (e.g. multiple SERP fields on a
// page) only ever issue a single network request.
let cachedTitleSuffix: string | undefined;

// React UI for the Google SERP preview property editor. Controlled by the host web component:
// `value` comes in as a prop, edits are pushed back out via `onChange` (the host then raises
// UmbPropertyValueChangeEvent). Hybrid UI: uui-box for backoffice-standard chrome + a custom
// styled SERP preview (the bespoke surface).
export function SerpEditorApp({ value, maxCharsTitle, maxCharsDescription, authFetch, onChange }: SerpEditorAppProps) {
    const [titleSuffix, setTitleSuffix] = useState(cachedTitleSuffix ?? '');

    useEffect(() => {
        if (!authFetch) {
            return;
        }

        // Return early if we already have the suffix (from cache or a prior mount in this session).
        if (cachedTitleSuffix !== undefined) {
            return;
        }

        let active = true;

        authFetch('/umbraco/backoffice/api/serpEditor/templateOptions')
            .then((response) => {
                if (!response.ok) {
                    throw new Error(`templateOptions fetch failed: ${response.status}`);
                }
                return response.json() as Promise<TemplateOptionsResponse>;
            })
            .then((data) => {
                cachedTitleSuffix = data.titleSuffix ?? '';
                if (active) {
                    setTitleSuffix(cachedTitleSuffix);
                }
            })
            .catch((err: unknown) => {
                console.error('[SerpEditor] Failed to load templateOptions:', err);
            });

        return () => {
            active = false;
        };
    }, [authFetch]);

    const title = value.title ?? '';
    const description = value.description ?? '';
    const host = `${location.protocol}//${window.location.hostname}`;

    return (
        <uui-box headline="SEO preview">
            <div className="sv">
                <div className="sv-form">
                    <input
                        type="text"
                        value={title}
                        placeholder="Enter a short but descriptive title"
                        onChange={(e) => onChange({ title: e.target.value, description })}
                    />
                    {title.length > maxCharsTitle ? (
                        <p className="sv-error">A title should not be more than {maxCharsTitle} characters.</p>
                    ) : null}

                    <textarea
                        value={description}
                        placeholder="Enter a meta description"
                        onChange={(e) => onChange({ title, description: e.target.value })}
                    />
                    {description.length > maxCharsDescription ? (
                        <p className="sv-error">
                            A meta description should not be more than {maxCharsDescription} characters.
                        </p>
                    ) : null}
                </div>

                <div className="sv-demo">
                    {title.length > 0 ? (
                        <h6>
                            {title} {titleSuffix}
                        </h6>
                    ) : null}
                    {title.length > 0 || description.length > 0 ? <p className="sv-url">{host}</p> : null}
                    <p>{description}</p>
                </div>
            </div>

            <style>{styles}</style>
        </uui-box>
    );
}
