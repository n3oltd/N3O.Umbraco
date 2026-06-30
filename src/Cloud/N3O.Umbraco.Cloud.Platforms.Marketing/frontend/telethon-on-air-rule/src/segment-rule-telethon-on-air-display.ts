import { LitElement, css, customElement, html, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

interface SegmentRule {
    name?: string;
    type?: string;
}

const elementName = 'segment-rule-telethon-on-air-display';

// Telethon On Air segment rule DISPLAY component. Loaded by the bundle in umbraco-package.json; renders
// the thin wrapper shown in the cockpit. No config fields (empty defaultConfig {}).
@customElement(elementName)
export class SegmentRuleTelethonOnAirDisplayElement extends UmbElementMixin(LitElement) {
    // Mirrors the AngularJS bindings: rule: "<", config: "<".
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
