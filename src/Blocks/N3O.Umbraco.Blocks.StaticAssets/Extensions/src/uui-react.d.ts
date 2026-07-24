import 'react';

declare module 'react' {
    namespace JSX {
        interface IntrinsicElements {
            'uui-loader': any;
        }
    }
}
