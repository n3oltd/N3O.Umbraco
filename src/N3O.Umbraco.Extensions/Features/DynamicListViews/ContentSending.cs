using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Features.DynamicListViews;

// https://docs.umbraco.com/umbraco-cms/reference/notifications
// https://docs.umbraco.com/umbraco-cms/customizing/extending-overview/extension-types/condition
public class ContentSending : INotificationAsyncHandler<SendingContentNotification> {
    private readonly IDataTypeService _dataTypeService;
    private readonly PropertyEditorCollection _propertyEditors;

    public ContentSending(IDataTypeService dataTypeService, PropertyEditorCollection propertyEditors) {
        _dataTypeService = dataTypeService;
        _propertyEditors = propertyEditors;
    }

    public Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken) {
        if (ContentPathHelper.DynamicListViewsEnabled(notification.Content.Path)) {
            var dataTypeName = $"List View - {notification.Content.ContentTypeAlias}";
            var dataType = _dataTypeService.GetDataType(dataTypeName);

            if (dataType.HasValue()) {
                ConfigureListView(notification, dataType);
            }
        }

        return Task.CompletedTask;
    }

    private void ConfigureListView(SendingContentNotification notification, IDataType dataType) {
        var listViewApp = ListViewContentAppFactory.CreateContentApp(_dataTypeService,
                                                                     _propertyEditors,
                                                                     "content",
                                                                     notification.Content.ContentTypeAlias,
                                                                     dataType.Id);

        listViewApp.Weight = -666;

        notification.Content.ContentApps = notification.Content
                                                       .ContentApps
                                                       .Concat(listViewApp)
                                                       .OrderBy(x => x.Weight)
                                                       .ToList();
    }
}