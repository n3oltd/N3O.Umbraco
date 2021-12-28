using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Plugins.Lookups;

public class ImageFormat : Lookup {
    public ImageFormat(string id) : base(id) { }
}

public class ImageFormats : StaticLookupsCollection<ImageFormat> {
    public static readonly ImageFormat Gif = new("gif");
    public static readonly ImageFormat Jpg = new("jpg");
    public static readonly ImageFormat Png = new("png");
}
