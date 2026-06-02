import { defineConfig } from 'vite';

// Builds the Scheduler dashboard web-component shell (which mounts a React app) into the shipped
// App_Plugins folder. @umbraco-cms/* AND react/react-dom are kept external — resolved at runtime
// by Umbraco's import map (react/react-dom are the self-hosted shared runtime from
// N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.React). Only this plugin's own code is bundled.
export default defineConfig({
    esbuild: { jsx: 'automatic' },
    build: {
        lib: {
            entry: 'src/scheduler-dashboard.ts',
            formats: ['es'],
            fileName: () => 'scheduler-dashboard.js',
        },
        outDir: '../App_Plugins/N3O.Umbraco.Scheduler',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/, 'react', 'react-dom', 'react-dom/client', 'react/jsx-runtime'],
        },
    },
});
