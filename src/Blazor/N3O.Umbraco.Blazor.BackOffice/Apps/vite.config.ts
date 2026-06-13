import { n3oPluginConfig } from '../../../build/vite-config';

// Builds the Blazor BackOffice loader script into the shipped App_Plugins folder.
// This is a non-Lit bundle/script entry — it is a plain loader, not a custom element.
// @umbraco-cms/* imports are kept external (resolved at runtime by Umbraco's import map);
// the loader's own code is bundled.
export default n3oPluginConfig({
    entries: { 'N3O.Umbraco.Blazor.BackOffice': 'src/N3O.Umbraco.Blazor.BackOffice.ts' },
    outDir: '../App_Plugins/N3O.Umbraco.Blazor.BackOffice',
});
