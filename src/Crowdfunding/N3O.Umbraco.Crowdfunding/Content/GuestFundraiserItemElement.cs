using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Plugins.Models;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.GuestFundraiserItemElement.Alias)]
public class GuestFundraiserItemElement : UmbracoElement<GuestFundraiserItemElement> {
    public UploadedFile Icon => GetValue(x => x.Icon);
    public string Title => GetValue(x => x.Title);
    public string Text => GetValue(x => x.Text);
}