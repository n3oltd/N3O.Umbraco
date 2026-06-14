import { LitElement, html, css } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

// Telethon On Air segment rule DISPLAY component.
// Loaded by the bundle in umbraco-package.json; renders the thin wrapper shown
// in the cockpit. No config fields (empty defaultConfig {}).
const elementName = 'segment-rule-telethon-on-air-display';

class SegmentRuleTelethonOnAirDisplayElement extends UmbElementMixin(LitElement) {
    static properties = {
        // Mirrors the AngularJS bindings: rule: "<", config: "<"
        rule: { attribute: false },
        config: { attribute: false },
    };

    constructor() {
        super();
        this.rule = undefined;
        this.config = undefined;
    }

    render() {
        // Original markup: thin wrapper span (no config to surface).
        return html`<span class="ums-segmentrule__wrapper ums-segmentrule__wrapper--thin"></span>`;
    }

    static styles = css`
        :host {
            display: inline;
        }
    `;
}

customElements.define(elementName, SegmentRuleTelethonOnAirDisplayElement);

export default SegmentRuleTelethonOnAirDisplayElement;
export { SegmentRuleTelethonOnAirDisplayElement };
