import { LitElement, css, customElement, html, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

interface SegmentRule {
    name?: string;
    type?: string;
}

const elementName = 'segment-rule-telethon-on-air-editor';

@customElement(elementName)
export class SegmentRuleTelethonOnAirEditorElement extends UmbElementMixin(LitElement) {
    @property({ attribute: false }) rule?: SegmentRule;
    @property({ attribute: false }) config?: Record<string, unknown>;
    @property({ attribute: false }) save?: () => void;

    #onSave(): void {
        this.save?.();
    }

    override render() {
        const name = this.rule?.name ?? 'Telethon On Air';
        const type = this.rule?.type ?? 'TelethonOnAir';

        return html`
            <div class="ums-segmentrule__editor">
                <uui-box headline=${name}>
                    <p class="type">${type}</p>
                    <uui-button look="primary" label="Save" @click=${this.#onSave}>Save</uui-button>
                </uui-box>
            </div>
        `;
    }

    static override styles = css`
        :host {
            display: block;
        }

        .type {
            color: var(--uui-color-text-alt);
            margin: 0 0 var(--uui-size-space-4) 0;
        }
    `;
}

export default SegmentRuleTelethonOnAirEditorElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: SegmentRuleTelethonOnAirEditorElement;
    }
}
