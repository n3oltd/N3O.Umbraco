using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Content;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Search.Notification;

public class SiteMapContentPublishedNotification : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ISitemap _sitemap;

    public SiteMapContentPublishedNotification(IWebHostEnvironment webHostEnvironment, ISitemap sitemap) {
        _webHostEnvironment = webHostEnvironment;
        _sitemap = sitemap;
    }

    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.ContentType.Alias == AliasHelper<SitemapContent>.ContentTypeAlias()) {
                await _webHostEnvironment.SaveFiletoWwwroot(SearchConstants.SitemapXml, _sitemap.GetXml());
            }
        }
    }
}