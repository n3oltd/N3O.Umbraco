import { defineConfig } from 'vite';

// Bundles React + ReactDOM into self-hosted ESM files served from
// App_Plugins/N3O.Umbraco.React, mapped via that folder's umbraco-package.json importmap.
// react-dom / jsx-runtime / client keep `react` (and `react-dom`) external so there is ONE
// React instance shared across every plugin at runtime.
export default defineConfig({
    define: { 'process.env.NODE_ENV': JSON.stringify('production') },
    build: {
        outDir: '../App_Plugins/N3O.Umbraco.React',
        emptyOutDir: false,
        sourcemap: false,
        lib: {
            entry: {
                'react': 'src/react.js',
                'react-jsx-runtime': 'src/react-jsx-runtime.js',
                'react-dom': 'src/react-dom.js',
                'react-dom-client': 'src/react-dom-client.js',
            },
            formats: ['es'],
        },
        rollupOptions: {
            external: ['react', 'react/jsx-runtime', 'react-dom'],
            output: { entryFileNames: '[name].js', chunkFileNames: '[name]-[hash].js' },
        },
    },
});
