using SixLabors.ImageSharp;

namespace N3O.Umbraco.ImageProcessing.Models;

public class ImageLayer {
    private ImageLayer(Image image, string srcPath, Size size, Point point) {
        Image = image;
        SrcPath = srcPath;
        Size = size;
        Point = point;
    }

    public Image Image { get; }
    public string SrcPath { get; }
    public Size Size { get; }
    public Point Point { get; }

    public static ImageLayer ForImage(Image image, int x, int y, int width, int height) {
        return Create(image, null, x, y, width, height);
    }
    
    public static ImageLayer ForMedia(string srcPath, int x, int y, int width, int height) {
        return Create(null, srcPath, x, y, width, height);
    }
    
    private static ImageLayer Create(Image image, string srcPath, int x, int y, int width, int height) {
        var imageLayer = new ImageLayer(image, srcPath, new Size(width, height), new Point(x, y));

        return imageLayer;
    }
}