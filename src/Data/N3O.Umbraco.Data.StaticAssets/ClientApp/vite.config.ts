import { defineConfig } from 'vite';

// Builds all four Data plugin web-component shells (each mounts a React app) into their respective
// App_Plugins folders. @umbraco-cms/*, react/react-dom AND @n3o/auth-fetch are kept external — resolved
// at runtime by Umbraco's import map (react/react-dom and @n3o/auth-fetch are the self-hosted shared
// runtime from N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.ReactRuntime). Only each plugin's own code is
// bundled.
export default defineConfig({
    esbuild: { jsx: 'automatic' },
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
        outDir: '../wwwroot/App_Plugins',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/, 'react', 'react-dom', 'react-dom/client', 'react/jsx-runtime', '@n3o/auth-fetch'],
            output: { entryFileNames: '[name].js' },
        },
    },
});
