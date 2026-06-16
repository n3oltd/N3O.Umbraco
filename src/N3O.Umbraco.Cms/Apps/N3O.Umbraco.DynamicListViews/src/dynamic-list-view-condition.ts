import { UmbConditionBase } from '@umbraco-cms/backoffice/extension-registry';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { UMB_AUTH_CONTEXT } from '@umbraco-cms/backoffice/auth';
import type { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';
import type { UmbConditionConfigBase } from '@umbraco-cms/backoffice/extension-api';
import { createAuthFetch, type AuthFetch } from '@n3o/backoffice-core';

interface DynamicListViewConditionArgs {
    config: UmbConditionConfigBase;
    onChange: (permitted: boolean) => void;
}

export class DynamicListViewCondition extends UmbConditionBase<UmbConditionConfigBase> {
    readonly #args: DynamicListViewConditionArgs;
    #authFetch: AuthFetch | null = null;
    #unique: string | null = null;

    constructor(host: UmbControllerHost, args: DynamicListViewConditionArgs) {
        super(host, args);
        this.#args = args;

        // The endpoint is backoffice-authorized (DynamicListViewApiController extends
        // BackofficeAuthorizedApiController, [Authorize] policy = BackOfficeAccess), so the call
        // must carry the OAuth bearer token via createAuthFetch (see workspace-visibility-condition.ts).
        this.consumeContext(UMB_AUTH_CONTEXT, (authContext) => {
            this.#authFetch = authContext ? createAuthFetch(authContext.getOpenApiConfiguration()) : null;
            void this.#evaluate();
        });

        this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
            if (!context) { return; }
            this.observe(context.unique, (unique) => {
                this.#unique = unique ?? null;
                void this.#evaluate();
            });
        });
    }

    async #evaluate(): Promise<void> {
        // Wait until both the document key and the auth fetch are available.
        if (!this.#unique || !this.#authFetch) {
            return;
        }

        this.permitted = await this.#isEnabled(this.#unique);
        this.#args.onChange(this.permitted);
    }

    async #isEnabled(contentKey: string): Promise<boolean> {
        try {
            const response = await this.#authFetch!(`/umbraco/backoffice/api/DynamicListViewApi/${contentKey}`, {
                headers: { Accept: 'application/json' },
            });

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
