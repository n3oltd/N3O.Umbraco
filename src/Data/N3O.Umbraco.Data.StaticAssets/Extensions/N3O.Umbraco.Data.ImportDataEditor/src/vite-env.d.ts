// Vite resolves a `?inline` CSS import to the file's raw CSS string (default export). Declared here
// because this version of vite/client types `*.css` as an empty module and doesn't cover the `?inline`
// query, so `import styles from './x.css?inline'` would otherwise have no type under `tsc`.
declare module '*.css?inline' {
    const css: string;
    export default css;
}
