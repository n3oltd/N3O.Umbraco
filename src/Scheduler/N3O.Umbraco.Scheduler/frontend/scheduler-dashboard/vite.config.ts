import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Scheduler/scheduler-dashboard': 'src/scheduler-dashboard.ts',
    },
    outDir: 'dist',
});
