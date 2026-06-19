// Block tool that lets editors pick images from the Umbraco media library.
// EditorJS instantiates tools via `new ToolClass({ data, api, config })`, so the openMediaPicker
// dependency is injected through a factory that returns the concrete class. The caller registers
// the returned class directly with EditorJS.

// ---- minimal shims for the EditorJS block tool API ----

export interface EditorJsApi {
    styles: { inlineToolButton: string; inlineToolButtonActive: string };
    selection: {
        findParentTag(tagName: string, className?: string): HTMLElement | null;
        expandToTag(element: HTMLElement): void;
    };
}

export interface BlockToolConstructorArg {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    data: any;
    api: EditorJsApi;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    config: any;
}

export interface ImageData {
    url: string;
    alt: string;
    udi: string;
    width?: number;
    height?: number;
}

export interface MediaPickerResultItem {
    url?: string;
    name?: string;
    unique?: string;
    udi?: string;
    width?: string | number;
    height?: string | number;
}

export type OpenMediaPicker = (tool: { applyMediaSelection(item: MediaPickerResultItem): void }) => Promise<void>;

// ---- small DOM helpers shared by image tool ----

class RenderHelper {
    static randomUUID(): string {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            const r = (Math.random() * 16) | 0;
            const v = c === 'x' ? r : (r & 0x3) | 0x8;
            return v.toString(16);
        });
    }

    static createLabel(id: string, cssClass: string, text: string): HTMLLabelElement {
        const label = document.createElement('label');
        label.innerHTML = text;
        label.classList.add(cssClass);
        label.setAttribute('for', id);
        return label;
    }

    static createInput(id: string, value: string, text: string, type: string): HTMLInputElement {
        const input = document.createElement('input');
        input.setAttribute('type', type);
        if (value) {
            input.setAttribute('value', value);
        }
        if (text) {
            input.setAttribute('placeholder', text);
        }
        input.setAttribute('id', id);
        input.classList.add('cdx-input');
        return input;
    }
}

export function makeUmbracoImageTool(openMediaPicker: OpenMediaPicker) {
    return class UmbracoImageTool {
        static get toolbox(): object {
            return {
                title: 'Image',
                icon: '<svg width="17" height="15" viewBox="0 0 336 276" xmlns="http://www.w3.org/2000/svg"><path d="M291 150V79c0-19-15-34-34-34H79c-19 0-34 15-34 34v42l67-44 81 72 56-29 42 30zm0 52l-43-30-56 30-81-67-66 39v23c0 19 15 34 34 34h178c17 0 31-13 34-29zM79 0h178c44 0 79 35 79 79v118c0 44-35 79-79 79H79c-44 0-79-35-79-79V79C0 35 35 0 79 0z"/></svg>',
            };
        }

        api: EditorJsApi;
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        config: any;
        data: ImageData;
        wrapper: HTMLDivElement | undefined;
        input: HTMLInputElement | undefined;
        altTextLabel: HTMLLabelElement | undefined;
        altTextInput: HTMLInputElement | undefined;
        image: HTMLImageElement | undefined;
        button: HTMLButtonElement | undefined;

        constructor({ data, api, config }: BlockToolConstructorArg) {
            this.api = api;
            this.config = config || {};
            this.data = {
                url: (data.url as string) || '',
                alt: (data.alt as string) || '',
                udi: (data.udi as string) || '',
            };
        }

        render(): HTMLDivElement {
            this.wrapper = document.createElement('div');
            this.input = document.createElement('input');
            this.input.setAttribute('type', 'hidden');

            const altTextID = RenderHelper.randomUUID();
            this.altTextLabel = RenderHelper.createLabel(altTextID, 'sr-only', 'Alt text');
            this.altTextInput = RenderHelper.createInput(altTextID, this.data.alt, 'Enter alt text', 'text');

            this.wrapper.classList.add('simple-image');

            this._createImage(this.data.url);

            this.button = document.createElement('button');
            this.button.type = 'button';
            this.button.classList.add('umb-group-builder__group-add-property');
            this.button.classList.add('skriv-let__add-image-button');
            this.button.textContent = this.data?.url ? 'Change image' : 'Select an image';
            this.button.addEventListener('click', () => {
                void openMediaPicker(this);
            });
            this.image?.addEventListener('click', () => {
                void openMediaPicker(this);
            });
            this.wrapper.appendChild(this.altTextLabel);
            this.wrapper.appendChild(this.altTextInput);
            this.wrapper.appendChild(this.button);
            this.wrapper.appendChild(this.input);

            return this.wrapper;
        }

        applyMediaSelection(item: MediaPickerResultItem): void {
            const imageUrl = item.url ?? '';
            const imageAlt = item.name ?? '';

            this.data.url = imageUrl;
            this.data.alt = imageAlt;
            this.data.udi = item.unique ?? item.udi ?? '';
            this.data.width = parseInt(String(item.width));
            this.data.height = parseInt(String(item.height));

            if (this.input) {
                this.input.value = imageUrl;
            }
            if (this.image) {
                this.image.src = imageUrl;
                this.image.alt = imageAlt;
            }
            if (this.altTextInput) {
                this.altTextInput.value = imageAlt;
            }
            if (this.button) {
                this.button.textContent = this.data?.url ? 'Change image' : 'Select an image';
            }
            this.save();
            setTimeout(() => {
                this.image?.scrollIntoView();
            }, 200);
        }

        _createImage(url: string): void {
            this.image = document.createElement('img');
            this.image.src = url;
            this.image.alt = this.data.alt;
            this.wrapper?.appendChild(this.image);
        }

        save(): ImageData {
            return {
                url: this.data.url,
                alt: this.altTextInput?.value ?? '',
                udi: this.data.udi,
                width: this.data.width,
                height: this.data.height,
            };
        }

        validate(savedData: ImageData): boolean {
            if (!savedData.url.trim() || !savedData.udi.trim()) {
                return false;
            }
            return true;
        }
    };
}
