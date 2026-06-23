import { defineConfig } from 'vite';

// react.js: bundles React (+ its scheduler dep) as a single self-hosted ESM module. This is the first of
// the runtime's two vite passes (see package.json), so it cleans the shared outDir to evict stale hashed
// internals chunks (react-internals-*.js / react-dom-internals-*.js) left by a previous build; the second
// pass (vite.config.rest.ts) keeps emptyOutDir:false so it appends rather than wiping this pass's output.
export default defineConfig({
    define: { 'process.env.NODE_ENV': JSON.stringify('production') },
    build: {
        outDir: 'dist/N3O.Umbraco.ReactRuntime',
        emptyOutDir: true,
        sourcemap: false,
        lib: { entry: { react: 'src/react.js' }, formats: ['es'] },
        rollupOptions: { output: { entryFileNames: '[name].js', chunkFileNames: 'react-internals-[hash].js' } },
    },
});
