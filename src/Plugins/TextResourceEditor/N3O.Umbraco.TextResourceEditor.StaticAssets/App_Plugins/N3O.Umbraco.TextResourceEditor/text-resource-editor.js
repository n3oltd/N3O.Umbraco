import { LitElement, html, css, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbPropertyValueChangeEvent } from '@umbraco-cms/backoffice/property-editor';

const elementName = 'n3o-text-resource-editor';

// Property editor UI for editing text resource overrides. The value is an array of
// { source, custom } entries. Each entry is rendered with a delete button, the (read-only)
// source text and a text input bound to `custom`. Mutations dispatch UmbPropertyValueChangeEvent.
class N3oTextResourceEditorElement extends UmbElementMixin(LitElement) {
    static properties = {
        value: { type: Array },
    };

    #value = [];

    get value() {
        return this.#value;
    }

    set value(value) {
        const oldValue = this.#value;
        this.#value = Array.isArray(value) ? value : [];
        this.requestUpdate('value', oldValue);
    }

    // config is set by Umbraco; not used by this editor but accepted to satisfy the contract.
    set config(_config) {}

    #deleteEntry(index) {
        if (!confirm('Are you sure you wish to delete this entry?')) {
            return;
        }

        this.#value = this.#value.filter((_, i) => i !== index);
        this.#dispatchChange();
    }

    #onInput(index, event) {
        const custom = event.target.value;

        this.#value = this.#value.map((entry, i) => (i === index ? { ...entry, custom } : entry));
        this.#dispatchChange();
    }

    #dispatchChange() {
        this.requestUpdate();
        this.dispatchEvent(new UmbPropertyValueChangeEvent());
    }

    render() {
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
                                    @input=${(e) => this.#onInput(index, e)} />
                            </div>
                        </div>
                    `
                )}
            </div>
        `;
    }

    static styles = css`
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

customElements.define(elementName, N3oTextResourceEditorElement);

export default N3oTextResourceEditorElement;
export { N3oTextResourceEditorElement };
