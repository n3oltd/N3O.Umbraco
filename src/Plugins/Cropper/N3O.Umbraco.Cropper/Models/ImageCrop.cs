using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Cropper.Models;

[SerializeToUrl(nameof(Src))]
public class ImageCrop : Value {
    public ImageCrop(string alias, string src, int height, int width) {
        Alias = alias;
        Src = src;
        Height = height;
        Width = width;
    }

    public string Alias { get; }
    public string Src { get; }
    public int Height { get; }
    public int Width { get; }
}
