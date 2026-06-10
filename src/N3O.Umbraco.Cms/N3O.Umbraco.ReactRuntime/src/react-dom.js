// react-dom (+ react-dom/client) re-exported as one shared ESM module for the import map.
// `export * from 'react-dom'` drops React's CommonJS named exports through Vite's lib build
// (only `default` survives), so the public API is re-exported explicitly off the default object —
// same fix as the react shim. `react` stays external so this shares the single React instance.
import ReactDOM from 'react-dom';

export default ReactDOM;

export const {
    createPortal,
    flushSync,
    preconnect,
    prefetchDNS,
    preinit,
    preinitModule,
    preload,
    preloadModule,
    requestFormReset,
    unstable_batchedUpdates,
    useFormState,
    useFormStatus,
    version,
} = ReactDOM;

export { createRoot, hydrateRoot } from 'react-dom/client';
