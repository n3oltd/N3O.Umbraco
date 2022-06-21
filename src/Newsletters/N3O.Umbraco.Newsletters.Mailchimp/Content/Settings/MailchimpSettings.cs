using N3O.Umbraco.Content;

namespace N3O.Umbraco.Newsletters.Mailchimp.Content;

public class MailchimpSettingsContent : UmbracoContent<MailchimpSettingsContent> {
    public string ApiKey => GetValue(x => x.ApiKey);
    public string AudienceId => GetValue(x => x.AudienceId);
}
