import { defineConfig } from 'vite';

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
