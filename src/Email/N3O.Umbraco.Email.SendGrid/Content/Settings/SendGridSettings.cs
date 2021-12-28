using N3O.Umbraco.Content;

namespace N3O.Umbraco.Email.SendGrid.Content;

public class SendGridSettings : UmbracoContent {
    public string ApiKey => GetValue<SendGridSettings, string>(x => x.ApiKey);
    public bool SandboxMode => GetValue<SendGridSettings, bool>(x => x.SandboxMode);
}
