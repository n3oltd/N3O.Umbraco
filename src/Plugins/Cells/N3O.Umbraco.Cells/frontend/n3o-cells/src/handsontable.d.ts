// handsontable 12.x has no "types" condition in its package.json "exports", so tsc
// (moduleResolution: bundler, from the shared @repo/build-config preset) cannot resolve its shipped
// typings for the bare `handsontable` import and reports TS7016. Re-point the module at the shipped
// Core typings (base) so we keep real types without changing the shared preset for one third-party lib.
declare module 'handsontable' {
    import Core from 'handsontable/base';
    export default Core;
}
