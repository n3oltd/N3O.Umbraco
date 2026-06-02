import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import {
    UmbPropertyValueChangeEvent,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { UMB_MODAL_MANAGER_CONTEXT } from '@umbraco-cms/backoffice/modal';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { EditorJsApp, type EditorJsHostBridge } from './editor-js-app';

const elementName = 'n3o-editor-js';

// Web-component SHELL for the EditorJS property editor. Umbraco's backoffice only loads custom
// elements, so this thin element owns the Umbraco contract (value + UmbPropertyValueChangeEvent),
// consumes UMB_MODAL_MANAGER_CONTEXT (needed by the EditorJS image/link tools' media & link pickers),
// and mounts the React UI (EditorJsApp) into its shadow root. React itself is NOT bundled here — it
// is external and resolved at runtime from the shared N3O.Umbraco.React import map. The @editorjs/*
// libraries ARE bundled (they are not React).
//
// UmbElementMixin(HTMLElement) is used (not LitElement) purely for context plumbing; React renders
// the UI. The value contract is unchanged: a JSON string (Umbraco may also hand back an object,
// handled in the React app).
@customElement(elementName)
export class N3oEditorJsElement
    extends UmbElementMixin(HTMLElement)
    implements UmbPropertyEditorUiElement
{
    #root?: Root;
    #mount: HTMLDivElement;
    #value: string | undefined;
    // FLAG: UMB_MODAL_MANAGER_CONTEXT consumer value — using `any` then casting at call site (as in
    // the original Lit version).
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    #modalManager: any;

    constructor() {
        super();

        const shadow = this.attachShadow({ mode: 'open' });
        this.#mount = document.createElement('div');
        shadow.appendChild(this.#mount);

        this.consumeContext(UMB_MODAL_MANAGER_CONTEXT, (context) => {
            this.#modalManager = context;
            this.#render();
        });
    }

    get value(): string | undefined {
        return this.#value;
    }

    set value(value: string | undefined) {
        this.#value = value;
        this.#render();
    }

    connectedCallback(): void {
        super.connectedCallback();
        this.#root ??= createRoot(this.#mount);
        this.#render();
    }

    disconnectedCallback(): void {
        super.disconnectedCallback();
        this.#root?.unmount();
        this.#root = undefined;
    }

    #render(): void {
        if (!this.#root) {
            return;
        }

        const bridge: EditorJsHostBridge = {
            host: this,
            modalManager: this.#modalManager,
        };

        this.#root.render(
            createElement(EditorJsApp, {
                value: this.#value,
                bridge,
                onChange: (value: string) => {
                    this.#value = value;
                    this.dispatchEvent(new UmbPropertyValueChangeEvent());
                },
            }),
        );
    }
}

export default N3oEditorJsElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oEditorJsElement;
    }
}
