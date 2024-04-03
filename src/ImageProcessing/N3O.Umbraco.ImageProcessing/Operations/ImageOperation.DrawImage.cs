using N3O.Umbraco.ImageProcessing.Content;
using N3O.Umbraco.ImageProcessing.Extensions;
using SixLabors.ImageSharp.Processing;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing.Operations;

public class DrawImageOperation : ImageOperation<DrawImageOperationElement> {
    public DrawImageOperation(MediaFileManager mediaFileManager) : base(mediaFileManager) { }

    protected override void Apply(DrawImageOperationElement options, IImageProcessingContext image) {
        var foregroundImage = LoadMediaImage(options.Image.Src);

        if (options.HasSize()) {
            foregroundImage = foregroundImage.Clone(o => o.Resize(options.GetSize()));
        }

        if (options.HasPoint()) {
            image.DrawImage(foregroundImage, options.GetPoint(), options.Opacity / 100f);
        } else {
            image.DrawImage(foregroundImage, options.Opacity / 100f);
        }
    }
}