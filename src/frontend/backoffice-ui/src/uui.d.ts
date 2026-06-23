import 'react';

// JSX typings for the UUI tags these wrappers render. They are kept loose (`any`): the wrappers are the
// typed public surface, and consumers see only that surface (via the emitted .d.ts), never these tags.
declare module 'react' {
    namespace JSX {
        interface IntrinsicElements {
            'uui-button': any;
            'uui-select': any;
            'uui-toggle': any;
            'uui-checkbox': any;
            'uui-radio-group': any;
            'uui-radio': any;
        }
    }
}
