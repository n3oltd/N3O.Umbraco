// Type stubs for editorjs packages that ship no (or broken) TypeScript declarations.
// skipLibCheck suppresses errors inside their own .d.ts files, but import resolution
// errors must be silenced with declare module.

declare module '@editorjs/raw' {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const RawTool: any;
    export default RawTool;
}

declare module '@editorjs/checklist' {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const Checklist: any;
    export default Checklist;
}

declare module '@editorjs/embed' {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const Embed: any;
    export default Embed;
}

declare module 'editorjs-drag-drop' {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const DragDrop: any;
    export default DragDrop;
}

declare module 'editor-js-alignment-tune' {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const AlignmentTune: any;
    export default AlignmentTune;
}
