import { defineConfig } from 'vite';

// Builds the SERP editor Lit component into the shipped App_Plugins folder.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map);
// the component's own code (and any third-party libs) are bundled.
export default defineConfig({
    build: {
        lib: {
            entry: 'src/serp-editor.ts',
            formats: ['es'],
            fileName: () => 'serp-editor.js',
        },
        outDir: '../App_Plugins/N3O.Umbraco.SerpEditor',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
        },
    },
});
