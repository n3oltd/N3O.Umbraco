import {
    UMB_EDIT_DOCUMENT_WORKSPACE_PATH_PATTERN,
    UmbDocumentTreeRepository,
} from '@umbraco-cms/backoffice/document';
import type { UmbDocumentTreeItemModel } from '@umbraco-cms/backoffice/document';
import type { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';

// View models for the Dynamic List View, decoupled from Umbraco's raw tree-item shape so the
// presentation layer never reaches into `variants[0]`, `documentType`, etc.
export interface DynamicListViewItem {
    unique: string;
    name: string;
    state: string;
    icon: string;
    createDate: string;
    editPath: string;
}

export interface DynamicListViewChildren {
    items: DynamicListViewItem[];
    total: number;
}

const PAGE_SIZE = 100;

// Data access for the Dynamic List View. The doc types using this feature are deliberately NOT
// Umbraco collections (so the content tree keeps its inline expand-arrow navigation), which means
// the native document collection endpoint 400s for them; the document tree-children repository
// works on any node. Raw tree items are mapped to the view's own model here so the presentation
// layer stays decoupled from Umbraco's shapes.
export class DynamicChildrenRepository {
    readonly #treeRepository: UmbDocumentTreeRepository;

    constructor(host: UmbControllerHost) {
        this.#treeRepository = new UmbDocumentTreeRepository(host);
    }

    async getChildren(unique: string): Promise<DynamicListViewChildren> {
        const { data } = await this.#treeRepository.requestTreeItemsOf({
            parent: { unique, entityType: 'document' },
            paging: { skip: 0, take: PAGE_SIZE },
        });

        const items = (data?.items ?? []).map(toListViewItem);

        return { items, total: data?.total ?? items.length };
    }
}

function toListViewItem(item: UmbDocumentTreeItemModel): DynamicListViewItem {
    return {
        unique: item.unique,
        name: item.variants?.[0]?.name ?? '(unnamed)',
        state: item.variants?.[0]?.state ?? 'Unknown',
        icon: item.documentType.icon,
        createDate: item.createDate,
        editPath: UMB_EDIT_DOCUMENT_WORKSPACE_PATH_PATTERN.generateAbsolute({ unique: item.unique }),
    };
}
