// Minimal JSX typings so Umbraco UI Library (uui-*) web components can be used inside React TSX.
// `any` is intentional — UUI ships its own (Lit) types, not React JSX types; add tags as needed.
// Interactive uui controls (uui-button, uui-input, etc.) are intentionally omitted: they break when
// rendered by React in v17 — use native HTML controls inside uui-box/umb-property-layout instead.
import 'react';

declare module 'react' {
    namespace JSX {
        interface IntrinsicElements {
            'uui-box': any;
            'uui-label': any;
            'uui-icon': any;
            'uui-loader': any;
            'uui-load-indicator': any;
        }
    }
}
