using Flurl;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Slugify;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class OfferingSending : INotificationAsyncHandler<SendingContentNotification> {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IContentCache> _contentCache;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly Lazy<ISlugHelper> _slugHelper;
    
    public OfferingSending(IContentTypeService contentTypeService,
                           Lazy<IContentCache> contentCache,
                           Lazy<IContentLocator> contentLocator,
                           Lazy<ISlugHelper> slugHelper) {
        _contentTypeService = contentTypeService;
        _contentCache = contentCache;
        _contentLocator = contentLocator;
        _slugHelper = slugHelper;
    }

    public Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken) {
        var isOffering = _contentTypeService.Get(notification.Content.ContentTypeKey)
                                            .CompositionAliases()
                                            .Contains(PlatformsConstants.Offerings.CompositionAlias, true);

        if (isOffering) {
            foreach (var variant in notification.Content.Variants) {
                SetUrl( notification, variant);
            }
        }
        
        return Task.CompletedTask;
    }
    
    private void SetUrl(SendingContentNotification notification, ContentVariantDisplay variant) {
        if (variant.State == ContentSavedState.Published) {
            var campaignName = _contentLocator.Value.ById(notification.Content.ParentId.GetValueOrThrow()).Name;

            var offeringPath = _contentCache.Value.GetOfferingPath(_slugHelper.Value, campaignName, variant.Name);
            
            var urlSettings = _contentCache.Value.Single<UrlSettingsContent>();

            if (offeringPath.HasValue()) {
                var stagingUrl = new Url(urlSettings.StagingBaseUrl).AppendPathSegment(offeringPath);
                var production = new Url(urlSettings.ProductionBaseUrl).AppendPathSegment(offeringPath);

                var urls = new List<UrlInfo>();
                urls.Add(new UrlInfo(stagingUrl, true, null));
                urls.Add(new UrlInfo(production, true, null));
                
                notification.Content.Urls = urls.ToArray();
            }
        }
    }
}
