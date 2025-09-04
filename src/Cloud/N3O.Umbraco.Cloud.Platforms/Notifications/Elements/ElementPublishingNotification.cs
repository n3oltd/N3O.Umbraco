using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Notifications.Elements;

public class ElementPublishingNotification : INotificationAsyncHandler<ContentPublishingNotification> {
    public Task HandleAsync(ContentPublishingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.IsDonationFormElement() || content.IsDonateButtonElement()) {
                var type = content.IsDonationFormElement() ? ElementTypes.DonationForm : ElementTypes.DonateButton;
                
                var tag = new TagBuilder(type.TagName);
        
                tag.Attributes.Add("id", content.Key.ToString());
        
                content.SetValue(AliasHelper<ElementContent>.PropertyAlias(x => x.EmbedCode), tag.ToHtmlString());
            }
        }
        
        return Task.CompletedTask;
    }
}