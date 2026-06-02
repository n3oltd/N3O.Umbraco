import { defineConfig } from 'vite';

// Builds the Platforms Preview Lit component into the shipped App_Plugins folder.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map);
// the component's own code is bundled into a single ES module.
export default defineConfig({
    build: {
        lib: {
            entry: 'src/platforms-preview.ts',
            formats: ['es'],
            fileName: () => 'platforms-preview.js',
        },
        outDir: '../App_Plugins/N3O.Umbraco.Cloud.Platforms.Preview',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
        },
    },
});
