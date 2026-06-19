import { n3oPluginConfig } from '../../../../build/vite-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Data.Import/data-import': 'src/data-import.ts',
    },
    outDir: '../../wwwroot/App_Plugins',
    react: true,
    additionalExternals: ['@n3o/backoffice-core'],
});
