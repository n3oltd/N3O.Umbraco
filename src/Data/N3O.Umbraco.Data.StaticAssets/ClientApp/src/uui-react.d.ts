// Minimal JSX typings so Umbraco UI Library (uui-*) web components can be used inside React TSX.
// `any` is intentional — UUI ships its own (Lit) types, not React JSX types; add tags as needed.
import 'react';

declare module 'react' {
    namespace JSX {
        interface IntrinsicElements {
            'uui-box': any;
            'uui-button': any;
            'uui-input': any;
            'uui-textarea': any;
            'uui-label': any;
            'uui-icon': any;
            'uui-loader': any;
            'uui-load-indicator': any;
            'uui-button-group': any;
            'uui-form-layout-item': any;
            'umb-property-layout': any;
        }
    }
}
