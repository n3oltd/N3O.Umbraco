using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Notifications;

public class OnePerSiteValidationHandler : INotificationAsyncHandler<ContentPublishingNotification> {
    private static readonly List<string> OnePerSiteContentTypes;
    
    private readonly Lazy<IContentLocator> _contentLocator;

    static OnePerSiteValidationHandler() {
        OnePerSiteContentTypes = OurAssemblies.GetTypes(t => t.IsConcreteClass() && t.HasAttribute<OnePerSiteAttribute>())
                                              .Select(AliasHelper.ContentTypeAlias)
                                              .ToList();
    }

    public OnePerSiteValidationHandler(Lazy<IContentLocator> contentLocator) {
        _contentLocator = contentLocator;
    }

    public Task HandleAsync(ContentPublishingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (OnePerSiteContentTypes.Contains(content.ContentType.Alias, true)) {
                var allPublishedContentOfSameType = _contentLocator.Value.All(content.ContentType.Alias);
                var otherIds = allPublishedContentOfSameType.Select(x => x.Key).Except(content.Key).ToList();

                if (otherIds.Any()) {
                    notification.CancelWithError($"This site may only contain a single published version of {content.ContentType.Alias.Quote()}. Copies already exist with IDs: {otherIds.ToCsv(true)}");
                }
            }
        }

        return Task.CompletedTask;
    }
}
