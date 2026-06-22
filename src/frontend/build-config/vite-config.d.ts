import type { UserConfig } from 'vite';

export interface N3oPluginConfigOptions {
    entries: Record<string, string>;
    outDir: string;
    react?: boolean;
    additionalExternals?: (string | RegExp)[];
    sourcemap?: boolean;
}

export function n3oPluginConfig(options: N3oPluginConfigOptions): UserConfig;
