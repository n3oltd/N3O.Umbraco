import { n3oPluginConfig } from '../../../../build/vite-config';

// Builds the EditorJS editor as a web-component shell that mounts a React app, into the shipped
// App_Plugins folder. @umbraco-cms/* AND react/react-dom are kept external — resolved at runtime
// by Umbraco's import map (react/react-dom are the self-hosted shared runtime from
// N3O.Umbraco.Cms App_Plugins/N3O.Umbraco.ReactRuntime). The component's own code and all
// @editorjs/* / editorjs-* third-party libs are bundled (they are NOT React).
export default n3oPluginConfig({
    entries: { 'editor-js': 'src/editor-js.ts' },
    outDir: '../wwwroot/App_Plugins/N3O.Umbraco.EditorJs',
    react: true,
});
