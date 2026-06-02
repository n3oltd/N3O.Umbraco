import { LitElement, css, customElement, html, nothing, property, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import {
    UmbPropertyValueChangeEvent,
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';

const elementName = 'n3o-serp-editor';

interface SerpValue {
    title: string;
    description: string;
}

interface TemplateOptionsResponse {
    titleSuffix?: string;
}

// Google SERP preview property editor. Edits a { title, description } JSON value and renders a
// live preview of how the page would appear in Google search results. Ported from the AngularJS
// "N3O.Umbraco.SerpEditor" controller/view.
@customElement(elementName)
export class N3oSerpEditorElement extends UmbElementMixin(LitElement) implements UmbPropertyEditorUiElement {
    #value: SerpValue = { title: '', description: '' };
    #maxCharsTitle = 60;
    #maxCharsDescription = 160;

    @property({ type: Object })
    get value(): SerpValue {
        return this.#value;
    }
    set value(value: SerpValue | undefined) {
        const oldValue = this.#value;
        this.#value = value ?? { title: '', description: '' };
        this.requestUpdate('value', oldValue);
    }

    // Config (prevalues) arrives as UmbPropertyEditorConfigCollection.
    public set config(config: UmbPropertyEditorConfigCollection | undefined) {
        if (!config) {
            return;
        }

        const maxCharsTitle = Number.parseInt(config.getValueByAlias('maxCharsTitle') ?? '', 10);
        const maxCharsDescription = Number.parseInt(config.getValueByAlias('maxCharsDescription') ?? '', 10);

        if (!Number.isNaN(maxCharsTitle) && maxCharsTitle > 0) {
            this.#maxCharsTitle = maxCharsTitle;
        }

        if (!Number.isNaN(maxCharsDescription) && maxCharsDescription > 0) {
            this.#maxCharsDescription = maxCharsDescription;
        }
    }

    @state()
    private _titleSuffix = '';

    override async connectedCallback(): Promise<void> {
        super.connectedCallback();

        try {
            const response = await fetch('/umbraco/backoffice/api/serpEditor/templateOptions');

            if (response.ok) {
                const data = (await response.json()) as TemplateOptionsResponse;
                this._titleSuffix = data.titleSuffix ?? '';
            }
        } catch {
            // ignore - preview suffix is non-essential
        }
    }

    #onTitleInput(event: Event): void {
        this.#updateModel({ title: (event.target as HTMLInputElement).value });
    }

    #onDescriptionInput(event: Event): void {
        this.#updateModel({ description: (event.target as HTMLTextAreaElement).value });
    }

    #updateModel(partial: Partial<SerpValue>): void {
        this.#value = {
            title: this.#value?.title ?? '',
            description: this.#value?.description ?? '',
            ...partial,
        };
        this.requestUpdate();
        this.dispatchEvent(new UmbPropertyValueChangeEvent());
    }

    #getUrl(): string {
        return `${location.protocol}//${window.location.hostname}`;
    }

    override render() {
        const title = this.#value?.title ?? '';
        const description = this.#value?.description ?? '';

        return html`
            <div class="sv-form">
                <input
                    type="text"
                    .value=${title}
                    placeholder="Enter a short but descriptive title"
                    @input=${this.#onTitleInput} />
                ${title.length > this.#maxCharsTitle
                    ? html`<p class="sv-error">A title should not be more than ${this.#maxCharsTitle} characters.</p>`
                    : nothing}
                <br /><br />
                <textarea
                    .value=${description}
                    placeholder="Enter a meta description"
                    @input=${this.#onDescriptionInput}></textarea>
                ${description.length > this.#maxCharsDescription
                    ? html`<p class="sv-error">
                          A meta description should not be more than ${this.#maxCharsDescription} chars.
                      </p>`
                    : nothing}
            </div>

            <div class="sv-demo">
                ${title.length > 0 ? html`<h6>${title} ${this._titleSuffix}</h6>` : nothing}
                ${title.length > 0 || description.length > 0
                    ? html`<p class="sv-url">${this.#getUrl()}</p>`
                    : nothing}
                <p>${description}</p>
            </div>

            <div style="clear: both"></div>
        `;
    }

    static override styles = css`
        /* containers */
        .sv-form {
            width: 30%;
            float: left;
            margin-right: 40px;
        }

        .sv-demo {
            width: 600px; /* The width of the desktop-SERP as of 2019-11-14 */
            float: left;
        }

        /* form elements */
        .sv-form input,
        .sv-form textarea {
            width: 100%;
        }

        .sv-form textarea {
            height: 100px;
        }

        /* general text formating */
        .sv-demo h6,
        .sv-demo p {
            font-family: Arial, Helvectiva, san-serif;
            padding: 0;
            margin: 0;
        }

        /* form text formating */
        .sv-form p.sv-error {
            color: red;
            margin-top: 3px;
        }

        /* demo-mode text formating */
        .sv-demo h6 {
            font-size: 20px;
            line-height: 1.3;
            margin-bottom: 3px;
            color: blue;
            text-decoration: underline;
        }

        .sv-demo p {
            font-size: 14px;
            margin-bottom: 3px;
            line-height: 1.57;
            word-wrap: break-word;
        }

        .sv-demo p.sv-url {
            color: #00802a;
        }
    `;
}

export default N3oSerpEditorElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oSerpEditorElement;
    }
}
