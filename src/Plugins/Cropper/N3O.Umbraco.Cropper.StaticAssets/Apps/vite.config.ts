import { n3oPluginConfig } from '../../../../build/vite-config';

// Builds the Cropper Lit components into the shipped App_Plugins folder.
// Two entries: primary property editor UI (cropper) and secondary config UI (crop-definitions).
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map).
// cropperjs is bundled. Formstone/jQuery remain as vendored runtime scripts (pending product decision).
export default n3oPluginConfig({
    entries: {
        'N3O.Umbraco.Cropper/cropper': 'src/cropper.ts',
        'N3O.Umbraco.Cropper/crop-definitions': 'src/crop-definitions.ts',
    },
    outDir: '../wwwroot/App_Plugins',
});
