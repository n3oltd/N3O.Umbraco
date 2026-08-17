// Umbraco's backoffice loads custom elements only, which is why the React UI needs this shell. React
// is external, resolved at runtime from the shared N3O.Umbraco.ReactRuntime import map.
import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { WelcomeDashboardApp } from './welcome-dashboard-app';

const elementName = 'n3o-welcome-dashboard';

@customElement(elementName)
export class N3oWelcomeDashboardElement extends HTMLElement {
    #root?: Root;
    #mount: HTMLDivElement;

    constructor() {
        super();
        const shadow = this.attachShadow({ mode: 'open' });
        this.#mount = document.createElement('div');
        shadow.appendChild(this.#mount);
    }

    connectedCallback(): void {
        this.#root ??= createRoot(this.#mount);
        this.#root.render(createElement(WelcomeDashboardApp));
    }

    disconnectedCallback(): void {
        this.#root?.unmount();
        this.#root = undefined;
    }
}

export default N3oWelcomeDashboardElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oWelcomeDashboardElement;
    }
}
