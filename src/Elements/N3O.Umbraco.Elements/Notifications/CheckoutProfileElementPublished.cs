using N3O.Umbraco.Elements.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Elements.Notifications;

public class CheckoutProfileElementPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IContentService _contentService;
    private readonly Lazy<IElementsManager> _elementsManager;

    public CheckoutProfileElementPublished(IContentService contentService, Lazy<IElementsManager> elementsManager) {
        _contentService = contentService;
        _elementsManager = elementsManager;
    }
    
    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.IsCheckoutProfileDependency(_contentService)) {
                try {
                    await _elementsManager.Value.SaveAndPublishCheckoutProfileAsync();
                } catch (Exception ex) {
                    notification.Messages.Add(new EventMessage("Warning",
                                                               $"There was an error publishing the checkout profile: {ex.Message}",
                                                               EventMessageType.Error));
                }
            }
        }
    }
}