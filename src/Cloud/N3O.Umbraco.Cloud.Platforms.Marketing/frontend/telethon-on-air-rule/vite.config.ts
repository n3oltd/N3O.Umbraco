import { n3oPluginConfig } from '@repo/build-config';

// Builds the Telethon On Air Engage segment-rule UI as a Lit bundle (editor + display elements). The
// single entry imports both element modules so they self-register. @umbraco-cms/* is kept external and
// resolved at runtime by Umbraco's import map. Vite emits to dist/; the root Directory.Build.targets
// BuildFrontend step copies dist/ into the domain project's wwwroot/App_Plugins.
export default n3oPluginConfig({
    entries: {
        'telethon-on-air-rule/segment-rule-telethon-on-air': 'src/segment-rule-telethon-on-air.ts',
    },
    outDir: 'dist',
});
