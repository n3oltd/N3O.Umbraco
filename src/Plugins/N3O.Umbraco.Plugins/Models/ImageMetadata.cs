using N3O.Umbraco.Plugins.Lookups;

namespace N3O.Umbraco.Plugins.Models;

public class ImageMetadata : Value {
    public ImageMetadata(ImageFormat format, int height, int width) {
        Format = format;
        Height = height;
        Width = width;
    }

    public ImageFormat Format { get; }
    public int Height { get; }
    public int Width { get; }
}
