using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications.Elements;

public class ElementPublishingNotification : INotificationAsyncHandler<ContentPublishingNotification> {
    public Task HandleAsync(ContentPublishingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.IsDonationFormElement()) {
                    content.SetValue(AliasHelper<ElementContent>.PropertyAlias(x => x.EmbedCode),
                                     $"<n3o-donation-form id=\"{content.Key}\">");
            } else if (content.IsDonateButtonElement()) {
                content.SetValue(AliasHelper<ElementContent>.PropertyAlias(x => x.EmbedCode),
                                 $"<n3o-donate-button id=\"{content.Key}\">");
            }
        }
        
        return Task.CompletedTask;
    }
}