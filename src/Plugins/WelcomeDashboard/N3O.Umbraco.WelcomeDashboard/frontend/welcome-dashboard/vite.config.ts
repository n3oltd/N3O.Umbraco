import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.WelcomeDashboard/welcome-dashboard': 'src/welcome-dashboard.ts',
    },
    outDir: 'dist',
    react: true,
});
