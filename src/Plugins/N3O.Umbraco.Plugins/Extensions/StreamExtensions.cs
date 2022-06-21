using N3O.Umbraco.Extensions;
using N3O.Umbraco.Plugins.Lookups;
using N3O.Umbraco.Plugins.Models;
using SixLabors.ImageSharp;
using System.IO;

namespace N3O.Umbraco.Plugins.Extensions;

public static class StreamExtensions {
    public static ImageMetadata GetImageMetadata(this Stream stream) {
        using (var image = Image.Load(stream, out var format)) {
            var metadata = new ImageMetadata(ImageFormat.From(format), image.Height, image.Width);

            stream.Rewind();
            
            return metadata;
        }
    }
}
