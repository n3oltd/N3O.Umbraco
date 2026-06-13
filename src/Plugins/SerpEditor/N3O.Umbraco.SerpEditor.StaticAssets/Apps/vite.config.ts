import { n3oPluginConfig } from '../../../../build/vite-config';

// Builds the SERP editor as a web-component shell that mounts a React app, into the shipped
// App_Plugins folder. @umbraco-cms/* AND react/react-dom are kept external — resolved at runtime
// by Umbraco's import map (react/react-dom are the self-hosted shared runtime from
// N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.ReactRuntime). Only this plugin's own code is bundled.
export default n3oPluginConfig({
    entries: { 'serp-editor': 'src/serp-editor.ts' },
    outDir: '../wwwroot/App_Plugins/N3O.Umbraco.SerpEditor',
    react: true,
});
