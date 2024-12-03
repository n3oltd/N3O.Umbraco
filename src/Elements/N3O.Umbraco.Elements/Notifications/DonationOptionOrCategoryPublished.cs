using N3O.Umbraco.Crm;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Elements.Notifications;

public class DonationOptionOrCategoryPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IContentTypeService _contentTypeService;
    private readonly IElementsManager _elementsManager;

    public DonationOptionOrCategoryPublished(IElementsManager elementsManager, IContentTypeService contentTypeService) {
        _elementsManager = elementsManager;
        _contentTypeService = contentTypeService;
    }

    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if(IsDonationCategoryOrDonationOption(content)) {
                await _elementsManager.CreateOrUpdateDonationOptionAsync();
            }

            
        }
    }

    private bool IsDonationCategoryOrDonationOption(IContent content) {
        var contentType = _contentTypeService.Get(content.ContentType.Id);
        
        if (contentType.ContentTypeCompositionExists(ElementsConstants.DonationCategory.Alias) ||
            content.ContentType.Alias.EqualsInvariant(ElementsConstants.DonationCategory.Alias)) {
            return true;
        }

        if (contentType.ContentTypeCompositionExists(ElementsConstants.DonationOption.Alias)) {
            return true;
        }
        
        return false;
    }
}