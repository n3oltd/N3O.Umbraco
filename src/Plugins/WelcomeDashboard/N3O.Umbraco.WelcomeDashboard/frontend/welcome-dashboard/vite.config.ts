import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.WelcomeDashboard/welcome-dashboard': 'src/welcome-dashboard.ts',
        'N3O.Umbraco.WelcomeDashboard/entry-point': 'src/entry-point.ts',
    },
    outDir: 'dist',
    react: true,
});
