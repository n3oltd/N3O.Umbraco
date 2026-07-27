import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Blocks.Preview/block-preview': 'src/block-preview.ts',
    },
    outDir: 'dist',
    react: true,
    additionalExternals: ['@n3oltd/backoffice-core'],
});
