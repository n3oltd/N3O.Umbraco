using N3O.Umbraco.Cropper.Models;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CropperValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPublishedProperty, CropperValueRes>((_, _) => new CropperValueRes(), Map);
    }

    private void Map(IPublishedProperty src, CropperValueRes dest, MapperContext ctx) {
        var croppedImage = (CroppedImage) src.GetValue();

        dest.Image = croppedImage?.GetUncroppedImage();
    }
}