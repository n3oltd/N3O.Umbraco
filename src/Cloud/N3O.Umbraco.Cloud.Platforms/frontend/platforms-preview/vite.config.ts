import { n3oPluginConfig } from '@repo/build-config';

// Builds the Platforms Preview as a web-component shell that mounts a React app. @umbraco-cms/* AND
// react/react-dom are kept external — resolved at runtime by Umbraco's import map (react/react-dom are
// the self-hosted shared runtime published by N3O.Umbraco.ReactRuntime). Only this plugin's own code is
// bundled. Vite emits to dist/; the root Directory.Build.targets BuildFrontend step copies dist/ into
// the domain project's wwwroot/App_Plugins.
export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Cloud.Platforms.Preview/platforms-preview': 'src/platforms-preview.ts',
    },
    outDir: 'dist',
    react: true,
    additionalExternals: ['@n3oltd/backoffice-core'],
});
