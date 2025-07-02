using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Dev;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Notifications;

public class NotificationsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<INotificationHandlerSkipper>(),
                    t => builder.Services.AddTransient(typeof(INotificationHandlerSkipper), t));
        
        if (DevFlags.IsNotSet(GlobalFlags.DisableNotificationRegistrations)) {
            RegisterAll(t => t.ImplementsGenericInterface(typeof(INotificationAsyncHandler<>)),
                        t => RegisterNotificationHandler(builder, t));
        }
    }

    private void RegisterNotificationHandler(IUmbracoBuilder builder, Type handlerType) {
        var notificationTypes = handlerType.GetInterfaces()
                                           .Where(x => x.IsSubclassOrSubInterfaceOfGenericType(typeof(INotificationAsyncHandler<>)))
                                           .Select(x => x.GetParameterTypesForGenericInterface(typeof(INotificationAsyncHandler<>)).Single())
                                           .ToList();
        
        foreach (var notificationType in notificationTypes) {
            this.CallMethod(nameof(RegisterHandlerForNotification))
                .OfGenericType(notificationType)
                .OfGenericType(handlerType)
                .WithParameter(typeof(IUmbracoBuilder), builder)
                .Run();
        }
    }

    private void RegisterHandlerForNotification<TNotification, THandler>(IUmbracoBuilder builder)
        where TNotification : INotification
        where THandler : class, INotificationAsyncHandler<TNotification> {
        builder.Services.AddTransient<THandler>();
        
        builder.Services.AddTransient(typeof(INotificationAsyncHandler<TNotification>), s => {
            var notificationHandlerSkippers = s.GetServices<INotificationHandlerSkipper>();
            var handler = s.GetRequiredService<THandler>();
            
            return new NotificationAsyncHandlerWrapper<TNotification>(notificationHandlerSkippers, handler);
        });
    }
}
