// Minimal JSX typings so Umbraco UI Library (uui-*) web components can be used inside React TSX.
// `any` is intentional — UUI ships its own (Lit) types, not React JSX types; add tags as needed.
import 'react';

declare module 'react' {
    namespace JSX {
        interface IntrinsicElements {
            'uui-loader': any;
        }
    }
}
