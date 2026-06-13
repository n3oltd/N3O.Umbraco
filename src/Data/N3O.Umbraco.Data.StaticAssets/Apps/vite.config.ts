import { n3oPluginConfig } from '../../../build/vite-config';

// Builds all four Data plugin web-component shells (each mounts a React app) into their respective
// App_Plugins folders. @umbraco-cms/*, react/react-dom AND @n3o/backoffice-core are kept external — resolved
// at runtime by Umbraco's import map (react/react-dom and @n3o/backoffice-core are the self-hosted shared
// runtime from N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.ReactRuntime). Only each plugin's own code is
// bundled.
export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Data.Import/data-import': 'src/data-import.ts',
        'N3O.Umbraco.Data.Export/data-export': 'src/data-export.ts',
        'N3O.Umbraco.Data.ImportDataEditor/import-data-editor': 'src/import-data-editor.ts',
        'N3O.Umbraco.Data.ImportNoticesViewer/import-notices-viewer': 'src/import-notices-viewer.ts',
    },
    outDir: '../wwwroot/App_Plugins',
    react: true,
    additionalExternals: ['@n3o/backoffice-core'],
});
