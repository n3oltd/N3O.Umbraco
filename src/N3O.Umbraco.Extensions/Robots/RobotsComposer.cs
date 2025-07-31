using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Robots;

public class RobotsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        
        builder.Services.AddTransient<IRobotsTxt, RobotsTxt>();
        
        builder.Services.AddTransient<IRobotsFileService, RobotsFileService>();

        builder.Services.AddTransient<INotificationAsyncHandler<ContentPublishedNotification>, RobotsContentPublishedNotification>();

        builder.Services.AddTransient<INotificationAsyncHandler<UmbracoApplicationStartedNotification>, RobotsStartupHandler>();

    }
}