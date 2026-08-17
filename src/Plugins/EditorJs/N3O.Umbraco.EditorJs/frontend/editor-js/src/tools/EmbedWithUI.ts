import Embed from '@editorjs/embed';

import { createInput, createLabel, randomUUID } from './renderHelpers';

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

            const inputId = randomUUID();

            const label = createLabel(
                inputId,
                'cdx-label',
                'Enter a URL to embed a video from YouTube or Vimeo'
            );
            container.appendChild(label);

            const input = createInput(inputId, '', '', 'url');

            const applyUrl = (url: string): void => {
                // eslint-disable-next-line @typescript-eslint/no-explicit-any
                const EmbedClass = Embed as any;
                const service = Object.keys(EmbedClass.services as Record<string, { regex: RegExp }>).find(
                    (key) => (EmbedClass.services[key] as { regex: RegExp }).regex.test(url)
                );
                if (service) {
                    // eslint-disable-next-line @typescript-eslint/no-explicit-any
                    (this as any).onPaste({ detail: { key: service, data: url } });
                }
            };

            input.addEventListener('paste', (event: ClipboardEvent) => {
                applyUrl(event.clipboardData?.getData('text') ?? '');
            });
            input.addEventListener('change', () => {
                applyUrl(input.value);
            });
            input.addEventListener('keydown', (event: KeyboardEvent) => {
                if (event.key === 'Enter') {
                    event.preventDefault();
                    applyUrl(input.value);
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
