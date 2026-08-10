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
import Quote from '@editorjs/quote';
import CodeTool from '@editorjs/code';
import Paragraph from '@editorjs/paragraph';
import DragDrop from 'editorjs-drag-drop';
import AlignmentTuneCtor from 'editor-js-alignment-tune';

import { makeUmbracoLinkTool } from './tools/UmbracoLinkTool';
import { makeUmbracoImageTool, type MediaPickerResultItem } from './tools/UmbracoImageTool';
import { EmbedWithUI } from './tools/EmbedWithUI';

// ---- media/link picker result shapes ----

// v17 verified: UmbPickerModalValue = { selection: Array<string | null> } (plain GUID strings).
interface MediaPickerResult {
    selection: Array<string | null>;
}

interface LinkPickerResult {
    link?: { url?: string; unique?: string };
}

// ---- small helpers ----

function randomUUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        const r = (Math.random() * 16) | 0;
        const v = c === 'x' ? r : (r & 0x3) | 0x8;
        return v.toString(16);
    });
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
            return {
                paragraph: {
                    class: Paragraph,
                    tunes: ['alignmentTune'],
                },
                header: {
                    class: Header,
                    tunes: ['alignmentTune'],
                },
                image: makeUmbracoImageTool(openMediaPicker),
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
                link: makeUmbracoLinkTool(openLinkPicker), // override link with Umbraco link picker
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

            // TODO: Not working — the global `sanitizer` option in EditorJS v2.x is NOT applied
            // during save(); save() uses each tool's static `sanitize` property exclusively.
            // The global option only affects certain paste/clipboard flows. To enforce per-output
            // HTML sanitization, add a `static get sanitize()` to each tool class. See TECH_DEBT
            // entry F-24 in TECH_DEBT_AND_MODERNIZATION.md for the full investigation.
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
