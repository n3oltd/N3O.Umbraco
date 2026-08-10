// Frame entry — runs INSIDE the editor's iframe (a separate, same-origin document).
//
// EditorJS is built for a top-level document: it injects its stylesheet into `document.head` and its
// "close popovers on outside click" handler reads `event.target`. Inside Umbraco 17's all-shadow-DOM
// backoffice both break (head styles can't cross the shadow boundary; `event.target` is retargeted to
// `umb-app`). Hosting EditorJS in an iframe gives it its own real document, so BOTH work natively with
// no per-symptom shims: styles land in the iframe's head, and clicks report the real target.
//
// This module is loaded by the shell (editor-js.ts) into the iframe. It has NO @umbraco / React deps —
// everything it needs is bundled. Umbraco-specific work (media/link pickers) is delegated back to the
// shell in the parent via the injected bridge callbacks.

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

// Bridge callbacks are provided by the shell (parent). They run in the parent realm where the Umbraco
// modal manager + media repositories live, and return plain data the (Umbraco-free) tools apply.
export interface EditorJsFrameConfig {
    value: string | undefined;
    onChange: (value: string) => void;
    pickMedia: () => Promise<MediaPickerResultItem | null>;
    pickLink: () => Promise<string | null>;
}

const FULLSCREEN_ICON =
    '<svg xmlns="http://www.w3.org/2000/svg" class="skriv-let__fullscreen-button-icon" fill="currentColor" width="22" height="22" viewBox="0 0 512 512"><path d="M368.432 110.765l32.367 32.362 34.896-34.898 36.539 36.539V38.783H366.256l37.075 37.077-34.899 34.905zm66.725 293.309l-34.901-34.899-32.37 32.368 34.9 34.899-36.534 36.536h105.986V366.996l-37.081 37.078zm-294.656-3.081l-32.37-32.365-34.898 34.902L36.7 366.993v105.985h105.979l-37.079-37.08 34.901-34.905zm-31.828-258.41l32.373-32.365-34.903-34.899 36.538-36.536H36.698v105.978l37.08-37.075 34.895 34.897zm278.314 157.969v-86.92c0-35.993-29.179-65.169-65.17-65.169H186.109c-35.991 0-65.171 29.177-65.171 65.169v86.92c0 35.993 29.18 65.168 65.171 65.168h135.708c35.992 0 65.17-29.175 65.17-65.168z"></path></svg>';

function getInitialData(value: string | undefined): object {
    // Sometimes Umbraco returns a string (e.g. block grid), sometimes an object.
    if (!value) {
        return {};
    }
    if (typeof value === 'string') {
        try {
            return JSON.parse(value) as object;
        } catch (e) {
            console.error('Error parsing SkrivLet initial data JSON:', e);
            return {};
        }
    }
    return (value as unknown as object) ?? {};
}

function init(config: EditorJsFrameConfig): void {
    // EditorJS + each tool inject their CSS into this (iframe) document's head; add the plugin CSS too.
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

    // The Umbraco-free tools take a picker callback; delegate the actual pick to the parent bridge and
    // apply the returned data locally.
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

    // EditorJS ships a .d.ts whose `holder`/config generics are awkward to satisfy; cast to any for the
    // whole config object (as the original did).
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    let editor: any;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any, @typescript-eslint/no-unsafe-call
    editor = new (EditorJS as any)({
        holder,
        placeholder: "Type '/' to insert a block or just start typing something super...",
        data: getInitialData(config.value),
        inlineToolbar: true,
        // NOTE: global `sanitizer` is not applied during save() in EditorJS 2.x (see TECH_DEBT F-24).
        sanitizer: {
            a: {},
        },
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
                .then((outputData: unknown) => {
                    config.onChange(JSON.stringify(outputData));
                })
                .catch((error: unknown) => {
                    console.log('Saving failed: ', error);
                });
        },
        onReady: () => {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-call
            new DragDrop(editor);
        },
    });
}

// The shell talks to this frame through the <iframe> element itself (same-origin): it reads __n3oInit
// to start the editor and is notified via __n3oOnReady. Using the frame element (not postMessage / a
// window global) keeps everything per-instance and lets us pass live function references for the bridge.
type FrameHandshake = HTMLIFrameElement & {
    __n3oInit?: (config: EditorJsFrameConfig) => void;
    __n3oOnReady?: () => void;
};

const frameElement = window.frameElement as FrameHandshake | null;
if (frameElement) {
    frameElement.__n3oInit = init;
    frameElement.__n3oOnReady?.();
}
