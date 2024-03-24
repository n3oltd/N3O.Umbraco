using N3O.Umbraco.ImageProcessing.Content;
using SixLabors.ImageSharp.Processing;
using System.Threading.Tasks;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing.Operations;

public class DrawImageOperation : ImageOperation<DrawImageOperationContent> {
    public DrawImageOperation(MediaFileManager mediaFileManager) : base(mediaFileManager) { }
    
    protected override async Task ApplyAsync(DrawImageOperationContent options, IImageProcessingContext image) {
        using (var foregroundImage = await LoadImageAsync(options.Image)) {
            image.DrawImage(foregroundImage, options.Opacity);
        }
    }
}