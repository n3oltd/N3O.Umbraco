using N3O.Umbraco.Content;

namespace N3O.Umbraco.Email.SendGrid.Content;

public class SendGridSettingsContent : UmbracoContent<SendGridSettingsContent> {
    public string ApiKey => GetValue(x => x.ApiKey);
    public bool SandboxMode => GetValue(x => x.SandboxMode);
}
