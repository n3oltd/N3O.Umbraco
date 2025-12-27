using SixLabors.ImageSharp.Processing;

namespace N3O.Umbraco.ImageProcessing.Operations;

public abstract class ImageOperation : IImageOperation {
    public abstract void Apply(IImageProcessingContext image);
}