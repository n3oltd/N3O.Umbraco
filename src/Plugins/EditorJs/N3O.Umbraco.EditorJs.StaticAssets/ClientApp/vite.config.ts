import { defineConfig } from 'vite';

// Builds the EditorJS Lit component into the shipped App_Plugins folder.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map);
// the component's own code and all @editorjs/* / editorjs-* third-party libs are bundled.
export default defineConfig({
    build: {
        lib: {
            entry: 'src/editor-js.ts',
            formats: ['es'],
            fileName: () => 'editor-js.js',
        },
        outDir: '../App_Plugins/N3O.Umbraco.EditorJs',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
        },
    },
});
