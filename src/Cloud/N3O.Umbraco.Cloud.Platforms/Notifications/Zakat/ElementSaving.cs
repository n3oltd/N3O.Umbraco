using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ZakatCalculatorFieldSaving : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly IContentHelper _contentHelper;

    public ZakatCalculatorFieldSaving(IContentHelper contentHelper) {
        _contentHelper = contentHelper;
    }
    
    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if (content.IsZakatCalculatorField()) {
                var fieldType = GetZakatCalculatorFieldType(content);
                var metal = content.GetValue(AliasHelper<ZakatCalculatorFieldSettingsContent>.PropertyAlias(x => x.Metal));

                if (fieldType == ZakatCalculatorFieldTypes.Metal && !metal.HasValue()) {
                    notification.CancelWithError("Metal must be specified");
                } else if (fieldType == ZakatCalculatorFieldTypes.Money && metal.HasValue()) {
                    notification.CancelWithError("Metal cannot specified");
                }
            }
        }
        
        return Task.CompletedTask;
    }

    private ZakatCalculatorFieldType GetZakatCalculatorFieldType(IContent content) {
        var contentProperties = _contentHelper.GetContentProperties(content);
        var fieldType = _contentHelper.GetDataListValue<ZakatCalculatorFieldType>(contentProperties,
                                                                                  AliasHelper<ZakatCalculatorFieldSettingsContent>.PropertyAlias(x => x.Type));
        
        return fieldType;
    }
}