import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.EditorJs/editor-js': 'src/editor-js.ts',
    },
    outDir: 'dist',
    react: true,
    additionalExternals: ['@n3oltd/backoffice-core'],
});
