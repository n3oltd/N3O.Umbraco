import { defineConfig } from 'vite';

// Builds the Cells editor as a web-component shell that mounts a React app, into the shipped
// App_Plugins folder. @umbraco-cms/* AND react/react-dom are kept external — resolved at runtime
// by Umbraco's import map (react/react-dom are the self-hosted shared runtime from
// N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.ReactRuntime). Handsontable and this plugin's own code ARE
// bundled into the single output file (Handsontable is not React).
export default defineConfig({
    esbuild: { jsx: 'automatic' },
    build: {
        lib: {
            entry: 'src/n3o-cells.ts',
            formats: ['es'],
            fileName: () => 'n3o-cells.js',
        },
        outDir: '../wwwroot/App_Plugins/N3O.Umbraco.Cells',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/, 'react', 'react-dom', 'react-dom/client', 'react/jsx-runtime'],
        },
    },
});
