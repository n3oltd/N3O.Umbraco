import {
    UMB_EDIT_DOCUMENT_WORKSPACE_PATH_PATTERN,
    UmbDocumentTreeRepository,
} from '@umbraco-cms/backoffice/document';
import type { UmbDocumentTreeItemModel } from '@umbraco-cms/backoffice/document';
import type { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';

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

const PAGE_SIZE = 50;

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
