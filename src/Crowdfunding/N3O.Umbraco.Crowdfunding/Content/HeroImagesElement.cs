using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Uploader.Extensions;

namespace N3O.Umbraco.Crowdfunding.Content;

public class HeroImagesElement : UmbracoElement<HeroImagesElement> {
    public CroppedImage Image => this.GetCroppedImageAs(x => x.Image);
}
