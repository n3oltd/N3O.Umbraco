import { defineConfig } from 'vite';

// jsx-runtime + react-dom (which also serves react-dom/client), plus the shared auth-fetch runtime.
// `react` stays external so the whole backoffice shares the single React instance from react.js, and
// `@umbraco-cms/*` stays external so auth-fetch.js resolves the auth/element APIs at runtime — both via
// the import map. Each emitted file becomes an import-map entry in umbraco-package.json.
export default defineConfig({
    define: { 'process.env.NODE_ENV': JSON.stringify('production') },
    build: {
        outDir: '../wwwroot/App_Plugins/N3O.Umbraco.ReactRuntime',
        emptyOutDir: false,
        sourcemap: false,
        lib: {
            entry: {
                'react-jsx-runtime': 'src/react-jsx-runtime.js',
                'react-dom': 'src/react-dom.js',
                'auth-fetch': 'src/auth-fetch.js',
                'workspace-visibility-condition': 'src/workspace-visibility-condition.js',
            },
            formats: ['es'],
        },
        rollupOptions: {
            external: ['react', /^@umbraco/],
            output: { entryFileNames: '[name].js', chunkFileNames: 'react-dom-internals-[hash].js' },
        },
    },
});
