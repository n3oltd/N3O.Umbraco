using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cropper.Notifications {
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
                var contentNodes = _contentHelper.GetContentNode(content).Flatten();
                var allProperties = contentNodes.SelectMany(x => x.Properties).ToList();

                var properties = allProperties.Where(x => x.Type.HasEditorAlias(CropperConstants.PropertyEditorAlias))
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

        private async Task GenerateCropsAsync(ContentProperty property, CancellationToken cancellationToken) {
            var dataType = _dataTypeService.GetDataType(property.Type.DataTypeId);
            var cropperConfiguration = (CropperConfiguration) dataType.Configuration;
        
            if (property.Value is string json && json.HasValue()) {
                var cropperSource = JsonConvert.DeserializeObject<CropperSource>(json);

                await _imageCropper.CropAllAsync(cropperConfiguration, cropperSource, cancellationToken);
            }
        }
    }
}
