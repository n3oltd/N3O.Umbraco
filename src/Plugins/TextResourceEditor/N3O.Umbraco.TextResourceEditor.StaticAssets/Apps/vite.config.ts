import { n3oPluginConfig } from '../../../../build/vite-config';

// Builds the Text Resource Editor as a web-component shell that mounts a React app, into the shipped
// wwwroot/App_Plugins folder. @umbraco-cms/* AND react/react-dom are kept external — resolved at runtime
// by Umbraco's import map (react/react-dom are the self-hosted shared runtime from
// N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.ReactRuntime). Only this plugin's own code is bundled.
export default n3oPluginConfig({
    entries: { 'text-resource-editor': 'src/text-resource-editor.ts' },
    outDir: '../wwwroot/App_Plugins/N3O.Umbraco.TextResourceEditor',
    react: true,
});
