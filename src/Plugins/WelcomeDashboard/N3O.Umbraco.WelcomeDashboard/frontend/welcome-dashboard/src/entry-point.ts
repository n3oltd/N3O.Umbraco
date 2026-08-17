import type { UmbEntryPointOnInit } from '@umbraco-cms/backoffice/extension-api';

const hiddenContentDashboards = ['Umb.Dashboard.UmbracoNews', 'Umb.Dashboard.RedirectManagement'];

export const onInit: UmbEntryPointOnInit = (_host, extensionRegistry) => {
    hiddenContentDashboards.forEach((alias) => extensionRegistry.exclude(alias));
};
