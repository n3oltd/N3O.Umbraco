import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Cloud.Platforms.Preview/platforms-preview': 'src/platforms-preview.ts',
    },
    outDir: 'dist',
    additionalExternals: ['@n3oltd/backoffice-core'],
});
