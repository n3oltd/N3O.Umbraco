import { defineConfig } from 'vite';

// Builds all four Data plugin Lit/TypeScript components into their respective App_Plugins folders.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map);
// each component's own code is bundled into one output file per entry.
export default defineConfig({
    build: {
        lib: {
            entry: {
                'N3O.Umbraco.Data.Import/data-import': 'src/data-import.ts',
                'N3O.Umbraco.Data.Export/data-export': 'src/data-export.ts',
                'N3O.Umbraco.Data.ImportDataEditor/import-data-editor': 'src/import-data-editor.ts',
                'N3O.Umbraco.Data.ImportNoticesViewer/import-notices-viewer': 'src/import-notices-viewer.ts',
            },
            formats: ['es'],
        },
        outDir: '../App_Plugins',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
            output: { entryFileNames: '[name].js' },
        },
    },
});
