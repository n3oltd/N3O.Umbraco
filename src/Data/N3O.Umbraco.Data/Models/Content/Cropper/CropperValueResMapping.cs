using Humanizer.Bytes;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Plugins.Extensions;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Models;

public class CropperValueResMapping : IMapDefinition {
    private readonly MediaFileManager _mediaFileManager;

    public CropperValueResMapping(MediaFileManager mediaFileManager) {
        _mediaFileManager = mediaFileManager;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PublishedContentProperty, CropperValueRes>((_, _) => new CropperValueRes(), Map);
    }

    private void Map(PublishedContentProperty src, CropperValueRes dest, MapperContext ctx) {
        var croppedImage = (CroppedImage) src.Property.GetValue();
        var cropperSource = croppedImage?.GetUncroppedImage();

        dest.Image = cropperSource;
        dest.Configuration = (CropperConfigurationRes) PropertyTypes.Cropper.GetConfigurationRes(ctx,
                                                                                                 src.ContentTypeAlias,
                                                                                                 src.Property.Alias);


        if (cropperSource.HasValue()) {
            using (var stream = _mediaFileManager.FileSystem.OpenFile(cropperSource.Src)) {
                var metadata = stream.GetImageMetadata();

                dest.StorageToken = new StorageToken(cropperSource.Filename,
                                                     "media",
                                                     metadata.Format.ContentType,
                                                     ByteSize.FromBytes(stream.Length));
            }
        }
    }
}