using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ElementPublishing : INotificationAsyncHandler<ContentPublishingNotification> {
    public Task HandleAsync(ContentPublishingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            var elementType = ElementTypes.FindByContentTypeAlias(content.ContentType.Alias);
            
            if (elementType.HasValue()) {
                content.SetValue(AliasHelper<ElementContent>.PropertyAlias(x => x.EmbedCode),
                                 elementType.GetEmbedCode(content.Key));
            }
        }
        
        return Task.CompletedTask;
    }
}