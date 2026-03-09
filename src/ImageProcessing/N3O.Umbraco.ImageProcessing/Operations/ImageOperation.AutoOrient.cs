using SixLabors.ImageSharp.Processing;

namespace N3O.Umbraco.ImageProcessing.Operations;

public class AutoOrientOperation : ImageOperation {
    public override void Apply(IImageProcessingContext image) {
        image.AutoOrient();
    }
}