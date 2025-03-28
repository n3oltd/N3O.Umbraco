import { Plugin, RootElement, Element, Writer } from 'ckeditor5';


/*
  pass characterLimit as a config option to the CKEditor component
  example: {
    characterLimit: {
        limit: 1000
    }
  }
*/

export class CharacterLimit extends Plugin {
    private limit: number;
    private isProcessing: boolean = false;

    constructor(editor) {
        super(editor);

        editor.config.define('characterLimit', {
            limit: Number.POSITIVE_INFINITY,
        });

        this.limit = editor.config.get('characterLimit.limit') as number;
    }

    public init(): void {
        this.editor.model.schema.extend('$text', { allowAttributes: 'characterLimit' });

        this.addBeforeInputListener();
        this.addPasteListener();
        this.addChangeListener();
    }

    private addBeforeInputListener(): void {
        this.editor.editing.view.document.on('beforeinput', (evt, data) => {
            if (this.isProcessing) {
                evt.stop();
                return;
            }

            const currentLength = this.getContentLength();

            if (data.inputType.includes('delete') || data.inputType.includes('backward')) {
                return;
            }

            if (data.inputType === 'insertFromPaste' && data.dataTransfer) {
                const pasteText = data.dataTransfer.getData('text/plain');
                const availableSpace = this.limit - currentLength;

                if (availableSpace <= 0) {
                    evt.stop();
                } else if (pasteText.length > availableSpace) {
                    data.dataTransfer.setData('text/plain', pasteText.substring(0, availableSpace));
                }
                return;
            }

            if (currentLength >= this.limit) {
                evt.stop();
            }
        }, { priority: 'high' });
    }

    private addPasteListener(): void {
        this.editor.editing.view.document.on('paste', (evt, data) => {
            const pasteText = data.dataTransfer.getData('text/plain');
            const availableSpace = this.limit - this.getContentLength();

            if (availableSpace <= 0) {
                evt.stop();
            } else if (pasteText.length > availableSpace) {
                data.dataTransfer.setData('text/plain', pasteText.substring(0, availableSpace));
            }
        }, { priority: 'high' });
    }

    private addChangeListener(): void {
        this.editor.model.document.on('change:data', () => {
            if (this.isProcessing) return;

            const length = this.getContentLength();
            if (length > this.limit) {
                this.isProcessing = true;
                this.trimContentToLimit();
                this.isProcessing = false;
            }
        }, { priority: 'high' });
    }

    private getContentLength(): number {
        const root = this.editor.model.document.getRoot();
        return this.calculateNodeLength(root);
    }

    private calculateNodeLength(node: RootElement | Element | null): number {
        let length = 0;

        if (!node) {
            return length;
        }

        for (const child of node.getChildren()) {
            if (child.is('$text')) {
                length += child.data.length;
            } else if (child.is('element')) {
                length += this.calculateNodeLength(child);
            }
        }

        return length;
    }

    private trimContentToLimit(): void {
        const model = this.editor.model;
        const root = model.document.getRoot();

        if (!root) {
            return;
        }

        model.change((writer: Writer) => {
            this.handleWriter(writer, root, 0);
        });
    }

    private handleWriter(writer: Writer, root: RootElement | Element, currentLength: number) {
        for (const child of root.getChildren()) {
            if (currentLength >= this.limit) {
                break;
            }

            if (child.is('$text')) {
                const textLength = child.data.length;

                if (currentLength + textLength > this.limit) {
                    const allowedLength = this.limit - currentLength;
                    const trimmedText = child.data.substring(0, allowedLength);

                    const range = writer.createRangeOn(child);
                    writer.remove(range);

                    const position = writer.createPositionAt(root, range.start.offset);
                    writer.insertText(trimmedText, child.getAttributes(), position);

                    currentLength += allowedLength;
                    break;
                } else {
                    currentLength += textLength;
                }
            } else if (child.is('element')) {
                currentLength = this.handleWriter(writer, child, currentLength);
            }
        }

        return currentLength;
    }


}
