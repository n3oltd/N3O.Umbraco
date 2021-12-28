using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Extensions;
using System.IO;

namespace N3O.Umbraco.Cropper {
    public static class ImagePath {
        public static string Get(string mediaId, string sourceFile, CropDefinition definition, CropperSource.Crop crop) {
            var extension = Path.GetExtension(sourceFile);
            var signature = $"{mediaId}{sourceFile}{definition.Alias}{crop.X}{crop.Y}{crop.Height}{crop.Width}";

            return $"/media/{mediaId}/{signature.Sha1().ToLowerInvariant().Substring(0, 8)}{extension}";
        }
    }
}
