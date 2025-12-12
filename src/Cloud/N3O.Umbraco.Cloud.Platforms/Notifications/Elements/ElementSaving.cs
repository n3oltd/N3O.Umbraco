using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ElementSaving : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly IContentTypeService _contentTypeService;

    public ElementSaving(IContentTypeService contentTypeService) {
        _contentTypeService = contentTypeService;
    }
    
    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if (content.IsElement(_contentTypeService)) {
                if (content.GetValue(AliasHelper<ElementContent>.PropertyAlias(x => x.Campaign)).HasValue() &&
                    content.GetValue(AliasHelper<DonationElementContent<DonationFormElementContent>>.PropertyAlias(x => x.Offering)).HasValue()) {
                    notification.CancelWithError("Campaign and Offering both cannot be specified");
                    
                    return Task.CompletedTask;
                }
            }
        }
        
        return Task.CompletedTask;
    }
}