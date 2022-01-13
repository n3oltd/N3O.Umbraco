using N3O.Umbraco.Content;

namespace N3O.Umbraco.Newsletters.Mailchimp.Content {
    public class MailchimpSettings : UmbracoContent<MailchimpSettings> {
        public string ApiKey => GetValue(x => x.ApiKey);
        public string AudienceId => GetValue(x => x.AudienceId);
    }
}
