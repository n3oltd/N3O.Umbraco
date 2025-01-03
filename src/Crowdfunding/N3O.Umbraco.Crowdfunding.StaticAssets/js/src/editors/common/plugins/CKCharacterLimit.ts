import {Plugin} from 'ckeditor5';


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

    constructor(editor: any) {
        super(editor);

        editor.config.define('characterLimit', {
            limit: Number.POSITIVE_INFINITY,
        });

        this.limit = editor.config.get('characterLimit.limit') as number;
    }

    public init(): void {
        const editor = this.editor;

        this.editor.model.schema.extend('$text', { allowAttributes: 'characterLimit' });

        editor.editing.view.document.on('beforeinput', (evt, data) => {
            const length = this.getContentLength();

            if (data.inputType.includes('delete') || data.inputType.includes('backward')) {
                return;
            }

            if (data.inputType === 'insertFromPaste' && data.dataTransfer) {
                const pasteText = data.dataTransfer.getData('text/plain');
                if (length + pasteText.length > this.limit) {
                    evt.stop();
                    return;
                }
            }

            else if (length >= this.limit) {
                evt.stop();
                return;
            }
        }, { priority: 'high' });

        editor.editing.view.document.on('paste', (evt, data) => {
            const length = this.getContentLength();
            const pasteText = data.dataTransfer.getData('text/plain');

            if (length + pasteText.length > this.limit) {
                evt.stop();
            }
        }, { priority: 'high' });

        editor.model.document.on('change', () => {
            const length = this.getContentLength();
            if (length > this.limit) {
                editor.model.change(() => {
                    const content = editor.getData();
                    const trimmedContent = this.trimToLimit(content);
                    editor.setData(trimmedContent);
                });
            }
        }, { priority: 'high' });

        editor.model.document.on('insertContent', (_, batch) => {
            this.enforceCharacterLimit(batch);
        }, { priority: 'high' });
    }


    private getContentLength(): number {
        const data = this.editor.getData();
        return data.replace(/<[^>]*>/g, '').length;
    }

    private enforceCharacterLimit(batch: any): void {
        const length = this.getContentLength();
        if (length > this.limit) {
            const editor = this.editor;

            editor.model.change(() => {
                const content = editor.getData();
                const trimmedContent = this.trimToLimit(content);

                batch.stop();

                editor.setData(trimmedContent);
            });
        }
    }

    private trimToLimit(content: string): string {
        const plainText = content.replace(/<[^>]*>/g, '');
        const trimmedText = plainText.substring(0, this.limit);
        return trimmedText;
    }
}
