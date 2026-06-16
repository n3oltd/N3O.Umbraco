// Minimal JSX typings so Umbraco UI Library (uui-*) web components can be used inside React TSX.
// `any` is intentional — UUI ships its own (Lit) types, not React JSX types; add tags as needed.
// NOTE: interactive uui controls (uui-button, uui-input, uui-textarea, uui-button-group,
// uui-form-layout-item, uui-toggle, uui-select) are NOT declared here because they break when
// rendered by React in Umbraco 17 (elements don't mount + console errors). Use native HTML
// controls inside uui-box/umb-property-layout instead.
import 'react';

declare module 'react' {
    namespace JSX {
        interface IntrinsicElements {
            // display-only uui elements — safe to render from React
            'uui-box': any;
            'uui-label': any;
            'uui-icon': any;
            'uui-loader': any;
            'uui-load-indicator': any;
        }
    }
}
