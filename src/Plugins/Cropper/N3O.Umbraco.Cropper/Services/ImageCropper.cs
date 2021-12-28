using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Hosting;

namespace N3O.Umbraco.Cropper;

public class ImageCropper : IImageCropper {
    private readonly IHostingEnvironment _hostingEnvironment;

    public ImageCropper(IHostingEnvironment hostingEnvironment) {
        _hostingEnvironment = hostingEnvironment;
    }
    
    public async Task CropAllAsync(CropperConfiguration configuration,
                                   CropperSource source,
                                   CancellationToken cancellationToken = default) {
        foreach (var (crop, index) in source.Crops.SelectWithIndex()) {
            var cropDefinition = configuration.CropDefinitions.ElementAtOrDefault(index);

            if (cropDefinition != null) {
                var srcPath = _hostingEnvironment.MapPathWebRoot(source.Src);
                var destPath = _hostingEnvironment.MapPathWebRoot(ImagePath.Get(source.MediaId,
                                                                                source.Filename,
                                                                                cropDefinition,
                                                                                crop));

                await CropAsync(cropDefinition, crop, srcPath, destPath, cancellationToken);
            }
        }
    }
    
    private async Task CropAsync(CropDefinition definition,
                                 CropperSource.Crop crop,
                                 string srcPath,
                                 string destPath,
                                 CancellationToken cancellationToken = default) {
        using (var image = await Image.LoadAsync(srcPath, cancellationToken)) {
            var clone = image.Clone(i => {
                i.Crop(new Rectangle(crop.X, crop.Y, crop.Width, crop.Height));

                i.Resize(definition.Width, definition.Height);
            });

            await clone.SaveAsync(destPath, cancellationToken);
        }
    }
}
