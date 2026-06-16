// Single shared ambient type for the `?inline` CSS import used across the backoffice client apps.
// Vite resolves a `?inline` CSS import to the file's raw CSS string (default export); this version of
// vite/client types `*.css` as an empty module and doesn't cover the `?inline` query, so without this
// `import styles from './x.css?inline'` would have no type under tsc.
//
// Wired into every app via @n3o/build/base.json "files" (rides through `extends`). Do NOT duplicate
// this in per-app src/vite-env.d.ts files.
declare module '*.css?inline' {
    const css: string;
    export default css;
}
