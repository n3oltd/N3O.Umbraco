using SixLabors.ImageSharp;

namespace N3O.Umbraco.ImageProcessing;

public interface IImageBuilder {
    IFluentImageBuilder Create(int width, int height);
    IFluentImageBuilder Create(Size size);
}