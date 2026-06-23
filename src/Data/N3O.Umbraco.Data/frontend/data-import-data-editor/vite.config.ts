import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Data.ImportDataEditor/import-data-editor': 'src/import-data-editor.ts',
    },
    outDir: 'dist',
    react: true,
    additionalExternals: ['@n3oltd/backoffice-core'],
});
