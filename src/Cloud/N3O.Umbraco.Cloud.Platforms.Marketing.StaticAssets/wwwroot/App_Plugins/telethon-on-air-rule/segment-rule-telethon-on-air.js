// ----------------------------------------------------------------------------
// PLACEHOLDER (BLOCKER-04) — Telethon On Air segment rule REGISTRATION.
//
// The original AngularJS file registered this rule with Engage's segment-rule
// repository service:
//
//   angular.module("umbraco").run(["umsSegmentRuleRepository", function (ruleRepo) {
//       ruleRepo.addRule({
//           name: "Telethon On Air",
//           type: "TelethonOnAir",
//           iconUrl: "data:image/png;base64,...",
//           order: 4,
//           defaultConfig: {},
//           components: {
//               display: "segment-rule-telethon-on-air-display",
//               editor:  "segment-rule-telethon-on-air-editor",
//           },
//       });
//   });
//
// In Umbraco 17 (Bellissima) there is no `angular.module(...).run` and no
// `umsSegmentRuleRepository`. The Engage v17.2.2 *client-side* extension API for
// contributing a custom segment rule (the equivalent of `ruleRepo.addRule`) is
// UNKNOWN and could not be confirmed from the codebase or the migrated reference
// plugin. Per the migration guidance we are NOT inventing that API.
//
// The rule descriptor below is preserved verbatim (name, type, iconUrl, order,
// defaultConfig, and the editor/display element tag names) so it can be handed to
// the real Engage v17 registration mechanism once it is identified. The two Lit
// components it references already exist and are registered via umbraco-package.json:
//   - editor:  segment-rule-telethon-on-air-editor  (segment-rule-telethon-on-air-editor.js)
//   - display: segment-rule-telethon-on-air-display (segment-rule-telethon-on-air-display.js)
//
// ACTION REQUIRED (see report / BLOCKER-04):
//   Replace the placeholder below with the Engage v17 client-side segment-rule
//   registration call once the API is confirmed (likely an Umbraco extension
//   manifest type, an Engage extension-registry entry, or an Engage JS API on the
//   global Umbraco/Engage namespace).
// ----------------------------------------------------------------------------

// Ensure the editor + display custom elements are defined when this bundle loads,
// so they are ready for the Engage v17 registration once it is wired up.
import './segment-rule-telethon-on-air-editor.js';
import './segment-rule-telethon-on-air-display.js';

export const telethonOnAirSegmentRule = {
    name: 'Telethon On Air',
    type: 'TelethonOnAir',
    iconUrl:
        'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAA7QAAAO0Bq2+TWQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAHcSURBVFiF5Zexb9NQEMZ/XxTEEFQJwpSFzS1rGWFhInMUhPoXMGekfxBV1ZGpEwuMTJESFUUhYo7EQgaClOvge62b2o6duKYSn3TS+d073+d7d+89y8xIg6QG8NkfX5vZKnViBgr7m1mqABFgLlHWvF39Gzkf0cjQi6KQfxNAUgt4BzwHHrjtcWLeB0m/ShLI8v8LjIFTM1sAdIAJ1+mqSyZAR8BH4AhYAV+BhTN9BLxy/Qvwu2QGsvxbwEviZTkBmDujwVoRHSTYHmxRhJn+wMDH5w2g7czGJb9wF4RY7WbOpB/Az4ReFoX8MwmY2R9JUdDLRi/qn5eBrQKX9U8SeC+pu0vAEngWFBFX4z9DMgPfiFuyDjwFXoSH0Kvdsr2+rQDdEHebQ6ZSpBKQtCdpJMlcRpL2qrJvJAA8IT7PAyIfq8p+hdR9wMxmvomEl3w3s1lV9o0E/CVTYHpX9o0EJL0hvqAAjM3svEr7DbKstSGwz+0LxH6F9qs2zMrADPhEYg19rCp7fgb+q40odQkkPQTOuJnCt+F43dW+jvtXhGZ24XeDZBtdVGVfx9LZ9Goswp7HXDaBIXAIHEuC6/+Cu0ILOHZ9CNCn/r+iIP2Qkj7xjWhZQ9Clx+qbGZcKDfX8hx8/qgAAAABJRU5ErkJggg==',
    order: 4,
    defaultConfig: {},
    components: {
        display: 'segment-rule-telethon-on-air-display',
        editor: 'segment-rule-telethon-on-air-editor',
    },
};

export default telethonOnAirSegmentRule;
