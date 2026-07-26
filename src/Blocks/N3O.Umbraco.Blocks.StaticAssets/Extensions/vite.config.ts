import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: { 'block-preview': 'src/block-preview.ts' },
    outDir: '../wwwroot/App_Plugins/N3O.Umbraco.Blocks.Preview',
    react: true,
    additionalExternals: ['@n3oltd/backoffice-core'],
});
