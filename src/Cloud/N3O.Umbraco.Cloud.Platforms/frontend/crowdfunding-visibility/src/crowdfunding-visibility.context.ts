import { UmbControllerBase } from '@umbraco-cms/backoffice/class-api';
import type { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import type { UmbDocumentWorkspaceContext } from '@umbraco-cms/backoffice/document';

const crowdfundingTabName = 'Crowdfunding';

export class N3oCrowdfundingVisibilityContext extends UmbControllerBase {
    #workspaceContext?: UmbDocumentWorkspaceContext;
    #isNew = false;
    #tabIds: string[] = [];
    #groupIdsByTab = new Map<string, string[]>();
    #properties: Array<{ unique: string; container?: { id: string } | null }> = [];
    #ruleUniques: Array<string | symbol> = [];

    constructor(host: UmbControllerHost) {
        super(host);

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            this.#workspaceContext = context ?? undefined;

            if (!context) {
                return;
            }

            this.observe(context.isNew, (isNew) => {
                this.#isNew = isNew === true;
                this.#apply();
            }, '_n3oCrowdfundingIsNew');

            this.observe(context.structure.contentTypeProperties, (properties) => {
                this.#properties = properties ?? [];
                this.#apply();
            }, '_n3oCrowdfundingProperties');

            this.observe(context.structure.containersByNameAndType(crowdfundingTabName, 'Tab'), (tabs) => {
                this.#tabIds = (tabs ?? []).map((x) => x.id);
                this.#observeGroups();
                this.#apply();
            }, '_n3oCrowdfundingTabs');
        });
    }

    #observeGroups(): void {
        const context = this.#workspaceContext;

        if (!context) {
            return;
        }

        this.#groupIdsByTab.clear();

        this.#tabIds.forEach((tabId) => {
            this.observe(context.structure.containersOfParentId(tabId, 'Group'), (groups) => {
                this.#groupIdsByTab.set(tabId, (groups ?? []).map((x) => x.id));
                this.#apply();
            }, `_n3oCrowdfundingGroups_${tabId}`);
        });
    }

    #apply(): void {
        const context = this.#workspaceContext;

        if (!context) {
            return;
        }

        if (this.#ruleUniques.length) {
            context.propertyViewGuard.removeRules(this.#ruleUniques);
            this.#ruleUniques = [];
        }

        if (!this.#isNew) {
            return;
        }

        const containerIds = new Set<string>(this.#tabIds);

        this.#groupIdsByTab.forEach((groupIds) => groupIds.forEach((id) => containerIds.add(id)));

        if (containerIds.size === 0) {
            return;
        }

        this.#properties
            .filter((property) => property.container != null && containerIds.has(property.container.id))
            .forEach((property) => {
                const ruleUnique = context.propertyViewGuard.addRule({
                    permitted: false,
                    propertyType: { unique: property.unique },
                });

                if (ruleUnique != null) {
                    this.#ruleUniques.push(ruleUnique);
                }
            });
    }
}

export { N3oCrowdfundingVisibilityContext as api };
export default N3oCrowdfundingVisibilityContext;
