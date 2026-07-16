import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Cloud.Platforms.Crowdfunding/crowdfunding-visibility': 'src/crowdfunding-visibility.context.ts',
    },
    outDir: 'dist',
});
