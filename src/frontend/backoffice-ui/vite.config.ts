import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        index: 'src/index.tsx',
    },
    outDir: 'dist',
    react: true,
});
