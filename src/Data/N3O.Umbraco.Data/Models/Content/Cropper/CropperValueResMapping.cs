using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Data.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Models;

public class CropperValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PublishedContentProperty, CropperValueRes>((_, _) => new CropperValueRes(), Map);
    }

    private void Map(PublishedContentProperty src, CropperValueRes dest, MapperContext ctx) {
        var croppedImage = (CroppedImage) src.Property.GetValue();

        dest.Image = croppedImage?.GetUncroppedImage();
        dest.Configuration = (CropperConfigurationRes) PropertyTypes.Cropper.GetConfigurationRes(ctx,
                                                                                                 src.ContentTypeAlias,
                                                                                                 src.Property.Alias);
    }
}