import { LitElement, html, css } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

// ----------------------------------------------------------------------------
// PLACEHOLDER (BLOCKER-04) — Telethon On Air segment rule DISPLAY component.
//
// The original AngularJS display component was a near-empty wrapper:
//   <span class="ums-segmentrule__wrapper ums-segmentrule__wrapper--thin"></span>
// rendered by Engage's segment-rule cockpit UI. The "Telethon On Air" rule has
// an empty defaultConfig ({}) so there is nothing rule-specific to display.
//
// The Engage v17 (Bellissima) client-side segment-rule extension API that hosts
// this component is UNKNOWN (see BLOCKER-04 + report). This is a faithful, fields-
// preserving placeholder: it accepts the same `rule` / `config` inputs the
// AngularJS component bound and renders the same thin wrapper, but it is NOT yet
// wired into a verified Engage extension point.
// ----------------------------------------------------------------------------
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
