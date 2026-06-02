import { defineConfig } from 'vite';

// Builds the Text Resource Editor Lit component into the shipped App_Plugins folder.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map);
// the component's own code (and any third-party libs) are bundled.
export default defineConfig({
    build: {
        lib: {
            entry: 'src/text-resource-editor.ts',
            formats: ['es'],
            fileName: () => 'text-resource-editor.js',
        },
        outDir: '../App_Plugins/N3O.Umbraco.TextResourceEditor',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
        },
    },
});
