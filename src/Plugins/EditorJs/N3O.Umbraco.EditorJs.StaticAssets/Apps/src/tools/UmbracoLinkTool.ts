// Inline tool that replaces EditorJS's built-in link tool with an Umbraco link picker.
// EditorJS instantiates tools via `new ToolClass({ api, ... })`, so dependencies that were
// previously captured by a closure (openLinkPicker) are injected through a factory that returns
// the concrete class. The caller registers the returned class directly with EditorJS.

// ---- minimal shims for the EditorJS inline tool API ----

export interface EditorJsApi {
    styles: { inlineToolButton: string; inlineToolButtonActive: string };
    selection: {
        findParentTag(tagName: string, className?: string): HTMLElement | null;
        expandToTag(element: HTMLElement): void;
    };
}

export interface InlineToolConstructorArg {
    api: EditorJsApi;
}

export type OpenLinkPicker = (
    tool: { wrap(range: Range, url: string): void },
    range: Range
) => Promise<void>;

export function makeUmbracoLinkTool(openLinkPicker: OpenLinkPicker) {
    return class UmbracoLinkTool {
        static get isInline(): boolean {
            return true;
        }

        get state(): boolean {
            return this._state;
        }

        set state(state: boolean) {
            this._state = state;
            this.button?.classList.toggle(this.api.styles.inlineToolButtonActive, state);
        }

        static get sanitize(): object {
            return {
                a: {
                    href: true,
                },
            };
        }

        api: EditorJsApi;
        button: HTMLButtonElement | null = null;
        _state = false;
        element: HTMLElement | null = null;
        tag = 'A';
        class = 'cdx-link';

        constructor({ api }: InlineToolConstructorArg) {
            this.api = api;
        }

        render(): HTMLButtonElement {
            this.button = document.createElement('button');
            this.button.type = 'button';
            this.button.innerHTML =
                '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7.69998 12.6L7.67896 12.62C6.53993 13.7048 6.52012 15.5155 7.63516 16.625V16.625C8.72293 17.7073 10.4799 17.7102 11.5712 16.6314L13.0263 15.193C14.0703 14.1609 14.2141 12.525 13.3662 11.3266L13.22 11.12"></path><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16.22 11.12L16.3564 10.9805C17.2895 10.0265 17.3478 8.5207 16.4914 7.49733V7.49733C15.5691 6.39509 13.9269 6.25143 12.8271 7.17675L11.3901 8.38588C10.0935 9.47674 9.95706 11.4241 11.0888 12.6852L11.12 12.72"></path></svg>';
            this.button.classList.add(this.api.styles.inlineToolButton);
            return this.button;
        }

        surround(range: Range): void {
            if (this.state) {
                this.unwrap(range);
                return;
            }

            void openLinkPicker(this, range);
        }

        wrap(range: Range, url: string): void {
            const selectedText = range.extractContents();
            const link = document.createElement(this.tag);

            link.classList.add(this.class);
            link.setAttribute('href', url);
            link.appendChild(selectedText);
            range.insertNode(link);

            this.api.selection.expandToTag(link);
            this.element = link;
        }

        unwrap(range: Range): void {
            const link = this.api.selection.findParentTag(this.tag, this.class);
            const text = range.extractContents();

            link?.remove();

            range.insertNode(text);
        }

        checkState(): void {
            const link = this.api.selection.findParentTag(this.tag);
            this.state = !!link;
        }
    };
}
