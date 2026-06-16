import { defineConfig } from 'vite';

export function n3oPluginConfig(options) {
    const { entries, outDir, react = false, additionalExternals = [], sourcemap = true } = options;

    const external = [/^@umbraco/];

    if (react) {
        external.push('react', 'react-dom', 'react-dom/client', 'react/jsx-runtime');
    }

    external.push(...additionalExternals);

    return defineConfig({
        ...(react ? { esbuild: { jsx: 'automatic' } } : {}),
        build: {
            lib: {
                entry: entries,
                formats: ['es'],
            },
            outDir,
            emptyOutDir: false,
            sourcemap,
            rollupOptions: {
                external,
                output: { entryFileNames: '[name].js' },
            },
        },
    });
}
