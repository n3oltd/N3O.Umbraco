import { LitElement, css, customElement, html, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

interface SegmentRule {
    name?: string;
    type?: string;
}

const elementName = 'segment-rule-telethon-on-air-display';

@customElement(elementName)
export class SegmentRuleTelethonOnAirDisplayElement extends UmbElementMixin(LitElement) {
    @property({ attribute: false }) rule?: SegmentRule;
    @property({ attribute: false }) config?: Record<string, unknown>;

    override render() {
        return html`<span class="ums-segmentrule__wrapper ums-segmentrule__wrapper--thin"></span>`;
    }

    static override styles = css`
        :host {
            display: inline;
        }
    `;
}

export default SegmentRuleTelethonOnAirDisplayElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: SegmentRuleTelethonOnAirDisplayElement;
    }
}
