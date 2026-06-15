import { useEffect, useRef } from 'react';
// v17 verified: UmbPickerModalValue = { selection: Array<string | null> } (plain GUID strings).
// URL + name are resolved after pick via UmbMediaUrlRepository / UmbMediaItemRepository.
import { UMB_MEDIA_PICKER_MODAL, UmbMediaUrlRepository, UmbMediaItemRepository } from '@umbraco-cms/backoffice/media';
// FLAG: UMB_LINK_PICKER_MODAL result shape (result.link.url/unique) is not verified against the
// Umbraco 17 type definitions — result is cast via `as` below.
import { UMB_LINK_PICKER_MODAL } from '@umbraco-cms/backoffice/multi-url-picker';
import styles from './editor-js-app.css?inline';

// All editorjs tools are imported directly and bundled by Vite (they are NOT React).
// These replace the old editorjs-bundle.js / editorjs-all.js side-effect script.
// Packages without shipping .d.ts are declared in vendor.d.ts (adjacent to this file).
import EditorJS from '@editorjs/editorjs';
import Header from '@editorjs/header';
import RawTool from '@editorjs/raw';
import Checklist from '@editorjs/checklist';
import List from '@editorjs/list';
import Embed from '@editorjs/embed';
import Quote from '@editorjs/quote';
import CodeTool from '@editorjs/code';
import Paragraph from '@editorjs/paragraph';
import DragDrop from 'editorjs-drag-drop';
import AlignmentTuneCtor from 'editor-js-alignment-tune';

// ---- minimal shims for the untyped EditorJS tool APIs used in inner classes ----

interface EditorJsApi {
    styles: { inlineToolButton: string; inlineToolButtonActive: string };
    selection: {
        findParentTag(tagName: string, className?: string): HTMLElement | null;
        expandToTag(element: HTMLElement): void;
    };
}

interface InlineToolConstructorArg {
    api: EditorJsApi;
}

interface BlockToolConstructorArg {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    data: any;
    api: EditorJsApi;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    config: any;
}

// ---- media/link picker result shapes ----

// v17 verified: UmbPickerModalValue = { selection: Array<string | null> } (plain GUID strings).
interface MediaPickerResult {
    selection: Array<string | null>;
}

// Internal shape after resolving the GUID to media data.
interface MediaPickerResultItem {
    url?: string;
    name?: string;
    unique?: string;
    udi?: string;
    width?: string | number;
    height?: string | number;
}

interface LinkPickerResult {
    link?: { url?: string; unique?: string };
}

// ---- small helpers (ported verbatim) ----

function randomUUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        const r = (Math.random() * 16) | 0;
        const v = c === 'x' ? r : (r & 0x3) | 0x8;
        return v.toString(16);
    });
}

class RenderHelper {
    static randomUUID(): string {
        return randomUUID();
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

// The host bridge gives the bundled EditorJS tools access to the Umbraco modal manager (consumed by
// the web-component shell). `host` is the shell element (the modal manager's `open(host, ...)` arg).
export interface EditorJsHostBridge {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    host: any;
    // FLAG: UMB_MODAL_MANAGER_CONTEXT consumer value — using `any` then casting at call site.
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    modalManager: any;
}

export interface EditorJsAppProps {
    value: string | undefined;
    bridge: EditorJsHostBridge;
    onChange: (value: string) => void;
}

// React UI for the EditorJS property editor. Controlled by the host web component: `value` comes in
// as a prop, and edits are pushed back out via `onChange` (the host then raises
// UmbPropertyValueChangeEvent). EditorJS is a third-party imperative editor, so it is wrapped
// imperatively: instantiated once in a useEffect against a ref container, hydrated from `value`,
// and serialized back through onChange on every change. Hybrid UI: the EditorJS canvas is the
// bespoke surface; the fullscreen button is bespoke chrome.
export function EditorJsApp({ value, bridge, onChange }: EditorJsAppProps) {
    const containerRef = useRef<HTMLDivElement>(null);
    // EditorJS ships its own .d.ts; the field type is inferred from the constructor.
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const editorRef = useRef<any>(null);
    // Keep the latest value/onChange/bridge in refs so the EditorJS instance (created once) always
    // reads current values without re-initialising per render.
    const valueRef = useRef(value);
    const onChangeRef = useRef(onChange);
    const bridgeRef = useRef(bridge);
    valueRef.current = value;
    onChangeRef.current = onChange;
    bridgeRef.current = bridge;

    const editorId = useRef('skrivlet-editor-' + randomUUID());

    useEffect(() => {
        const holder = containerRef.current?.querySelector<HTMLElement>('#' + editorId.current);
        if (!holder) {
            return;
        }

        const getInitialData = (): object => {
            const current = valueRef.current;
            // Sometimes Umbraco returns a string (e.g. block grid), sometimes an object
            if (!current) {
                return {};
            } else if (typeof current === 'string') {
                try {
                    return JSON.parse(current) as object;
                } catch (e) {
                    console.error('Error parsing SkrivLet initial data JSON:', e);
                    return {};
                }
            } else {
                return (current as unknown as object) ?? {};
            }
        };

        // See: https://github.com/umbraco/Umbraco-CMS/pull/7186/files
        const stopUmbracosInterferingHotKeys = (): void => {
            const root = containerRef.current;
            if (!root) {
                return;
            }
            const editableElements = root.querySelectorAll(
                '.cdx-block:not([disable-hotkeys="true"]),.ce-header:not([disable-hotkeys="true"]),.cdx-input:not([disable-hotkeys="true"]),.cdx-checklist__item-text:not([disable-hotkeys="true"])'
            );
            for (let index = 0; index < editableElements.length; index++) {
                const element = editableElements[index];
                element.setAttribute('disable-hotkeys', 'true');
            }
        };

        const openMediaPicker = async (tool: {
            applyMediaSelection(item: MediaPickerResultItem): void;
        }): Promise<void> => {
            const { host, modalManager } = bridgeRef.current;
            if (!modalManager) {
                return;
            }

            // FLAG: UMB_MEDIA_PICKER_MODAL open() + onSubmit() result shape is cast — not verified
            // against Umbraco 17 types. (Preserved as-is from the Lit version.)
            const modal = modalManager.open(host, UMB_MEDIA_PICKER_MODAL, {
                data: {
                    multiple: false,
                },
            });

            try {
                // v17: result.selection is Array<string | null> — plain GUID strings.
                const result = (await modal.onSubmit()) as MediaPickerResult;
                const unique = result?.selection?.find((s) => s != null) ?? null;

                if (!unique) {
                    return;
                }

                // Resolve the GUID to a URL (UmbMediaUrlRepository) and name (UmbMediaItemRepository).
                // The repos attach controllers to the host, so destroy them after the pick to avoid
                // accumulating registrations across repeated image selections.
                const urlRepo = new UmbMediaUrlRepository(host);
                const itemRepo = new UmbMediaItemRepository(host);

                try {
                    const [urlResult, itemResult] = await Promise.all([
                        urlRepo.requestItems([unique]),
                        itemRepo.requestItems([unique]),
                    ]);

                    const urlModel = urlResult.data?.[0];
                    const itemModel = itemResult.data?.[0];

                    tool.applyMediaSelection({
                        url: urlModel?.url ?? '',
                        name: itemModel?.name ?? '',
                        unique,
                        udi: unique,
                    });
                } finally {
                    urlRepo.destroy();
                    itemRepo.destroy();
                }
            } catch {
                // Picker was cancelled or resolution failed
            }
        };

        const openLinkPicker = async (
            tool: { wrap(range: Range, url: string): void },
            range: Range
        ): Promise<void> => {
            const { host, modalManager } = bridgeRef.current;
            if (!modalManager) {
                return;
            }

            // FLAG: UMB_LINK_PICKER_MODAL open() + onSubmit() result shape is cast — not verified
            // against Umbraco 17 types. (Preserved as-is from the Lit version.)
            const modal = modalManager.open(host, UMB_LINK_PICKER_MODAL, {
                data: {
                    config: {},
                },
            });

            try {
                const result = (await modal.onSubmit()) as LinkPickerResult;
                const link = result?.link;

                if (link) {
                    tool.wrap(range, link.url ?? link.unique ?? '');
                }
            } catch {
                // Picker was cancelled
            }
        };

        const buildTools = (): object => {
            class UmbracoLinkTool {
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
            }

            interface ImageData {
                url: string;
                alt: string;
                udi: string;
                width?: number;
                height?: number;
            }

            class UmbracoImageTool {
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
            }

            // Embed is `any` (declared in vendor.d.ts) so extending it is safe at runtime.
            // The base type is widened to `any` so that super.render() and super.* are accessible.
            // eslint-disable-next-line @typescript-eslint/no-unsafe-call, @typescript-eslint/no-explicit-any
            class EmbedWithUI extends (Embed as new (...args: any[]) => any) {
                static get toolbox(): object {
                    return {
                        title: 'Video',
                        icon: '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-youtube w-6 h-6 mx-1"><path d="M2.5 17a24.12 24.12 0 0 1 0-10 2 2 0 0 1 1.4-1.4 49.56 49.56 0 0 1 16.2 0A2 2 0 0 1 21.5 7a24.12 24.12 0 0 1 0 10 2 2 0 0 1-1.4 1.4 49.55 49.55 0 0 1-16.2 0A2 2 0 0 1 2.5 17"></path><path d="m10 15 5-3-5-3z"></path></svg>',
                    };
                }

                // eslint-disable-next-line @typescript-eslint/no-explicit-any
                render(): HTMLElement {
                    // eslint-disable-next-line @typescript-eslint/no-explicit-any
                    if (!(this as any).data?.service) {
                        const container = document.createElement('div');
                        // eslint-disable-next-line @typescript-eslint/no-explicit-any
                        (this as any).element = container;

                        const label = RenderHelper.createLabel(
                            'embed-input',
                            'cdx-label',
                            'Enter a URL to embed a video from YouTube or Vimeo'
                        );
                        container.appendChild(label);

                        const input = RenderHelper.createInput('embed-input', '', '', 'url');
                        input.addEventListener('paste', (event: ClipboardEvent) => {
                            const url = event.clipboardData?.getData('text') ?? '';
                            // eslint-disable-next-line @typescript-eslint/no-explicit-any
                            const EmbedClass = Embed as any;
                            const service = Object.keys(EmbedClass.services as Record<string, { regex: RegExp }>).find(
                                (key) => (EmbedClass.services[key] as { regex: RegExp }).regex.test(url)
                            );
                            if (service) {
                                // eslint-disable-next-line @typescript-eslint/no-explicit-any
                                (this as any).onPaste({ detail: { key: service, data: url } });
                            }
                        });
                        container.appendChild(input);

                        return container;
                    }
                    // eslint-disable-next-line @typescript-eslint/no-explicit-any
                    return (super.render as () => HTMLElement).call(this);
                }

                // eslint-disable-next-line @typescript-eslint/no-explicit-any
                validate(savedData: any): boolean {
                    return savedData.service && savedData.source ? true : false;
                }
            }

            return {
                paragraph: {
                    class: Paragraph,
                    tunes: ['alignmentTune'],
                },
                header: {
                    class: Header,
                    tunes: ['alignmentTune'],
                },
                image: UmbracoImageTool,
                quote: Quote,
                embed: {
                    class: EmbedWithUI,
                    config: {
                        services: {
                            youtube: true,
                            vimeo: true,
                        },
                    },
                },
                code: CodeTool,
                raw: RawTool,
                list: {
                    class: List,
                    inlineToolbar: true,
                },
                checklist: Checklist,
                link: UmbracoLinkTool, // override link with Umbraco link picker
                alignmentTune: {
                    class: AlignmentTuneCtor,
                    config: {
                        default: 'left',
                    },
                },
            };
        };

        // EditorJS ships a .d.ts but its `holder` type accepts string|HTMLElement;
        // passing HTMLElement directly is fine at runtime but tsc may complain — cast to any for the
        // whole config object to avoid fighting mismatched generic overloads in the EditorJS typings.
        // eslint-disable-next-line @typescript-eslint/no-explicit-any, @typescript-eslint/no-unsafe-call
        const editor = new (EditorJS as any)({
            holder: holder,

            placeholder: "Type '/' to insert a block or just start typing something super...",

            data: getInitialData(),

            inlineToolbar: true,

            // TODO: Not working
            sanitizer: {
                a: {},
            },

            tools: buildTools(),

            onChange: () => {
                stopUmbracosInterferingHotKeys();
                // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-call
                editorRef.current
                    ?.save()
                    .then((outputData: unknown) => {
                        onChangeRef.current(JSON.stringify(outputData));
                    })
                    .catch((error: unknown) => {
                        console.log('Saving failed: ', error);
                    });
            },

            onReady: () => {
                // eslint-disable-next-line @typescript-eslint/no-unsafe-call
                new DragDrop(editorRef.current);
                stopUmbracosInterferingHotKeys();
            },
        });

        editorRef.current = editor;

        return () => {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
            if (editorRef.current && typeof editorRef.current.destroy === 'function') {
                // eslint-disable-next-line @typescript-eslint/no-unsafe-call, @typescript-eslint/no-unsafe-member-access
                editorRef.current.destroy();
                editorRef.current = null;
            }
        };
        // Init once: the editor reads value/onChange/bridge from refs, so it must not re-init per render.
    }, []);

    const openFullscreen = (): void => {
        const container = containerRef.current?.querySelector<HTMLElement>('#' + editorId.current);

        if (!document.fullscreenElement && container) {
            void container.requestFullscreen();
        } else if (document.exitFullscreen) {
            void document.exitFullscreen();
        }
    };

    return (
        <div className="skriv-let" ref={containerRef}>
            <div id={editorId.current} className="skriv-let__container"></div>
            <button className="skriv-let__fullscreen-button" onClick={openFullscreen} type="button">
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    className="skriv-let__fullscreen-button-icon"
                    fill="currentColor"
                    width="22"
                    height="22"
                    viewBox="0 0 512 512">
                    <path d="M368.432 110.765l32.367 32.362 34.896-34.898 36.539 36.539V38.783H366.256l37.075 37.077-34.899 34.905zm66.725 293.309l-34.901-34.899-32.37 32.368 34.9 34.899-36.534 36.536h105.986V366.996l-37.081 37.078zm-294.656-3.081l-32.37-32.365-34.898 34.902L36.7 366.993v105.985h105.979l-37.079-37.08 34.901-34.905zm-31.828-258.41l32.373-32.365-34.903-34.899 36.538-36.536H36.698v105.978l37.08-37.075 34.895 34.897zm278.314 157.969v-86.92c0-35.993-29.179-65.169-65.17-65.169H186.109c-35.991 0-65.171 29.177-65.171 65.169v86.92c0 35.993 29.18 65.168 65.171 65.168h135.708c35.992 0 65.17-29.175 65.17-65.168z"></path>
                </svg>
                <span className="sr-only">Open editor in fullscreen</span>
            </button>
            <style>{styles}</style>
        </div>
    );
}
