using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Extensions;
using System.IO;

namespace N3O.Umbraco.Cropper {
    public static class CropFilename {
        public static string Generate(string mediaId,
                                      string sourceFilename,
                                      CropDefinition definition,
                                      CropperSource.Crop crop) {
            var extension = Path.GetExtension(sourceFilename);
            var signature = $"{mediaId}{sourceFilename}{definition.Alias}{crop.X}{crop.Y}{crop.Height}{crop.Width}";

            return $"{signature.Sha1().ToLowerInvariant().Substring(0, 8)}{extension}";
        }
    }
}
