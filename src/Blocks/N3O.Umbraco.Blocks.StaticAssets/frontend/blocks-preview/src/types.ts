export type PreviewState =
    | { status: 'loading' }
    | { status: 'ready'; markup: string }
    | { status: 'error'; message: string };
