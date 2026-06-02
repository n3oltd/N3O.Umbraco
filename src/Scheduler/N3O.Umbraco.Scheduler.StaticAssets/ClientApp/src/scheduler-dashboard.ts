// NOTE: React shell is overhead here (the dashboard merely wraps a Hangfire <iframe>) — kept for
// uniformity per migration decision. A web-component-only Lit view would be lighter, but every
// backoffice plugin now follows the same React-shell pattern.
//
// Web-component SHELL for the Scheduler dashboard. Umbraco's backoffice only loads custom
// elements, so this thin element mounts the React UI (SchedulerDashboardApp) into its shadow
// root. React itself is NOT bundled here — it is external and resolved at runtime from the
// shared N3O.Umbraco.React import map.
import { customElement } from '@umbraco-cms/backoffice/external/lit';
import { createElement } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { SchedulerDashboardApp } from './scheduler-dashboard-app';

const elementName = 'n3o-scheduler-dashboard';

@customElement(elementName)
export class N3oSchedulerDashboardElement extends HTMLElement {
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
        this.#root.render(createElement(SchedulerDashboardApp));
    }

    disconnectedCallback(): void {
        this.#root?.unmount();
        this.#root = undefined;
    }
}

export default N3oSchedulerDashboardElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oSchedulerDashboardElement;
    }
}
