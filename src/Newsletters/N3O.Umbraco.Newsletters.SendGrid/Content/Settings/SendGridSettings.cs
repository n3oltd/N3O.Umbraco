using N3O.Umbraco.Content;

namespace N3O.Umbraco.Newsletters.SendGrid.Content;

public class SendGridSettingsContent : UmbracoContent<SendGridSettingsContent> {
    public string ApiKey => GetValue(x => x.ApiKey);
    public string ListId => GetValue(x => x.ListId);
}