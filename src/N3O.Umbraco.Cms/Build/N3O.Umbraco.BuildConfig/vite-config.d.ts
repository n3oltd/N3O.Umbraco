import type { UserConfig } from 'vite';

// Hand-authored declarations for the plain-ESM vite-config.js preset. IDE/IntelliSense only — every
// app's vite.config.ts is outside its tsconfig include (["src"]), so these never gate tsc --noEmit
// or the build. Keep in sync with vite-config.js.
export interface N3oPluginConfigOptions {
    /** Output name (may include a subfolder, e.g. 'N3O.Umbraco.Cropper/cropper') -> source entry file. */
    entries: Record<string, string>;
    outDir: string;
    /** Externalizes react/react-dom (shared runtime) and enables the automatic JSX runtime. */
    react?: boolean;
    /** Extra externals on top of the standard list (e.g. '@n3o/backoffice-core'). */
    additionalExternals?: (string | RegExp)[];
    sourcemap?: boolean;
}

export function n3oPluginConfig(options: N3oPluginConfigOptions): UserConfig;
