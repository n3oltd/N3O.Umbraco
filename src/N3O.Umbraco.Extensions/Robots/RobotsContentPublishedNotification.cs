using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Robots;

public class RobotsContentPublishedNotification : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IRobotsTxt _robotsTxt;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public RobotsContentPublishedNotification(IRobotsTxt robotsTxt, IWebHostEnvironment webHostEnvironment) {
        _robotsTxt = robotsTxt;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.ContentType.Alias == AliasHelper<RobotsContent>.ContentTypeAlias()) {
                await _webHostEnvironment.SaveFiletoWwwroot(RobotsTxt.File, _robotsTxt.GetContent());
            }
        }
    }
}