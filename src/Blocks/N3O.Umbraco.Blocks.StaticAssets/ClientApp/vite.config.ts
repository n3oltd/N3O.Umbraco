import { defineConfig } from 'vite';

// Builds the block preview Lit component into the shipped App_Plugins folder.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map);
// the component's own code is bundled.
export default defineConfig({
    build: {
        lib: {
            entry: 'src/block-preview.ts',
            formats: ['es'],
            fileName: () => 'block-preview.js',
        },
        outDir: '../App_Plugins/N3O.Umbraco.Blocks.Preview',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
        },
    },
});
