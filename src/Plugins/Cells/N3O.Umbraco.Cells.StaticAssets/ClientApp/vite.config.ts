import { defineConfig } from 'vite';

// Builds the Cells Lit component into the shipped App_Plugins folder.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map);
// handsontable and all other code is bundled into the single output file.
export default defineConfig({
    build: {
        lib: {
            entry: 'src/n3o-cells.ts',
            formats: ['es'],
            fileName: () => 'n3o-cells.js',
        },
        outDir: '../App_Plugins/N3O.Umbraco.Cells',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
        },
    },
});
