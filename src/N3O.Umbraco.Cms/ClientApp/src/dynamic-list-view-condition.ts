import { UmbConditionBase } from '@umbraco-cms/backoffice/extension-registry';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import type { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';
import type { UmbConditionConfigBase } from '@umbraco-cms/backoffice/extension-api';

interface DynamicListViewConditionArgs {
    config: UmbConditionConfigBase;
    onChange: (permitted: boolean) => void;
}

// Extension condition that calls the backend to determine whether the Dynamic List View
// workspace view should be shown for a given document. Ported from the plain-JS
// dynamic-list-view-condition.js.
export class DynamicListViewCondition extends UmbConditionBase<UmbConditionConfigBase> {
    readonly #args: DynamicListViewConditionArgs;

    constructor(host: UmbControllerHost, args: DynamicListViewConditionArgs) {
        super(host, args);
        this.#args = args;

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            if (!context) { return; }
            this.observe(context.unique, (unique) => {
                if (unique) {
                    void this.#checkApi(unique);
                }
            });
        });
    }

    async #checkApi(contentKey: string): Promise<void> {
        try {
            const response = await fetch(`/umbraco/backoffice/api/DynamicListViewApi/${contentKey}`);
            const data = (await response.json()) as { enabled: boolean };
            this.permitted = response.ok && data.enabled;
        } catch {
            this.permitted = false;
        }
        this.#args.onChange(this.permitted);
    }
}

export default DynamicListViewCondition;
