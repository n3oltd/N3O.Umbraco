import { defineConfig } from 'vite';

// Builds the Text Resource Editor as a web-component shell that mounts a React app, into the shipped
// wwwroot/App_Plugins folder. @umbraco-cms/* AND react/react-dom are kept external — resolved at runtime
// by Umbraco's import map (react/react-dom are the self-hosted shared runtime from
// N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.ReactRuntime). Only this plugin's own code is bundled.
export default defineConfig({
    esbuild: { jsx: 'automatic' },
    build: {
        lib: {
            entry: 'src/text-resource-editor.ts',
            formats: ['es'],
            fileName: () => 'text-resource-editor.js',
        },
        outDir: '../wwwroot/App_Plugins/N3O.Umbraco.TextResourceEditor',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/, 'react', 'react-dom', 'react-dom/client', 'react/jsx-runtime'],
        },
    },
});
