using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Accounts.Content;

public class TermsOfServicesSettingsContent : UmbracoContent<TermsOfServicesSettingsContent> {
    public string Text => GetValue(x => x.Text);
    public HtmlString Url => GetValue(x => x.Url);
}