using SixLabors.ImageSharp;

namespace N3O.Umbraco.ImageProcessing;

public interface IImageBuilder {
    IFluentImageBuilder Create(int width, int height, Color backgroundColor);
    IFluentImageBuilder Create(Size size, Color backgroundColor);
    IFluentImageBuilder Create(string srcPath);
}