import { defineConfig } from 'vite';

// Shared Vite preset for the N3O backoffice client apps. Every app builds as an ES-module library
// straight into its shipped App_Plugins folder. @umbraco-cms/* imports are always kept external —
// resolved at runtime by Umbraco's import map. React apps additionally keep react/react-dom external
// (the self-hosted shared runtime in N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.ReactRuntime) and
// compile JSX with the automatic runtime; each app's own code (and any non-React third-party libs)
// is bundled.
//
// Plain ESM (not .ts): apps import this BY NAME via '@n3o/build', and Vite's config bundler
// externalizes the resolved bare specifier, so Node loads this .js directly. Types live in
// vite-config.d.ts (IDE only). Keep this file and vite-config.d.ts in sync.
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
