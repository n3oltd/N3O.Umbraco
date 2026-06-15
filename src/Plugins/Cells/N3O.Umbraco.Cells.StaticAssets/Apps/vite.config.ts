import { n3oPluginConfig } from '@n3o/build';

// Builds the Cells editor as a web-component shell that mounts a React app, into the shipped
// App_Plugins folder. @umbraco-cms/* AND react/react-dom are kept external — resolved at runtime
// by Umbraco's import map (react/react-dom are the self-hosted shared runtime from
// N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.ReactRuntime). Handsontable and this plugin's own code ARE
// bundled into the single output file (Handsontable is not React).
export default n3oPluginConfig({
    entries: { 'n3o-cells': 'src/n3o-cells.ts' },
    outDir: '../wwwroot/App_Plugins/N3O.Umbraco.Cells',
    react: true,
});
