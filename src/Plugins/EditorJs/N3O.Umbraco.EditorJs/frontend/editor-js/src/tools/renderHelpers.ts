// The `cdx-input` class is EditorJS's own, so inputs built here inherit the editor's field styling.

const safeLinkSchemes = ['http:', 'https:', 'mailto:', 'tel:'];

export function randomUUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        const r = (Math.random() * 16) | 0;
        const v = c === 'x' ? r : (r & 0x3) | 0x8;
        return v.toString(16);
    });
}

// The link picker does not constrain the scheme, and a stored href runs in the backoffice's own origin
// when an editor follows it.
export function isSafeUrl(url: string): boolean {
    const candidate = url.trim();

    if (!candidate) {
        return false;
    }

    if (candidate.startsWith('/') || candidate.startsWith('#') || candidate.startsWith('?')) {
        return true;
    }

    try {
        const scheme = new URL(candidate, window.location.href).protocol;

        return safeLinkSchemes.includes(scheme) || scheme === 'umb:';
    } catch {
        return false;
    }
}

export function createLabel(id: string, cssClass: string, text: string): HTMLLabelElement {
    const label = document.createElement('label');
    label.textContent = text;
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

    // EditorJS detects edits with a MutationObserver, which a typed value alone does not trigger.
    input.addEventListener('input', () => {
        input.setAttribute('value', input.value);
    });

    return input;
}
