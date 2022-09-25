using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cropper.Notifications;

public class CropperNotificationHandlers : INotificationAsyncHandler<ContentPublishingNotification> {
    private readonly IDataTypeService _dataTypeService;
    private readonly IContentHelper _contentHelper;
    private readonly IImageCropper _imageCropper;

    public CropperNotificationHandlers(IDataTypeService dataTypeService,
                                       IContentHelper contentHelper,
                                       IImageCropper imageCropper) {
        _dataTypeService = dataTypeService;
        _contentHelper = contentHelper;
        _imageCropper = imageCropper;
    }

    public async Task HandleAsync(ContentPublishingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            var contentProperties = _contentHelper.GetContentProperties(content);
            var properties = GetProperties(contentProperties);

            var cropperProperties = properties.Where(x => x.Type.HasEditorAlias(CropperConstants.PropertyEditorAlias))
                                              .ToList();

            foreach (var property in cropperProperties) {
                try {
                    await GenerateCropsAsync(property, cancellationToken);
                } catch (Exception ex) {
                    notification.CancelWithError($"Generating image crops failed with error: {ex.Message}");
                }
            }
        }
    }

    private IReadOnlyList<IContentProperty> GetProperties(ContentProperties content) {
        var list = new List<IContentProperty>();

        list.AddRange(content.Properties.OrEmpty());
        
        var nestedContents = content.NestedContentProperties.OrEmpty()
                                    .SelectMany(x => x.Value)
                                    .ToList();
            
        foreach (var nestedContent in nestedContents) {
            list.AddRange(GetProperties(nestedContent));
        }

        return list;
    }

    private async Task GenerateCropsAsync(IContentProperty property, CancellationToken cancellationToken) {
        var dataType = _dataTypeService.GetDataType(property.Type.DataTypeId);
        var configuration = dataType.ConfigurationAs<CropperConfiguration>();
        var json = property.Value as string ?? property.Value.IfNotNull(JsonConvert.SerializeObject);
    
        if (json.HasValue()) {
            var cropperSource = JsonConvert.DeserializeObject<CropperSource>(json);

            await _imageCropper.CropAllAsync(configuration, cropperSource, cancellationToken);
        }
    }
}
