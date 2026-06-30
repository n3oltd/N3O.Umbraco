import { n3oPluginConfig } from '@repo/build-config';

// Lit workspaceInfoApp showing staging + production platform URLs in the document Info tab. The authed
// call goes through @n3oltd/backoffice-core (externalized, resolved at runtime). No React. Vite emits to
// dist/; the root Directory.Build.targets BuildFrontend step copies dist/ into wwwroot/App_Plugins.
export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Cloud.Platforms.Urls/platforms-urls-info-app': 'src/platforms-urls-info-app.ts',
    },
    outDir: 'dist',
    additionalExternals: ['@n3oltd/backoffice-core'],
});
