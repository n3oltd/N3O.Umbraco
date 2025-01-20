using N3O.Umbraco.Content;
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
    
    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.IsElementsSettings()) {
                _elementsManager.Value.SaveAndPublishElementsSettings().GetAwaiter().GetResult();
            }
        }
        
        return Task.CompletedTask;
    }
}