// Shared, configurable workspace-view visibility condition, self-hosted in the ReactRuntime and
// registered once via this project's umbraco-package.json as `N3O.Condition.WorkspaceVisibility`.
//
// WHY: in Umbraco 13 a content app (IContentAppFactory.GetContentAppFor) could decide PER NODE / PER USER
// whether to show — e.g. Data Export/Import ran IExportContentFilter/IImportContentFilter + a user-group
// check, and Platforms Preview checked the document type's composition. Content apps are now manifest
// `workspaceView` extensions and that server-side gating was lost (they show on every document). This
// condition restores it generically: a workspace view lists it in its `conditions` with an `endpoint`,
// and the view is permitted only when `GET {endpoint}/{documentUnique}` returns `{ permitted: true }`.
//
// The call is authenticated (the gating often depends on the current user's groups), using createAuthFetch
// from @n3o/backoffice-core. `@umbraco-cms/*` stays external (resolved at runtime via the import map);
// this file is loaded directly by Umbraco as the condition's `api`.
import { UmbConditionBase } from '@umbraco-cms/backoffice/extension-registry';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { UMB_AUTH_CONTEXT } from '@umbraco-cms/backoffice/auth';
import type { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';
import type { UmbConditionConfigBase, UmbConditionControllerArguments } from '@umbraco-cms/backoffice/extension-api';
import { createAuthFetch, type AuthFetch } from './auth-fetch.js';

export type WorkspaceVisibilityConditionConfig = UmbConditionConfigBase & {
    /** Backoffice API endpoint returning `{ permitted: boolean }` for `GET {endpoint}/{documentUnique}`. */
    endpoint?: string;
};

export class WorkspaceVisibilityCondition extends UmbConditionBase<WorkspaceVisibilityConditionConfig> {
    #args: UmbConditionControllerArguments<WorkspaceVisibilityConditionConfig>;
    #authFetch: AuthFetch | null = null;
    #unique: string | null = null;

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

        // Wait until the document key, the auth fetch and the configured endpoint are all available.
        if (!endpoint || !this.#unique || !this.#authFetch) {
            return;
        }

        this.permitted = await this.#isPermitted(endpoint, this.#unique);
        this.#args.onChange(this.permitted);
    }

    async #isPermitted(endpoint: string, unique: string): Promise<boolean> {
        try {
            const response = await this.#authFetch!(`${endpoint}/${unique}`, {
                headers: { Accept: 'application/json' },
            });

            if (!response.ok) {
                return false;
            }

            const data = await response.json();

            return data?.permitted === true;
        } catch {
            return false;
        }
    }
}

export default WorkspaceVisibilityCondition;
