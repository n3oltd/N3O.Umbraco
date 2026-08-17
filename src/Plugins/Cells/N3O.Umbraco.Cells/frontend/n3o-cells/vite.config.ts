import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Cells/n3o-cells': 'src/n3o-cells.ts',
    },
    outDir: 'dist',
    react: true,
});
