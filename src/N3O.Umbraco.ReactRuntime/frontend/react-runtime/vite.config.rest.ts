import { defineConfig } from 'vite';

// jsx-runtime + react-dom (which also serves react-dom/client). `react` stays external so the whole
// backoffice shares the single React instance from react.js (via the import map). Each emitted file
// becomes an import-map entry in umbraco-package.json. (auth-fetch + the workspace-visibility condition
// moved to the separate N3O.Umbraco.BackofficeCore app.)
export default defineConfig({
    define: { 'process.env.NODE_ENV': JSON.stringify('production') },
    build: {
        outDir: 'dist/N3O.Umbraco.ReactRuntime',
        emptyOutDir: false,
        sourcemap: false,
        lib: {
            entry: {
                'react-jsx-runtime': 'src/react-jsx-runtime.js',
                'react-dom': 'src/react-dom.js',
            },
            formats: ['es'],
        },
        rollupOptions: {
            external: ['react', /^@umbraco/],
            output: { entryFileNames: '[name].js', chunkFileNames: 'react-dom-internals-[hash].js' },
        },
    },
});
