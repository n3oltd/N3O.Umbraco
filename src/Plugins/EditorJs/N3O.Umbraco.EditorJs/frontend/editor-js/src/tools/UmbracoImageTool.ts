import { createInput, createLabel, randomUUID } from './renderHelpers';

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

            const altTextID = randomUUID();
            this.altTextLabel = createLabel(altTextID, 'sr-only', 'Alt text');
            this.altTextInput = createInput(altTextID, this.data.alt, 'Enter alt text', 'text');

            this.wrapper.classList.add('simple-image');

            this._createImage(this.data.url);

            this.button = document.createElement('button');
            this.button.type = 'button';
            this.button.classList.add('umb-group-builder__group-add-property');
            this.button.classList.add('skriv-let__add-image-button');
            this.button.textContent = this.data?.url ? 'Change image' : 'Select an image';
            // The picker's modal belongs to the parent document and renders behind a fullscreen frame.
            const pickUnlessFullscreen = () => {
                if (document.fullscreenElement) {
                    return;
                }

                void openMediaPicker(this);
            };

            this.button.addEventListener('click', pickUnlessFullscreen);
            this.image?.addEventListener('click', pickUnlessFullscreen);
            this.wrapper.appendChild(this.altTextLabel);
            this.wrapper.appendChild(this.altTextInput);
            this.wrapper.appendChild(this.button);
            this.wrapper.appendChild(this.input);

            return this.wrapper;
        }

        applyMediaSelection(item: MediaPickerResultItem): void {
            const imageUrl = item.url ?? '';
            const imageAlt = item.name ?? '';

            const width = Number.parseInt(String(item.width), 10);
            const height = Number.parseInt(String(item.height), 10);

            this.data.url = imageUrl;
            this.data.alt = imageAlt;
            this.data.udi = item.udi ?? '';
            this.data.width = Number.isNaN(width) ? undefined : width;
            this.data.height = Number.isNaN(height) ? undefined : height;

            if (this.input) {
                this.input.value = imageUrl;
            }
            if (this.image) {
                this.image.src = imageUrl;
                this.image.alt = imageAlt;
            }
            if (this.altTextInput) {
                this.altTextInput.value = imageAlt;
                this.altTextInput.setAttribute('value', imageAlt);
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
