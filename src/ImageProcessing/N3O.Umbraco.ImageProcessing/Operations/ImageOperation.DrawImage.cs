using N3O.Umbraco.ImageProcessing.Content;
using SixLabors.ImageSharp.Processing;
using System;

namespace N3O.Umbraco.ImageProcessing.Operations;

public class DrawImageOperation : ImageOperation<DrawImageOperationContent> {
    protected override void Apply(DrawImageOperationContent options, IImageProcessingContext image) {
        throw new NotImplementedException();
    }
}