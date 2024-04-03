using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Telemetry.Notifications;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Giving.Notifications;

[Order(0)]
public class FeedbackSchemePublished : NotificationAsyncHandlerWithTelemetry<ContentPublishedNotification> {
    
    protected override Task HandleNotificationAsync(ContentPublishedNotification notification,
                                                    CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.ContentType.Alias.EqualsInvariant(nameof(FeedbackScheme))) {
                notification.Messages.Add(new EventMessage("Scheme Updated",
                                                           "Updating a feedback scheme can effect the data import on engage. Please contact N3O support for assistance.",
                                                           EventMessageType.Warning));
            }
        }
        
        return Task.CompletedTask;
    }
}