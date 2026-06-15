import { n3oPluginConfig } from '@n3o/build';

// Shared backoffice-core modules, served to all plugins via the import map (umbraco-package.json):
//  - auth-fetch.js          -> the authenticated-fetch runtime, import-mapped as "@n3o/backoffice-core"
//  - workspace-visibility-condition.js -> the N3O.Condition.WorkspaceVisibility condition extension
// @umbraco-cms/* stays external (resolved at runtime by Umbraco's import map). No React here.
export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.BackofficeCore/auth-fetch': 'src/auth-fetch.ts',
        'N3O.Umbraco.BackofficeCore/workspace-visibility-condition': 'src/workspace-visibility-condition.ts',
    },
    outDir: '../../wwwroot/App_Plugins',
});
