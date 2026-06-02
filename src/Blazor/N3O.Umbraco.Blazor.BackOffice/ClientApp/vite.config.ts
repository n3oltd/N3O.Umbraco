import { defineConfig } from 'vite';

// Builds the Blazor BackOffice loader script into the shipped App_Plugins folder.
// This is a non-Lit bundle/script entry — it is a plain loader, not a custom element.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map);
// the loader's own code is bundled.
export default defineConfig({
    build: {
        lib: {
            entry: 'src/N3O.Umbraco.Blazor.BackOffice.ts',
            formats: ['es'],
            fileName: () => 'N3O.Umbraco.Blazor.BackOffice.js',
        },
        outDir: '../App_Plugins/N3O.Umbraco.Blazor.BackOffice',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
        },
    },
});
