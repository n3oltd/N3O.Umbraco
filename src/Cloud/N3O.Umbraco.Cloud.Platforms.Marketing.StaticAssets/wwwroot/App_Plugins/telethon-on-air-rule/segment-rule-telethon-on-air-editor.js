import { LitElement, html, css } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

// Telethon On Air segment rule EDITOR component.
// Registered via umbraco-package.json as type "engageSegmentRule" (elementName).
// The rule has no configurable fields (empty config {}), so this element only
// identifies the rule type; Engage v17 provides the generic editor chrome.
const elementName = 'segment-rule-telethon-on-air-editor';

class SegmentRuleTelethonOnAirEditorElement extends UmbElementMixin(LitElement) {
    static properties = {
        // Mirrors the AngularJS bindings: rule: "<", config: "<", save: "&"
        rule: { attribute: false },
        config: { attribute: false },
        save: { attribute: false },
    };

    constructor() {
        super();
        this.rule = undefined;
        this.config = undefined;
        this.save = undefined;
    }

    #onSave() {
        // Equivalent to the AngularJS save="$ctrl.save()" binding.
        if (typeof this.save === 'function') {
            this.save();
        }
    }

    render() {
        const name = this.rule?.name ?? 'Telethon On Air';
        const type = this.rule?.type ?? 'TelethonOnAir';

        // The Telethon On Air rule has no configurable fields (empty defaultConfig),
        // so there is no form to render — only the generic name/type + save, which
        // Engage previously provided via <ums-segment-rule-editor>.
        return html`
            <div class="ums-segmentrule__editor">
                <uui-box headline=${name}>
                    <p class="type">${type}</p>
                    <uui-button
                        look="primary"
                        label="Save"
                        @click=${this.#onSave}>
                        Save
                    </uui-button>
                </uui-box>
            </div>
        `;
    }

    static styles = css`
        :host {
            display: block;
        }

        .type {
            color: var(--uui-color-text-alt);
            margin: 0 0 var(--uui-size-space-4) 0;
        }
    `;
}

customElements.define(elementName, SegmentRuleTelethonOnAirEditorElement);

export default SegmentRuleTelethonOnAirEditorElement;
export { SegmentRuleTelethonOnAirEditorElement };
