using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;

namespace N3O.Umbraco.ImageProcessing.Operations;

public class CropToAspectRatioOperation : ImageOperation {
    private readonly decimal _aspectRatio;

    public CropToAspectRatioOperation(decimal aspectRatio) {
        _aspectRatio = aspectRatio;
    }

    public override void Apply(IImageProcessingContext image) {
        var currentSize = image.GetCurrentSize();
        Size newSize;
        
        if (currentSize.Width > currentSize.Height) {
            newSize = new Size(currentSize.Width, (int) Math.Floor(currentSize.Height / _aspectRatio));
        } else {
            newSize = new Size((int) Math.Floor(currentSize.Width * _aspectRatio), currentSize.Height);
        }

        var resizeOptions = new ResizeOptions();
        resizeOptions.Mode = ResizeMode.Crop;
        resizeOptions.Size = newSize;

        image.Resize(resizeOptions);
    }
}