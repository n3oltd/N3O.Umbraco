import { defineConfig } from 'vite';

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
