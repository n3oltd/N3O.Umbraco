using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cropper.Notifications;

public class CropperNotificationHandlers : INotificationAsyncHandler<ContentPublishingNotification> {
    private readonly IDataTypeService _dataTypeService;
    private readonly IImageCropper _imageCropper;

    public CropperNotificationHandlers(IDataTypeService dataTypeService, IImageCropper imageCropper) {
        _dataTypeService = dataTypeService;
        _imageCropper = imageCropper;
    }
    
    public async Task HandleAsync(ContentPublishingNotification notification, CancellationToken cancellationToken) {
        foreach (var entity in notification.PublishedEntities) {
            var properties = entity.GetDirtyProperties()
                                   .Where(x => entity.HasProperty(x))
                                   .Select(x => entity.Properties[x])
                                   .Where(x => x.PropertyType.HasEditorAlias(CropperConstants.PropertyEditorAlias))
                                   .ToList();

            foreach (var property in properties) {
                try {
                    await GenerateCropsAsync(property, cancellationToken);
                } catch (Exception ex) {
                    var message = new EventMessage("Error",
                                                   $"Generating image crops failed with error: {ex.Message}",
                                                   EventMessageType.Error);
                    
                    notification.Cancel = true;
                    notification.Messages.Add(message);
                }
            }
        }
    }

    private async Task GenerateCropsAsync(IProperty property, CancellationToken cancellationToken) {
        var dataType = _dataTypeService.GetDataType(property.PropertyType.DataTypeId);
        var cropperConfiguration = (CropperConfiguration) dataType.Configuration;
        
        if (property.GetValue() is string json && json.HasValue()) {
            var cropperSource = JsonConvert.DeserializeObject<CropperSource>(json);

            await _imageCropper.CropAllAsync(cropperConfiguration, cropperSource, cancellationToken);
        }
    }
}
