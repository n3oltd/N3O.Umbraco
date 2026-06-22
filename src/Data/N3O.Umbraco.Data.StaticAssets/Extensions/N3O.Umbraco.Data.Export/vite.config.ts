import { n3oPluginConfig } from '../../../../build/vite-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Data.Export/data-export': 'src/data-export.ts',
    },
    outDir: '../../wwwroot/App_Plugins',
    react: true,
    additionalExternals: ['@n3o/backoffice-core'],
});
