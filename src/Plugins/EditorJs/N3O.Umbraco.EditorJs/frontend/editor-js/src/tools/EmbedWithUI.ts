// Block tool that extends the @editorjs/embed tool with a custom UI for entering a URL before
// any service has been detected. Embed is declared `any` in vendor.d.ts (it ships no .d.ts), so
// extending it is safe at runtime via the widened base type.

import Embed from '@editorjs/embed';

// ---- small DOM helpers (duplicated from UmbracoImageTool to keep each tool file self-contained) ----

class RenderHelper {
    static createLabel(id: string, cssClass: string, text: string): HTMLLabelElement {
        const label = document.createElement('label');
        label.innerHTML = text;
        label.classList.add(cssClass);
        label.setAttribute('for', id);
        return label;
    }

    static createInput(id: string, value: string, text: string, type: string): HTMLInputElement {
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
}

// Embed is `any` (declared in vendor.d.ts) so extending it is safe at runtime.
// The base type is widened to `any` so that super.render() and super.* are accessible.
// eslint-disable-next-line @typescript-eslint/no-unsafe-call, @typescript-eslint/no-explicit-any
export class EmbedWithUI extends (Embed as new (...args: any[]) => any) {
    static get toolbox(): object {
        return {
            title: 'Video',
            icon: '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-youtube w-6 h-6 mx-1"><path d="M2.5 17a24.12 24.12 0 0 1 0-10 2 2 0 0 1 1.4-1.4 49.56 49.56 0 0 1 16.2 0A2 2 0 0 1 21.5 7a24.12 24.12 0 0 1 0 10 2 2 0 0 1-1.4 1.4 49.55 49.55 0 0 1-16.2 0A2 2 0 0 1 2.5 17"></path><path d="m10 15 5-3-5-3z"></path></svg>',
        };
    }

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    render(): HTMLElement {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        if (!(this as any).data?.service) {
            const container = document.createElement('div');
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            (this as any).element = container;

            const label = RenderHelper.createLabel(
                'embed-input',
                'cdx-label',
                'Enter a URL to embed a video from YouTube or Vimeo'
            );
            container.appendChild(label);

            const input = RenderHelper.createInput('embed-input', '', '', 'url');
            input.addEventListener('paste', (event: ClipboardEvent) => {
                const url = event.clipboardData?.getData('text') ?? '';
                // eslint-disable-next-line @typescript-eslint/no-explicit-any
                const EmbedClass = Embed as any;
                const service = Object.keys(EmbedClass.services as Record<string, { regex: RegExp }>).find(
                    (key) => (EmbedClass.services[key] as { regex: RegExp }).regex.test(url)
                );
                if (service) {
                    // eslint-disable-next-line @typescript-eslint/no-explicit-any
                    (this as any).onPaste({ detail: { key: service, data: url } });
                }
            });
            container.appendChild(input);

            return container;
        }
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        return (super.render as () => HTMLElement).call(this);
    }

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    validate(savedData: any): boolean {
        return savedData.service && savedData.source ? true : false;
    }
}
