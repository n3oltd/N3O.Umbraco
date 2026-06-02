import { LitElement, html, css } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

// ----------------------------------------------------------------------------
// PLACEHOLDER (BLOCKER-04) — Telethon On Air segment rule EDITOR component.
//
// The original AngularJS editor component delegated entirely to Engage's own
// <ums-segment-rule-editor> directive:
//   <ums-segment-rule-editor name="$ctrl.rule.name" type="$ctrl.rule.type"
//                            save="$ctrl.save()"></ums-segment-rule-editor>
// i.e. the rule itself has no custom fields (empty defaultConfig {}); Engage
// supplies the generic editor chrome (name/type + save).
//
// The Engage v17 (Bellissima) client-side equivalent of <ums-segment-rule-editor>
// and of the segment-rule extension contract is UNKNOWN (see BLOCKER-04 + report).
// This placeholder preserves the same inputs the AngularJS component received
// (rule, config) and the same `save` callback, and surfaces the rule name/type
// so the rule is still identifiable in the UI. It must be re-pointed at the real
// Engage v17 editor host once that client API is confirmed.
// ----------------------------------------------------------------------------
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
