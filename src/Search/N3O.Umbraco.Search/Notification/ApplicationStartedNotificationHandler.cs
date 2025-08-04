using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Content;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Search.Notification;

public class ApplicationStartedNotificationHandler : INotificationAsyncHandler<UmbracoApplicationStartedNotification> {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ISitemap _sitemap;

    public ApplicationStartedNotificationHandler(IWebHostEnvironment webHostEnvironment, ISitemap sitemap) {
        _webHostEnvironment = webHostEnvironment;
        _sitemap = sitemap;
    }
    
    public async Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken) {
        await _webHostEnvironment.SaveFiletoWwwroot(SearchConstants.SitemapXml, _sitemap.GetXml());
    }
}