import { UmbConditionBase } from '@umbraco-cms/backoffice/extension-registry';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { UMB_AUTH_CONTEXT } from '@umbraco-cms/backoffice/auth';
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
            // The endpoint is [Authorize(BackOfficeAccess)]. In Umbraco 17 the backoffice uses
            // cookie-based auth: the real token lives in an httpOnly cookie, and a request must
            // send credentials AND an `Authorization: Bearer <token>` header so the server's
            // HideBackOfficeTokensHandler swaps the real token in. getOpenApiConfiguration()
            // supplies the correct base, credentials ('include') and token sentinel ('[redacted]').
            // A plain fetch() (the previous implementation) sent neither and always got a 401.
            const authContext = await this.getContext(UMB_AUTH_CONTEXT);
            if (!authContext) {
                this.permitted = false;
                this.#args.onChange(this.permitted);
                return;
            }
            const config = authContext.getOpenApiConfiguration();

            const response = await fetch(`${config.base}/umbraco/backoffice/api/DynamicListViewApi/${contentKey}`, {
                credentials: config.credentials,
                headers: { Authorization: `Bearer ${await config.token()}` },
            });

            const data = (await response.json()) as { enabled: boolean };
            this.permitted = response.ok && data.enabled === true;
        } catch {
            this.permitted = false;
        }
        this.#args.onChange(this.permitted);
    }
}

export default DynamicListViewCondition;
