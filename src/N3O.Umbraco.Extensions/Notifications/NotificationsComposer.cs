using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.Notifications {
    public class NotificationsComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            RegisterAll(t => t.ImplementsGenericInterface(typeof(INotificationAsyncHandler<>)),
                        t => RegisterNotificationHandler(builder, t));
        }

        private void RegisterNotificationHandler(IUmbracoBuilder builder, Type handlerType) {
            var notificationTypes = handlerType.GetInterfaces()
                                               .Where(x => x.IsSubclassOrSubInterfaceOfGenericType(typeof(INotificationAsyncHandler<>)))
                                               .Select(x => x.GetParameterTypesForGenericInterface(typeof(INotificationAsyncHandler<>)).Single())
                                               .ToList();

            foreach (var notificationType in notificationTypes) {
                typeof(global::Umbraco.Cms.Core.DependencyInjection.UmbracoBuilderExtensions)
                    .CallStaticMethod(nameof(global::Umbraco.Cms.Core.DependencyInjection.UmbracoBuilderExtensions.AddNotificationAsyncHandler))
                    .OfGenericType(notificationType)
                    .OfGenericType(handlerType)
                    .WithParameter(typeof(IUmbracoBuilder), builder)
                    .Run();
            }
        }
    }
}
