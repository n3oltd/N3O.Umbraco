using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Robots;

public class
    ApplicationStartedNotificationHandler : INotificationAsyncHandler<
    UmbracoApplicationStartedNotification> {
    private readonly IUmbracoContextFactory _umbracoContextFactory;
    private readonly IRobotsTxt _robotsTxt;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ApplicationStartedNotificationHandler(IUmbracoContextFactory umbracoContextFactory,
                                                 IRobotsTxt robotsTxt,
                                                 IWebHostEnvironment webHostEnvironment) {
        _umbracoContextFactory = umbracoContextFactory;
        _robotsTxt = robotsTxt;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task HandleAsync(UmbracoApplicationStartedNotification notification,
                                  CancellationToken cancellationToken) {
        using (_umbracoContextFactory.EnsureUmbracoContext()) {
            await _webHostEnvironment.SaveFiletoWwwroot(RobotsTxt.File, _robotsTxt.GetContent());
        }
    }
}