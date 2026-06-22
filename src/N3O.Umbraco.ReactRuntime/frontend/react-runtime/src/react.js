// React, bundled once and re-exported as a single shared ESM module for the backoffice import map.
// NOTE: `export * from 'react'` does NOT carry React's CommonJS named exports through Vite's lib
// build — only `default` survives — so the public API is re-exported explicitly off the default
// object (this mirrors the react-dom shim's explicit re-exports). Keep in sync with the React major.
import React from 'react';

export default React;

// React 19's internal handshake object. react-dom resolves `react` via the import map and reads this
// off the default export, but export it explicitly too so any named/internal import resolves and the
// whole backoffice shares one React instance. Keep in sync with the React major.
export const __CLIENT_INTERNALS_DO_NOT_USE_OR_WARN_USERS_THEY_CANNOT_UPGRADE =
    React.__CLIENT_INTERNALS_DO_NOT_USE_OR_WARN_USERS_THEY_CANNOT_UPGRADE;

export const {
    Children,
    Component,
    Fragment,
    Profiler,
    PureComponent,
    StrictMode,
    Suspense,
    cloneElement,
    createContext,
    createElement,
    createRef,
    forwardRef,
    isValidElement,
    lazy,
    memo,
    startTransition,
    use,
    useActionState,
    useCallback,
    useContext,
    useDebugValue,
    useDeferredValue,
    useEffect,
    useId,
    useImperativeHandle,
    useInsertionEffect,
    useLayoutEffect,
    useMemo,
    useOptimistic,
    useReducer,
    useRef,
    useState,
    useSyncExternalStore,
    useTransition,
    version,
} = React;
