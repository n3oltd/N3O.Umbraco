using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Uploader.Extensions;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CampaignImagesElement : UmbracoElement<CampaignImagesElement> {
    public CroppedImage CampaignImage => this.GetCroppedImageAs(x => x.CampaignImage);
}
