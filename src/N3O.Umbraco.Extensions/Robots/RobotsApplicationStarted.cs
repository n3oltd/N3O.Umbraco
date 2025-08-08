using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Robots;

public class RobotsApplicationStarted : INotificationAsyncHandler<UmbracoApplicationStartedNotification> {
    private readonly IRobotsTxt _robotsTxt;

    public RobotsApplicationStarted(IRobotsTxt robotsTxt) {
        _robotsTxt = robotsTxt;
    }

    public async Task HandleAsync(UmbracoApplicationStartedNotification notification,
                                  CancellationToken cancellationToken) {
        await _robotsTxt.PublishAsync();
    }
}