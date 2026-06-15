// JSX automatic-runtime entry. `export * from 'react/jsx-runtime'` yields nothing through Vite's
// lib build (CommonJS), so re-export jsx/jsxs/Fragment explicitly off the default object. `react`
// itself stays external (resolved via the import map), so this shares the single React instance.
import jsxRuntime from 'react/jsx-runtime';

export const jsx = jsxRuntime.jsx;
export const jsxs = jsxRuntime.jsxs;
export const Fragment = jsxRuntime.Fragment;
