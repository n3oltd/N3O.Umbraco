import { LitElement, css, customElement, html, nothing, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import {
    UmbPropertyValueChangeEvent,
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';

const elementName = 'n3o-text-resource-editor';

interface TextResourceEntry {
    source: string;
    custom: string | null | undefined;
}

// Property editor UI for editing text resource overrides. The value is an array of
// { source, custom } entries. Each entry is rendered with a delete button, the (read-only)
// source text and a text input bound to `custom`. Mutations dispatch UmbPropertyValueChangeEvent.
@customElement(elementName)
export class N3oTextResourceEditorElement extends UmbElementMixin(LitElement) implements UmbPropertyEditorUiElement {
    #value: TextResourceEntry[] = [];

    @property({ type: Array })
    get value(): TextResourceEntry[] {
        return this.#value;
    }
    set value(value: TextResourceEntry[] | undefined) {
        const oldValue = this.#value;
        this.#value = Array.isArray(value) ? value : [];
        this.requestUpdate('value', oldValue);
    }

    // config is set by Umbraco; not used by this editor but accepted to satisfy the contract.
    public set config(_config: UmbPropertyEditorConfigCollection | undefined) {}

    #deleteEntry(index: number): void {
        if (!confirm('Are you sure you wish to delete this entry?')) {
            return;
        }

        this.#value = this.#value.filter((_, i) => i !== index);
        this.#dispatchChange();
    }

    #onInput(index: number, event: Event): void {
        const custom = (event.target as HTMLInputElement).value;

        this.#value = this.#value.map((entry, i) => (i === index ? { ...entry, custom } : entry));
        this.#dispatchChange();
    }

    #dispatchChange(): void {
        this.requestUpdate();
        this.dispatchEvent(new UmbPropertyValueChangeEvent());
    }

    override render() {
        if (!this.#value.length) {
            return nothing;
        }

        return html`
            <div class="n3o-text-resource-editor">
                ${this.#value.map(
                    (entry, index) => html`
                        <div class="row-wrapper">
                            <div class="row-1">
                                [<a @click=${() => this.#deleteEntry(index)} style="cursor: pointer;">x</a>]
                                <span class="text">${entry.source}</span>
                            </div>
                            <div class="row-2">
                                <input
                                    type="text"
                                    class="custom"
                                    .value=${entry.custom ?? ''}
                                    @input=${(e: Event) => this.#onInput(index, e)} />
                            </div>
                        </div>
                    `
                )}
            </div>
        `;
    }

    static override styles = css`
        .n3o-text-resource-editor .row-wrapper {
            margin-bottom: 40px;
            width: 100%;
        }

        .n3o-text-resource-editor .row-1 {
            display: block;
            width: 90%;
        }

        .n3o-text-resource-editor .row-2 {
            display: block;
            width: 90%;
        }

        .n3o-text-resource-editor .text {
            font-weight: bold;
        }

        .n3o-text-resource-editor .custom {
            width: 100%;
            margin-top: 10px;
        }
    `;
}

export default N3oTextResourceEditorElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oTextResourceEditorElement;
    }
}
