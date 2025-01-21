using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Accounts.Content;

public class TermsDataEntrySettingsContent : UmbracoContent<TermsDataEntrySettingsContent> {
    public string Text => GetValue(x => x.Text);
    public Link Link => GetValue(x => x.Link);
}