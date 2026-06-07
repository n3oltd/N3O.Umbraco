import { defineConfig } from 'vite';

// jsx-runtime + react-dom (which also serves react-dom/client). `react` stays external so the
// whole backoffice shares the single React instance from react.js (via the import map).
export default defineConfig({
    define: { 'process.env.NODE_ENV': JSON.stringify('production') },
    build: {
        outDir: '../wwwroot/App_Plugins/N3O.Umbraco.React',
        emptyOutDir: false,
        sourcemap: false,
        lib: {
            entry: { 'react-jsx-runtime': 'src/react-jsx-runtime.js', 'react-dom': 'src/react-dom.js' },
            formats: ['es'],
        },
        rollupOptions: {
            external: ['react'],
            output: { entryFileNames: '[name].js', chunkFileNames: 'react-dom-internals-[hash].js' },
        },
    },
});
