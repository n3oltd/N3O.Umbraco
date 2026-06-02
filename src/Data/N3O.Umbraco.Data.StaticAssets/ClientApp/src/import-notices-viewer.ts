import { LitElement, css, customElement, html, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import {
    type UmbPropertyEditorConfigCollection,
    type UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';

const elementName = 'n3o-import-notices-viewer';

interface ImportNoticesValue {
    errors: string[];
    warnings: string[];
}

// Read-only property editor that displays the import notices (errors and warnings) stored on the value.
// The value is JSON of the shape { errors: string[], warnings: string[] }. Display only - no change event.
@customElement(elementName)
export class N3oImportNoticesViewerElement extends UmbElementMixin(LitElement) implements UmbPropertyEditorUiElement {
    #value: ImportNoticesValue | undefined = undefined;

    get value(): ImportNoticesValue | undefined {
        return this.#value;
    }

    set value(v: ImportNoticesValue | undefined) {
        const oldValue = this.#value;
        this.#value = v;
        this.requestUpdate('value', oldValue);
    }

    // Config is set by Umbraco for property editors; unused here but accepted to avoid warnings.
    public set config(_config: UmbPropertyEditorConfigCollection | undefined) { }

    public get config(): UmbPropertyEditorConfigCollection | undefined {
        return undefined;
    }

    get #errors(): string[] | null {
        return this.#value?.errors ?? null;
    }

    get #warnings(): string[] | null {
        return this.#value?.warnings ?? null;
    }

    override render() {
        const errors = this.#errors;
        const warnings = this.#warnings;

        return html`
            <div class="n3o-import-errors-viewer">
                ${errors && errors.length
                    ? html`
                          <p><em class="text-error">Errors</em></p>
                          ${errors.map(
                              (error) => html`
                                  <div class="row-wrapper">
                                      <div class="row">${error}</div>
                                  </div>
                              `
                          )}
                      `
                    : nothing}
                ${warnings && warnings.length
                    ? html`
                          <p><em class="text-warning">Warnings</em></p>
                          ${warnings.map(
                              (warning) => html`
                                  <div class="row-wrapper">
                                      <div class="row">${warning}</div>
                                  </div>
                              `
                          )}
                      `
                    : nothing}
                ${(!errors || !errors.length) && (!warnings || !warnings.length)
                    ? html`
                          <div class="row-wrapper">
                              <div class="row">No warnings or errors</div>
                          </div>
                      `
                    : nothing}
            </div>
        `;
    }

    static override styles = css`
        .n3o-import-errors-viewer .row-wrapper {
            margin-bottom: 40px;
            width: 100%;
        }

        .n3o-import-errors-viewer .row {
            display: block;
            width: 90%;
        }

        .text-error {
            color: var(--uui-color-danger);
        }

        .text-warning {
            color: var(--uui-color-warning);
        }
    `;
}

export default N3oImportNoticesViewerElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: N3oImportNoticesViewerElement;
    }
}
