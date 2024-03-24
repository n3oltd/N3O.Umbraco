using SixLabors.ImageSharp;

namespace N3O.Umbraco.ImageProcessing.Models;

public class ImageLayer {
    public Image Image { get; set; }
    public Size Size { get; set; }
    public Point Point { get; set; }
    public Rectangle Rectangle => new(Point, Size);

    public static ImageLayer Create(Image image, Size size, Point point) {
        var imageLayer = new ImageLayer();
        imageLayer.Image = image;
        imageLayer.Size = size;
        imageLayer.Point = point;

        return imageLayer;
    }
}