import { UmbConditionBase } from '@umbraco-cms/backoffice/extension-registry';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { UMB_AUTH_CONTEXT } from '@umbraco-cms/backoffice/auth';
import type { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';
import type { UmbConditionConfigBase, UmbConditionControllerArguments } from '@umbraco-cms/backoffice/extension-api';
import { createAuthFetch, type AuthFetch } from './auth-fetch.js';

export interface WorkspaceVisibilityRes {
    visible: boolean;
}

export type WorkspaceVisibilityConditionConfig = UmbConditionConfigBase & {
    endpoint?: string;
};

export class WorkspaceVisibilityCondition extends UmbConditionBase<WorkspaceVisibilityConditionConfig> {
    #args: UmbConditionControllerArguments<WorkspaceVisibilityConditionConfig>;
    #authFetch: AuthFetch | null = null;
    #unique: string | null = null;
    #generation = 0;

    constructor(host: UmbControllerHost, args: UmbConditionControllerArguments<WorkspaceVisibilityConditionConfig>) {
        super(host, args);
        this.#args = args;

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
        const endpoint = this.#args.config?.endpoint;
        const unique = this.#unique;
        const authFetch = this.#authFetch;

        if (!endpoint || !unique || !authFetch) {
            return;
        }

        const generation = ++this.#generation;

        this.permitted = false;
        this.#args.onChange(false);

        const permitted = await this.#isPermitted(endpoint, unique, authFetch);

        if (generation !== this.#generation) {
            return;
        }

        this.permitted = permitted;
        this.#args.onChange(this.permitted);
    }

    async #isPermitted(endpoint: string, unique: string, authFetch: AuthFetch): Promise<boolean> {
        try {
            const response = await authFetch(`${endpoint}/${unique}`, {
                headers: { Accept: 'application/json' },
            });

            if (!response.ok) {
                return false;
            }

            const data = await response.json() as WorkspaceVisibilityRes;

            if (typeof data.visible !== 'boolean') {
                console.error('[WorkspaceVisibilityCondition] Unexpected response shape from', endpoint, '— expected { visible: boolean }, got', data);
                return false;
            }

            return data.visible;
        } catch {
            return false;
        }
    }
}

export default WorkspaceVisibilityCondition;
