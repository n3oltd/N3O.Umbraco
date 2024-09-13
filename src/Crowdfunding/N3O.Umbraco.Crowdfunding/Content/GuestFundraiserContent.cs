using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Content;

public class GuestFundraiserContent : UmbracoContent<GuestFundraiserContent> {
    public string Title => GetValue(x => x.Title);
    public string Text => GetValue(x => x.Text);
    public string Description => GetValue(x => x.Description);
    public string ButtonText => GetValue(x => x.ButtonText);
    public CroppedImage Logo => GetValue(x => x.Logo);
    public IEnumerable<GuestFundraiserItemElement> Items => GetNestedAs(x => x.Items);
}