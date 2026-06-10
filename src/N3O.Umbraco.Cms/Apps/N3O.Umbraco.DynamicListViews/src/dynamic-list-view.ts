import { LitElement, css, customElement, html, nothing, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { DynamicChildrenRepository } from './dynamic-children.repository';
import type { DynamicListViewItem } from './dynamic-children.repository';

const elementName = 'n3o-dynamic-list-view';

// Workspace view that lists a document's children, gated per-node by the
// N3O.Condition.DynamicListView condition. This element is presentation only: it observes the
// current document and delegates loading + mapping to DynamicChildrenRepository.
@customElement(elementName)
export class N3oDynamicListViewElement extends UmbElementMixin(LitElement) {
    @state()
    private _items: DynamicListViewItem[] = [];

    @state()
    private _total = 0;

    @state()
    private _loading = true;

    readonly #repository = new DynamicChildrenRepository(this);

    constructor() {
        super();

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            if (!context) { return; }
            this.observe(context.unique, (unique) => {
                if (unique) {
                    void this.#load(unique);
                } else {
                    this.#reset();
                }
            });
        });
    }

    async #load(unique: string): Promise<void> {
        this._loading = true;
        const { items, total } = await this.#repository.getChildren(unique);
        this._items = items;
        this._total = total;
        this._loading = false;
    }

    #reset(): void {
        this._items = [];
        this._total = 0;
    }

    override render() {
        if (this._loading) { return html`<div class="center"><uui-loader></uui-loader></div>`; }
        if (!this._items.length) { return html`<uui-box><div class="center">There are no child items.</div></uui-box>`; }

        return html`
            <uui-box>
                <uui-table>
                    <uui-table-head>
                        <uui-table-head-cell>Name</uui-table-head-cell>
                        <uui-table-head-cell>Status</uui-table-head-cell>
                        <uui-table-head-cell>Created</uui-table-head-cell>
                    </uui-table-head>
                    ${this._items.map((item) => this.#renderRow(item))}
                </uui-table>
                ${this._total > this._items.length
                    ? html`<div class="footnote">Showing ${this._items.length} of ${this._total} items.</div>`
                    : nothing}
            </uui-box>
        `;
    }

    #renderRow(item: DynamicListViewItem) {
        const color = item.state === 'Published' ? 'positive' : item.state === 'Draft' ? 'warning' : 'default';
        return html`
            <uui-table-row>
                <uui-table-cell>
                    <uui-button compact look="default" href=${item.editPath} label=${item.name}>
                        <uui-icon name=${item.icon}></uui-icon>
                        <span style="margin-left: var(--uui-size-space-2)">${item.name}</span>
                    </uui-button>
                </uui-table-cell>
                <uui-table-cell><uui-tag color=${color} look="secondary">${item.state}</uui-tag></uui-table-cell>
                <uui-table-cell>${new Date(item.createDate).toLocaleDateString()}</uui-table-cell>
            </uui-table-row>
        `;
    }

    static override styles = css`
        :host { display: block; padding: var(--uui-size-layout-1); }
        .center { display: flex; justify-content: center; padding: var(--uui-size-layout-1); }
        .footnote { color: var(--uui-color-text-alt); font-size: var(--uui-type-small-size); padding-top: var(--uui-size-space-4); }
        uui-table-cell uui-button { --uui-button-padding-left-factor: 0; text-align: left; }
    `;
}

export default N3oDynamicListViewElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oDynamicListViewElement;
    }
}
