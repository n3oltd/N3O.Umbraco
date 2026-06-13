import { defineConfig, type UserConfig } from 'vite';

// Shared Vite preset for the N3O backoffice client apps. Every app builds as an ES-module library
// straight into its shipped App_Plugins folder. @umbraco-cms/* imports are always kept external —
// resolved at runtime by Umbraco's import map. React apps additionally keep react/react-dom external
// (the self-hosted shared runtime from N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.ReactRuntime) and
// compile JSX with the automatic runtime; each app's own code (and any non-React third-party libs)
// is bundled.
interface N3oPluginConfigOptions {
    /** Output name (may include a subfolder, e.g. 'N3O.Umbraco.Cropper/cropper') → source entry file. */
    entries: Record<string, string>;
    outDir: string;
    /** Externalizes react/react-dom (shared runtime) and enables the automatic JSX runtime. */
    react?: boolean;
    /** Extra externals on top of the standard list (e.g. '@n3o/auth-fetch'). */
    additionalExternals?: (string | RegExp)[];
    sourcemap?: boolean;
}

export function n3oPluginConfig(options: N3oPluginConfigOptions): UserConfig {
    const { entries, outDir, react = false, additionalExternals = [], sourcemap = true } = options;

    const external: (string | RegExp)[] = [/^@umbraco/];

    if (react) {
        external.push('react', 'react-dom', 'react-dom/client', 'react/jsx-runtime');
    }

    external.push(...additionalExternals);

    return defineConfig({
        ...(react ? { esbuild: { jsx: 'automatic' as const } } : {}),
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
