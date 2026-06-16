// Minimal JSX typings so Umbraco UI Library (uui-*) web components can be used inside React TSX.
// `any` is intentional — UUI ships its own (Lit) types, not React JSX types; add tags as needed.
// Only display-only elements are declared here: interactive uui controls (uui-button, uui-input,
// uui-textarea, uui-toggle, uui-select, uui-button-group, uui-form-layout-item) break when rendered
// by React in Umbraco 17 — use native HTML controls inside uui-box/umb-property-layout instead.
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
