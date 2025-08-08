using N3O.Umbraco.Content;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Robots;

public class RobotsSettingsPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IRobotsTxt _robotsTxt;

    public RobotsSettingsPublished(IRobotsTxt robotsTxt) {
        _robotsTxt = robotsTxt;
    }

    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.ContentType.Alias == AliasHelper<RobotsSettingsContent>.ContentTypeAlias()) {
                await _robotsTxt.PublishAsync();
            }
        }
    }
}