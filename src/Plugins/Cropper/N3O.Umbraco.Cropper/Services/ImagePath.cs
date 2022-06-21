using N3O.Umbraco.Cropper.DataTypes;

namespace N3O.Umbraco.Cropper;

public static class ImagePath {
    public static string Get(string mediaId,
                             string sourceFilename,
                             CropDefinition definition,
                             CropperSource.Crop crop) {
        var cropFilename = CropFilename.Generate(mediaId, sourceFilename, definition, crop);

        return $"/media/{mediaId}/{cropFilename}";
    }
}
