using N3O.Umbraco.Elements.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Elements.Notifications;

public class ElementsSettingsPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly Lazy<IElementsManager> _elementsManager;

    public ElementsSettingsPublished(Lazy<IElementsManager> elementsManager) {
        _elementsManager = elementsManager;
    }
    
    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.IsElementsSettings()) {
                await _elementsManager.Value.SaveAndPublishElementsSettingsAsync();
            }
        }
    }
}