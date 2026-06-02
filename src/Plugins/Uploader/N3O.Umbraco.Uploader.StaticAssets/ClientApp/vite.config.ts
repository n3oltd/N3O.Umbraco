import { defineConfig } from 'vite';

// Builds the Uploader Lit component into the shipped App_Plugins folder.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map);
// the component's own code (and any third-party libs) are bundled.
// NOTE: Formstone (core.js / upload.js) and jQuery are NOT bundled here — they are kept as
// vendored static files in App_Plugins and loaded at runtime by the component. See uploader.ts.
export default defineConfig({
    build: {
        lib: {
            entry: 'src/uploader.ts',
            formats: ['es'],
            fileName: () => 'uploader.js',
        },
        outDir: '../App_Plugins/N3O.Umbraco.Uploader',
        emptyOutDir: false,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/],
        },
    },
});
