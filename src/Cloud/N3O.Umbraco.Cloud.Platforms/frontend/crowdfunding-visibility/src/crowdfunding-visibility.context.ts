import { UmbControllerBase } from '@umbraco-cms/backoffice/class-api';
import type { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import type { UmbDocumentWorkspaceContext } from '@umbraco-cms/backoffice/document';

const crowdfundingTabName = 'Crowdfunding';

const noteAliasMarker = 'savenote';

export class N3oCrowdfundingVisibilityContext extends UmbControllerBase {
    #workspaceContext?: UmbDocumentWorkspaceContext;
    #isNew = false;
    #tabIds: string[] = [];
    #groupIdsByTab = new Map<string, string[]>();
    #properties: Array<{ unique: string; alias: string; container?: { id: string } | null }> = [];
    #ruleUniques: string[] = [];

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

        const containerIds = new Set<string>(this.#tabIds);

        this.#groupIdsByTab.forEach((groupIds) => groupIds.forEach((id) => containerIds.add(id)));

        if (containerIds.size === 0) {
            return;
        }

        this.#properties
            .filter((property) => property.container != null && containerIds.has(property.container.id))
            .filter((property) => {
                const isNote = (property.alias ?? '').toLowerCase().endsWith(noteAliasMarker);

                return this.#isNew ? !isNote : isNote;
            })
            .forEach((property) => {
                const ruleUnique = `n3o-crowdfunding-${property.unique}`;

                context.propertyViewGuard.addRule({
                    unique: ruleUnique,
                    permitted: false,
                    propertyType: { unique: property.unique },
                });

                this.#ruleUniques.push(ruleUnique);
            });
    }
}

export { N3oCrowdfundingVisibilityContext as api };
export default N3oCrowdfundingVisibilityContext;
