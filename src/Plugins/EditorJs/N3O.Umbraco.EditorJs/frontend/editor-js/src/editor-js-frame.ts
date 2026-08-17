import EditorJS from '@editorjs/editorjs';
import Header from '@editorjs/header';
import RawTool from '@editorjs/raw';
import Checklist from '@editorjs/checklist';
import List from '@editorjs/list';
import Quote from '@editorjs/quote';
import CodeTool from '@editorjs/code';
import Paragraph from '@editorjs/paragraph';
import DragDrop from 'editorjs-drag-drop';
import AlignmentTuneCtor from 'editor-js-alignment-tune';

import { makeUmbracoLinkTool } from './tools/UmbracoLinkTool';
import { makeUmbracoImageTool, type MediaPickerResultItem } from './tools/UmbracoImageTool';
import { EmbedWithUI } from './tools/EmbedWithUI';
import styles from './editor-js-app.css?inline';

export interface EditorJsFrameConfig {
    value: string | undefined;
    onChange: (value: string) => void;
    pickMedia: () => Promise<MediaPickerResultItem | null>;
    pickLink: () => Promise<string | null>;
    requestSave: (key: string) => void;
}

const FULLSCREEN_ICON =
    '<svg xmlns="http://www.w3.org/2000/svg" class="skriv-let__fullscreen-button-icon" fill="currentColor" width="22" height="22" viewBox="0 0 512 512"><path d="M368.432 110.765l32.367 32.362 34.896-34.898 36.539 36.539V38.783H366.256l37.075 37.077-34.899 34.905zm66.725 293.309l-34.901-34.899-32.37 32.368 34.9 34.899-36.534 36.536h105.986V366.996l-37.081 37.078zm-294.656-3.081l-32.37-32.365-34.898 34.902L36.7 366.993v105.985h105.979l-37.079-37.08 34.901-34.905zm-31.828-258.41l32.373-32.365-34.903-34.899 36.538-36.536H36.698v105.978l37.08-37.075 34.895 34.897zm278.314 157.969v-86.92c0-35.993-29.179-65.169-65.17-65.169H186.109c-35.991 0-65.171 29.177-65.171 65.169v86.92c0 35.993 29.18 65.168 65.171 65.168h135.708c35.992 0 65.17-29.175 65.17-65.168z"></path></svg>';

// Umbraco supplies this value as either a string or an object. Null signals a value that could not be
// read, which is not the same as an empty document.
function getInitialData(value: string | undefined): object | null {
    if (!value) {
        return {};
    }
    if (typeof value === 'string') {
        try {
            return JSON.parse(value) as object;
        } catch (e) {
            console.error('[EditorJs] Could not parse the stored value:', e);
            return null;
        }
    }
    return (value as unknown as object) ?? {};
}

let setValue: (value: string | undefined) => void = () => {};

function init(config: EditorJsFrameConfig): void {
    let flush: () => Promise<void> = () => Promise.resolve();
    let applyingExternalValue = false;

    // save() stamps a fresh time on every call, so only the blocks decide whether anything changed.
    const comparable = (outputData: unknown): string =>
        JSON.stringify((outputData as { blocks?: unknown } | undefined)?.blocks ?? null);

    let lastEmitted = comparable(getInitialData(config.value));

    const emit = (outputData: unknown): void => {
        if (applyingExternalValue) {
            return;
        }

        const blocks = comparable(outputData);

        if (blocks === lastEmitted) {
            return;
        }

        lastEmitted = blocks;

        config.onChange(JSON.stringify(outputData));
    };

    const styleEl = document.createElement('style');
    styleEl.textContent = styles;
    document.head.appendChild(styleEl);

    const container = document.createElement('div');
    container.className = 'skriv-let';

    const holder = document.createElement('div');
    holder.className = 'skriv-let__container';
    container.appendChild(holder);

    const fullscreenButton = document.createElement('button');
    fullscreenButton.type = 'button';
    fullscreenButton.className = 'skriv-let__fullscreen-button';
    fullscreenButton.innerHTML = `${FULLSCREEN_ICON}<span class="sr-only">Open editor in fullscreen</span>`;
    fullscreenButton.addEventListener('click', () => {
        if (!document.fullscreenElement) {
            void holder.requestFullscreen();
        } else {
            void document.exitFullscreen();
        }
    });
    container.appendChild(fullscreenButton);

    document.body.appendChild(container);

    const openMediaPicker = (tool: { applyMediaSelection(item: MediaPickerResultItem): void }): Promise<void> =>
        config.pickMedia().then((item) => {
            if (item) {
                tool.applyMediaSelection(item);
            }
        });

    const openLinkPicker = (tool: { wrap(range: Range, url: string): void }, range: Range): Promise<void> =>
        config.pickLink().then((url) => {
            if (url) {
                tool.wrap(range, url);
            }
        });

    // EditorJS's shipped .d.ts is narrower than the configuration it accepts at runtime.
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    let editor: any;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any, @typescript-eslint/no-unsafe-call
    editor = new (EditorJS as any)({
        holder,
        placeholder: "Type '/' to insert a block or just start typing something super...",
        data: getInitialData(config.value) ?? {},
        inlineToolbar: true,
        tools: {
            paragraph: { class: Paragraph, tunes: ['alignmentTune'] },
            header: { class: Header, tunes: ['alignmentTune'] },
            image: makeUmbracoImageTool(openMediaPicker),
            quote: Quote,
            embed: { class: EmbedWithUI, config: { services: { youtube: true, vimeo: true } } },
            code: CodeTool,
            raw: RawTool,
            list: { class: List, inlineToolbar: true },
            checklist: Checklist,
            link: makeUmbracoLinkTool(openLinkPicker),
            alignmentTune: { class: AlignmentTuneCtor, config: { default: 'left' } },
        },
        onChange: () => {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-call
            editor
                ?.save()
                .then(emit)
                .catch((error: unknown) => {
                    console.error('[EditorJs] Saving failed:', error);
                });
        },
        onReady: () => {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-call
            new DragDrop(editor);
        },
    });

    flush = async () => {
        try {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-call, @typescript-eslint/no-unsafe-assignment
            const outputData = await editor?.save();

            emit(outputData);
        } catch (error: unknown) {
            console.error('[EditorJs] Saving failed:', error);
        }
    };

    // EditorJS batches its change notification, so a recent edit is still only in the DOM.
    window.addEventListener('blur', () => {
        void flush();
    });

    // A key event raised in this document never reaches the backoffice's own window listener.
    document.addEventListener('keydown', (event: KeyboardEvent) => {
        const key = event.key.toLowerCase();

        if ((event.metaKey || event.ctrlKey) && (key === 's' || key === 'p')) {
            event.preventDefault();

            void flush().then(() => config.requestSave(key));
        }
    });

    setValue = (value: string | undefined) => {
        const data = getInitialData(value) as { blocks?: unknown[] } | null;

        if (data == null) {
            return;
        }

        applyingExternalValue = true;
        lastEmitted = comparable(data);

        // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-call, @typescript-eslint/no-unsafe-assignment
        const applied: unknown = data.blocks?.length ? editor?.render(data) : editor?.clear();

        void Promise.resolve(applied).finally(() => {
            applyingExternalValue = false;
        });
    };
}

type FrameHandshake = HTMLIFrameElement & {
    __n3oInit?: (config: EditorJsFrameConfig) => void;
    __n3oSetValue?: (value: string | undefined) => void;
    __n3oOnReady?: () => void;
};

const frameElement = window.frameElement as FrameHandshake | null;
if (frameElement) {
    frameElement.__n3oInit = init;
    frameElement.__n3oSetValue = (value) => setValue(value);
    frameElement.__n3oOnReady?.();
}
