import { UmbConditionBase } from '@umbraco-cms/backoffice/extension-registry';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import type { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';
import type { UmbConditionConfigBase } from '@umbraco-cms/backoffice/extension-api';

interface DynamicListViewConditionArgs {
    config: UmbConditionConfigBase;
    onChange: (permitted: boolean) => void;
}

// Visibility policy for the Dynamic List View workspace view: it is shown only for documents the
// backend reports as enabled.
export class DynamicListViewCondition extends UmbConditionBase<UmbConditionConfigBase> {
    readonly #args: DynamicListViewConditionArgs;

    constructor(host: UmbControllerHost, args: DynamicListViewConditionArgs) {
        super(host, args);
        this.#args = args;

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            if (!context) { return; }
            this.observe(context.unique, (unique) => {
                if (unique) {
                    void this.#evaluate(unique);
                }
            });
        });
    }

    async #evaluate(contentKey: string): Promise<void> {
        this.permitted = await this.#isEnabled(contentKey);
        this.#args.onChange(this.permitted);
    }

    // Asks the backend whether the Dynamic List View is enabled for this document. The endpoint
    // (DynamicListViewApiController) is anonymous, so a plain same-origin fetch is all that's needed.
    async #isEnabled(contentKey: string): Promise<boolean> {
        try {
            const response = await fetch(`/umbraco/api/DynamicListViewApi/${contentKey}`);
            if (!response.ok) {
                return false;
            }

            const data = (await response.json()) as { enabled: boolean };

            return data.enabled === true;
        } catch {
            return false;
        }
    }
}

export default DynamicListViewCondition;
