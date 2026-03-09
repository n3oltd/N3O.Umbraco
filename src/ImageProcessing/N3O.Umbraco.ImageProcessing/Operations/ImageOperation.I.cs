using SixLabors.ImageSharp.Processing;

namespace N3O.Umbraco.ImageProcessing;

public interface IImageOperation {
    void Apply(IImageProcessingContext image);
}