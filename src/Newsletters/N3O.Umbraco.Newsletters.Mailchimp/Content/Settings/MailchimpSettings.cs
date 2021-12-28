using N3O.Umbraco.Content;

namespace N3O.Umbraco.Newsletters.Mailchimp.Content;

public class MailchimpSettings : UmbracoContent {
    public string ApiKey => GetValue<MailchimpSettings, string>(x => x.ApiKey);
    public string AudienceId => GetValue<MailchimpSettings, string>(x => x.AudienceId);
}
