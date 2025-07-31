using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Robots;

public class RobotsStartupHandler : INotificationAsyncHandler<UmbracoApplicationStartedNotification> {
    private readonly IRobotsFileService _robotsFileService;
    private readonly IUmbracoContextFactory _umbracoContextFactory;
    
    public RobotsStartupHandler(IRobotsFileService robotsFileService, IUmbracoContextFactory umbracoContextFactory)
    {
        _robotsFileService = robotsFileService;
        _umbracoContextFactory = umbracoContextFactory;
    }
    
    public async Task HandleAsync(UmbracoApplicationStartedNotification notification,
        CancellationToken cancellationToken)
    {
        using (_umbracoContextFactory.EnsureUmbracoContext())
        {
            await _robotsFileService.SaveRobotsFileToWwwroot();
        }
        
    }
}