import { n3oPluginConfig } from '@n3o/build';

// Builds the DynamicListViews Lit component into the shipped App_Plugins folder.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map).
export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.DynamicListViews/dynamic-list-view': 'src/dynamic-list-view.ts',
    },
    outDir: '../../wwwroot/App_Plugins',
});
