using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;

namespace N3O.Umbraco.ImageProcessing;

public class ImageProcessor : IImageProcessor {
    private readonly Image _image;

    public ImageProcessor(Image image) {
        _image = image;
    }
    
    public IImageProcessor ApplyOperation(IImageOperation operation) {
        _image.Mutate(operation.Apply);

        return this;
    }

    public IImageProcessor Mutate(Action<IImageProcessingContext> action) {
        _image.Mutate(action);

        return this;
    }
}