import { n3oPluginConfig } from '@n3o/build';

export default n3oPluginConfig({
    entries: { 'block-preview': 'src/block-preview.ts' },
    outDir: '../wwwroot/App_Plugins/N3O.Umbraco.Blocks.Preview',
    react: true,
    additionalExternals: ['@n3o/backoffice-core'],
});
