using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace N3O.Umbraco.ImageProcessing.Operations;

public class ConstrainSizeOperation : ImageOperation {
    private readonly Size _maxSize;

    public ConstrainSizeOperation(int maxWidth, int maxHeight) {
        _maxSize = new Size(maxHeight, maxHeight);
    }

    public override void Apply(IImageProcessingContext image) {
        var resizeOptions = new ResizeOptions();
        resizeOptions.Mode = ResizeMode.Max;
        resizeOptions.Size = _maxSize;

        image.Resize(resizeOptions);
    }
}