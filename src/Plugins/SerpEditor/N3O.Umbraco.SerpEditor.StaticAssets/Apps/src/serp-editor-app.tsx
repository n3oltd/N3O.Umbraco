import { useEffect, useState } from 'react';
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
    onChange: (value: SerpValue) => void;
}

// React UI for the Google SERP preview property editor. Controlled by the host web component:
// `value` comes in as a prop, edits are pushed back out via `onChange` (the host then raises
// UmbPropertyValueChangeEvent). Hybrid UI: uui-box for backoffice-standard chrome + a custom
// styled SERP preview (the bespoke surface).
export function SerpEditorApp({ value, maxCharsTitle, maxCharsDescription, onChange }: SerpEditorAppProps) {
    const [titleSuffix, setTitleSuffix] = useState('');

    useEffect(() => {
        let active = true;

        fetch('/umbraco/backoffice/api/serpEditor/templateOptions')
            .then((response) => (response.ok ? response.json() : null))
            .then((data: TemplateOptionsResponse | null) => {
                if (active && data) {
                    setTitleSuffix(data.titleSuffix ?? '');
                }
            })
            .catch(() => {
                // preview suffix is non-essential
            });

        return () => {
            active = false;
        };
    }, []);

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
