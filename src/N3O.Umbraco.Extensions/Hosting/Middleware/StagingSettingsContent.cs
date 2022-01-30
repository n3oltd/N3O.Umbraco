using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Hosting {
    public class StagingSettingsContent : UmbracoContent<StagingSettingsContent> {
        public string Username => GetValue(x => x.Username);
        public string Password => GetValue(x => x.Password);
        public IEnumerable<FirewallRuleElement> Rules => GetNestedAs(x => x.Rules);
    }
}