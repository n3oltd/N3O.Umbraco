import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.DynamicListViews/dynamic-list-view': 'src/dynamic-list-view.ts',
    },
    outDir: 'dist',
});
