using N3O.Umbraco.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;

namespace N3O.Umbraco.ImageProcessing.Operations;

public class ResizeOperation : ImageOperation {
    private readonly Func<Size, Size?> _getNewSize;

    public ResizeOperation(int? width, int? height) {
        if (width.HasValue() && height.HasValue()) {
            _getNewSize = _ => new Size(width.GetValueOrThrow(), height.GetValueOrThrow());
        } else if (width.HasValue()) {
            _getNewSize = s => new Size(width.GetValueOrThrow(), Scale(s.Height, s.Width, width.GetValueOrThrow()));
        } else if (height.HasValue()) {
            _getNewSize = s => new Size(Scale(s.Width, s.Height, height.GetValueOrThrow()), height.GetValueOrThrow());
        } else {
            _getNewSize = _ => null;
        }
    }

    public override void Apply(IImageProcessingContext image) {
        var currentSize = image.GetCurrentSize();
        var newSize = _getNewSize(currentSize);

        if (newSize.HasValue()) {
            var resizeOptions = new ResizeOptions();
            resizeOptions.Mode = ResizeMode.Stretch;
            resizeOptions.Size = newSize.GetValueOrThrow();

            image.Resize(resizeOptions);   
        }
    }
    
    private int Scale(int toScale, int otherOriginal, int otherResized) {
        var scaleFactor = ((decimal) otherResized) / otherOriginal;
        var scaled = (int) Math.Floor(toScale * scaleFactor);

        return scaled;
    }
}