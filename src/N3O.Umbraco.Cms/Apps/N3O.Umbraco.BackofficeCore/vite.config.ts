import { n3oPluginConfig } from '@n3o/build';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.BackofficeCore/auth-fetch': 'src/auth-fetch.ts',
        'N3O.Umbraco.BackofficeCore/workspace-visibility-condition': 'src/workspace-visibility-condition.ts',
    },
    outDir: '../../wwwroot/App_Plugins',
});
