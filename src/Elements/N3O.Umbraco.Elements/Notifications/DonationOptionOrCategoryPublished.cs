using N3O.Umbraco.Elements.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Elements.Notifications;

public class DonationOptionOrCategoryPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IElementsManager> _elementsManager;

    public DonationOptionOrCategoryPublished(IContentTypeService contentTypeService,
                                             Lazy<IElementsManager> elementsManager) {
        _contentTypeService = contentTypeService;
        _elementsManager = elementsManager;
    }

    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.IsDonationCategory(_contentTypeService) || content.IsDonationOption(_contentTypeService)) {
                await _elementsManager.Value.SaveAndPublishDonationFormAsync();
            }
        }
    }
}