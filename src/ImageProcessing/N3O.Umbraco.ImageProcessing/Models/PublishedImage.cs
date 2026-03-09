using Flurl;
using SixLabors.ImageSharp;

namespace N3O.Umbraco.ImageProcessing.Models;

public class PublishedImage : Value {
    public PublishedImage(Url url, Size size) {
        Url = url;
        Size = size;
    }

    public Url Url { get; }
    public Size Size { get; }
}