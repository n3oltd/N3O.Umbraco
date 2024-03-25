using N3O.Umbraco.ImageProcessing.Content;
using SixLabors.ImageSharp.Processing;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing.Operations;

public class DrawImageOperation : ImageOperation<DrawImageOperationContent> {
    public DrawImageOperation(MediaFileManager mediaFileManager) : base(mediaFileManager) { }

    protected override void Apply(DrawImageOperationContent options, IImageProcessingContext image) {
        using (var foregroundImage = LoadMediaImage(options.Image.Src)) {
            image.DrawImage(foregroundImage, options.Opacity);
        }
    }
}