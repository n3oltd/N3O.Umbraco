using SixLabors.ImageSharp.Processing;
using System;

namespace N3O.Umbraco.ImageProcessing;

public interface IImageProcessor {
    IImageProcessor ApplyOperation(IImageOperation imageOperation);
    IImageProcessor Mutate(Action<IImageProcessingContext> action);
}