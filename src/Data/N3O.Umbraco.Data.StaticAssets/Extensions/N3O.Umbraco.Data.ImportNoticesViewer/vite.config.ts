import { n3oPluginConfig } from '../../../../build/vite-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Data.ImportNoticesViewer/import-notices-viewer': 'src/import-notices-viewer.ts',
    },
    outDir: '../../wwwroot/App_Plugins',
    react: true,
    additionalExternals: ['@n3o/backoffice-core'],
});
