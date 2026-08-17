import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        // Shell: the property-editor custom element. Uses @umbraco (external); hosts the iframe.
        'N3O.Umbraco.EditorJs/editor-js': 'src/editor-js.ts',
        // Frame: runs inside the iframe. EditorJS + tools + CSS, fully bundled (no @umbraco / React).
        'N3O.Umbraco.EditorJs/editor-js-frame': 'src/editor-js-frame.ts',
    },
    outDir: 'dist',
    // No React: the shell is a plain custom element and the frame is imperative EditorJS.
    react: false,
});
