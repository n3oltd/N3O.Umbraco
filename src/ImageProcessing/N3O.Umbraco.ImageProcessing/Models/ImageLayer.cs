using SixLabors.ImageSharp;

namespace N3O.Umbraco.ImageProcessing.Models;

public class ImageLayer {
    public Image Image { get; set; }
    public Size Size { get; set; }
    public Point Point { get; set; }
    public Rectangle Rectangle => new(Point, Size);

    public static ImageLayer Create(Image image, int x, int y, int width, int height) {
        var imageLayer = new ImageLayer();
        imageLayer.Image = image;
        imageLayer.Size = new Size(width, height);
        imageLayer.Point = new Point(x, y);

        return imageLayer;
    }
}