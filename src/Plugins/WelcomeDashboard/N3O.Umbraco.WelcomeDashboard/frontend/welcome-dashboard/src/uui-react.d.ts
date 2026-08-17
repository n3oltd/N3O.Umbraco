// UUI ships Lit types, not React JSX types.
import 'react';

declare module 'react' {
    namespace JSX {
        interface IntrinsicElements {
            'uui-box': any;
        }
    }
}
