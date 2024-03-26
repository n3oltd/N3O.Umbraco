using N3O.Umbraco.ImageProcessing.Content;
using N3O.Umbraco.ImageProcessing.Extensions;
using SixLabors.ImageSharp.Processing;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing.Operations;

public class DrawImageOperation : ImageOperation<DrawImageOperationContent> {
    public DrawImageOperation(MediaFileManager mediaFileManager) : base(mediaFileManager) { }

    protected override void Apply(DrawImageOperationContent options, IImageProcessingContext image) {
        var foregroundImage = LoadMediaImage(options.Image.Src);

        if (options.HasSize()) {
            foregroundImage = foregroundImage.Clone(o => o.Resize(options.GetSize()));
        }

        if (options.HasPoint()) {
            image.DrawImage(foregroundImage, options.GetPoint(), options.Opacity);
        } else {
            image.DrawImage(foregroundImage, options.Opacity);
        }
    }
}