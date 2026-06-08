import { LitElement, css, customElement, html, nothing, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import {
    UMB_DOCUMENT_WORKSPACE_CONTEXT,
    UMB_EDIT_DOCUMENT_WORKSPACE_PATH_PATTERN,
    UmbDocumentTreeRepository,
} from '@umbraco-cms/backoffice/document';
import type { UmbDocumentTreeItemModel } from '@umbraco-cms/backoffice/document';

const elementName = 'n3o-dynamic-list-view';
const PAGE_SIZE = 100;

// Workspace view that lists a document's children. The doc types using this feature are deliberately
// NOT configured as Umbraco collections, so the content tree keeps its normal inline expand-arrow
// navigation. We therefore can't use the native document collection endpoint (it 400s for
// non-collection nodes); instead we list children via the document tree children repository, which
// works for any node. This view is gated per-node by the N3O.Condition.DynamicListView condition.
@customElement(elementName)
export class N3oDynamicListViewElement extends UmbElementMixin(LitElement) {
    @state()
    private _items: UmbDocumentTreeItemModel[] = [];

    @state()
    private _total = 0;

    @state()
    private _loading = true;

    readonly #treeRepository = new UmbDocumentTreeRepository(this);

    constructor() {
        super();

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            if (!context) { return; }
            this.observe(context.unique, (unique) => {
                if (unique) {
                    void this.#load(unique);
                } else {
                    this._items = [];
                    this._total = 0;
                }
            });
        });
    }

    async #load(unique: string): Promise<void> {
        this._loading = true;
        const { data } = await this.#treeRepository.requestTreeItemsOf({
            parent: { unique, entityType: 'document' },
            paging: { skip: 0, take: PAGE_SIZE },
        });
        this._items = data?.items ?? [];
        this._total = data?.total ?? this._items.length;
        this._loading = false;
    }

    #name(item: UmbDocumentTreeItemModel): string {
        return item.variants?.[0]?.name ?? '(unnamed)';
    }

    #state(item: UmbDocumentTreeItemModel): string {
        return item.variants?.[0]?.state ?? 'Unknown';
    }

    #editPath(unique: string): string {
        return UMB_EDIT_DOCUMENT_WORKSPACE_PATH_PATTERN.generateAbsolute({ unique });
    }

    #renderRow(item: UmbDocumentTreeItemModel) {
        const state = this.#state(item);
        const color = state === 'Published' ? 'positive' : state === 'Draft' ? 'warning' : 'default';
        return html`
            <uui-table-row>
                <uui-table-cell>
                    <uui-button compact look="default" href=${this.#editPath(item.unique)} label=${this.#name(item)}>
                        <uui-icon name=${item.documentType.icon}></uui-icon>
                        <span style="margin-left: var(--uui-size-space-2)">${this.#name(item)}</span>
                    </uui-button>
                </uui-table-cell>
                <uui-table-cell><uui-tag color=${color} look="secondary">${state}</uui-tag></uui-table-cell>
                <uui-table-cell>${new Date(item.createDate).toLocaleDateString()}</uui-table-cell>
            </uui-table-row>
        `;
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
