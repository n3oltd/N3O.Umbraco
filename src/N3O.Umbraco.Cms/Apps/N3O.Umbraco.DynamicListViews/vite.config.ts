import { defineConfig } from 'vite';

// Builds both DynamicListViews Lit components into the shipped App_Plugins folder.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map).
export default defineConfig({
    build: {
        lib: {
            entry: {
                'N3O.Umbraco.DynamicListViews/dynamic-list-view': 'src/dynamic-list-view.ts',
                'N3O.Umbraco.DynamicListViews/dynamic-list-view-condition': 'src/dynamic-list-view-condition.ts',
            },
            formats: ['es'],
        },
        outDir: '../../wwwroot/App_Plugins',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
            output: { entryFileNames: '[name].js' },
        },
    },
});
