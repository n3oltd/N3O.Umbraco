import { LitElement, css, customElement, html } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

const elementName = 'n3o-dynamic-list-view';

// Workspace view that delegates to Umbraco's built-in document collection workspace view element.
// Ported from the plain-JS dynamic-list-view.js.
@customElement(elementName)
export class N3oDynamicListViewElement extends UmbElementMixin(LitElement) {
    override render() {
        // Delegates to Umbraco's built-in document collection workspace view element.
        // If this renders blank, verify the element name in the installed Umbraco version:
        // grep for 'customElements.define' in node_modules/@umbraco-cms/backoffice/document*.js
        return html`<umb-document-workspace-view-collection></umb-document-workspace-view-collection>`;
    }

    static override styles = css`:host { display: block; height: 100%; }`;
}

export default N3oDynamicListViewElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oDynamicListViewElement;
    }
}
