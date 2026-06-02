import { defineConfig } from 'vite';

// Builds the Scheduler dashboard Lit component into the shipped App_Plugins folder.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map);
// the component's own code is bundled.
export default defineConfig({
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
            external: [/^@umbraco/],
        },
    },
});
