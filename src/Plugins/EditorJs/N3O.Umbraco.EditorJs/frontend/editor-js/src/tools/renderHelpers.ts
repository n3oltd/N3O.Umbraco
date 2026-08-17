// DOM helpers shared by the custom EditorJS tools. The `cdx-input` class is EditorJS's own, so inputs
// built here inherit the editor's field styling.

export function randomUUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        const r = (Math.random() * 16) | 0;
        const v = c === 'x' ? r : (r & 0x3) | 0x8;
        return v.toString(16);
    });
}

export function createLabel(id: string, cssClass: string, text: string): HTMLLabelElement {
    const label = document.createElement('label');
    label.innerHTML = text;
    label.classList.add(cssClass);
    label.setAttribute('for', id);
    return label;
}

export function createInput(id: string, value: string, text: string, type: string): HTMLInputElement {
    const input = document.createElement('input');
    input.setAttribute('type', type);
    if (value) {
        input.setAttribute('value', value);
    }
    if (text) {
        input.setAttribute('placeholder', text);
    }
    input.setAttribute('id', id);
    input.classList.add('cdx-input');
    return input;
}
