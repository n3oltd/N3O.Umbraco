import { defineConfig } from 'vite';

// react.js: bundles React (+ its scheduler dep) as a single self-hosted ESM module.
export default defineConfig({
    define: { 'process.env.NODE_ENV': JSON.stringify('production') },
    build: {
        outDir: '../App_Plugins/N3O.Umbraco.React',
        emptyOutDir: false,
        sourcemap: false,
        lib: { entry: { react: 'src/react.js' }, formats: ['es'] },
        rollupOptions: { output: { entryFileNames: '[name].js', chunkFileNames: 'react-internals-[hash].js' } },
    },
});
