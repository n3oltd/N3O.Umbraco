using N3O.Umbraco.Content;

namespace N3O.Umbraco.Hosting {
    public class FirewallRuleElement : UmbracoElement<FirewallRuleElement> {
        public string RuleIpAddress => GetValue(x => x.RuleIpAddress);
        public string RuleName => GetValue(x => x.RuleName);
    }
}