// React, bundled once and re-exported as a single shared ESM module for the backoffice import map.
// NOTE: `export * from 'react'` does NOT carry React's CommonJS named exports through Vite's lib
// build — only `default` survives — so the public API is re-exported explicitly off the default
// object (this mirrors the react-dom shim's explicit re-exports). Keep in sync with the React major.
import React from 'react';

export default React;

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
