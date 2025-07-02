using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Cropper.Extensions;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cropper.Notifications;

[SkipDuringSync]
public abstract class OpenGraphImageCropperHandler : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly Lazy<IContentHelper> _contentHelper;
    private readonly Lazy<IImageCropper> _imageCropper;

    protected OpenGraphImageCropperHandler(Lazy<IContentHelper> contentHelper, Lazy<IImageCropper> imageCropper) {
        _contentHelper = contentHelper;
        _imageCropper = imageCropper;
    }
    
    public async Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if (ShouldExecute(content)) {
                var contentProperties = _contentHelper.Value.GetContentProperties(content);
                var croppedImage = GetImage(contentProperties);
                
                if (croppedImage.HasValue()) {
                    var cropperSource = croppedImage.GetUncroppedImage();
                    var crop = cropperSource.GetLargestCrop();

                    var cropDefinition = GetCropDefinition(cropperSource);
                
                    await _imageCropper.Value.CropAsync(cropDefinition,
                                                        crop,
                                                        cropperSource,
                                                        cancellationToken);

                    var imagePath = ImagePath.Get(cropperSource.MediaId, cropperSource.Filename, cropDefinition, crop);

                    PopulateImagePath(content, imagePath);
                }
            }
        }
    }

    private CropDefinition GetCropDefinition(CropperSource cropperSource) {
        if (cropperSource.Width >= CropDefinition.Width &&
            cropperSource.Height >= CropDefinition.Height) {
            return CropDefinition;
        } else {
            return new CropDefinition {
                Alias = CropDefinition.Alias,
                Label = CropDefinition.Label,
                Height = cropperSource.Height,
                Width = cropperSource.Width
            };
        }
    }

    protected abstract bool ShouldExecute(IContent content);
    protected abstract CroppedImage GetImage(ContentProperties contentProperties);
    protected abstract void PopulateImagePath(IContent content, string imagePath);
    
    private static readonly CropDefinition CropDefinition = new() {
        Alias = "openGraph",
        Width = 1200,
        Height = 630,
        Label = "Open Graph"
    };
}