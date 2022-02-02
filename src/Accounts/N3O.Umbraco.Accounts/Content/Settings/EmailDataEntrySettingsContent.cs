using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Accounts.Content {
    public class EmailDataEntrySettingsContent : UmbracoContent<EmailDataEntrySettingsContent> {
        public bool Required => GetValue(x => x.Required);
        public string Label => GetValue(x => x.Label);
        public string HelpText => GetValue(x => x.HelpText);
        public bool Validate => GetValue(x => x.Validate);

        public EmailDataEntrySettings ToDataEntrySettings() {
            return new EmailDataEntrySettings(Required, Label, HelpText, Validate);
        }
    }
}
