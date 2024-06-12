using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Analytics.Content;
using N3O.Umbraco.Analytics.Context;
using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace N3O.Umbraco.Analytics.Hosting;

public class AttributionMiddleware : IMiddleware {
    private readonly Lazy<IUmbracoContextFactory> _umbracoContextFactory;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly Lazy<AttributionEventsCookie> _attributionEventsCookie;
    private readonly Lazy<IClock> _clock;

    public AttributionMiddleware(Lazy<IUmbracoContextFactory> umbracoContextFactory,
                                 Lazy<IContentLocator> contentLocator,
                                 Lazy<AttributionEventsCookie> attributionEventsCookie,
                                 Lazy<IClock> clock) {
        _umbracoContextFactory = umbracoContextFactory;
        _contentLocator = contentLocator;
        _attributionEventsCookie = attributionEventsCookie;
        _clock = clock;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        if (context.Request.Query.Keys.ContainsAny(AnalyticsConstants.Attribution.QueryString.All, true)) {
            using (_umbracoContextFactory.Value.EnsureUmbracoContext()) {
                ParseQueryString(context.Request.Query);
            }
        }

        await next(context);
    }

    private void ParseQueryString(IQueryCollection query) {
        var expires = _clock.Value.GetCurrentInstant().Plus(AnalyticsConstants.Attribution.Lifetime);
        var existingEvents = _attributionEventsCookie.Value.GetEvents().Events;
        var attributionEvents = GetAttributionEvents(query, expires);
        var utmEvents = GetUtmEvents(query, expires);
        
        _attributionEventsCookie.Value.SetEvents(AttributionEvents.From(existingEvents, attributionEvents, utmEvents));
    }

    private IEnumerable<AttributionEvent> GetAttributionEvents(IQueryCollection query, Instant expires) {
        var encodedAttribution = query[AnalyticsConstants.Attribution.QueryString.EncodedAttribution].FirstOrDefault();
        var queryParameters = HttpUtility.ParseQueryString(Base64.Decode(encodedAttribution));
        
        for (var index = 0; index < AnalyticsConstants.Attribution.DimensionsCount; index++) {
            var key = AnalyticsConstants.Attribution.GetKey(index);
            
            if (queryParameters.ContainsKey(key)) {
                yield return new AttributionEvent(index, queryParameters[key], expires);
            }
        }
    }

    private IEnumerable<AttributionEvent> GetUtmEvents(IQueryCollection query, Instant expires) {
        var settingsContent = _contentLocator.Value.Single<AttributionSettingsContent>();

        var utmCampaign = query[AnalyticsConstants.Attribution.QueryString.UtmCampaign];
        var utmMedium = query[AnalyticsConstants.Attribution.QueryString.UtmMedium];
        var utmSource = query[AnalyticsConstants.Attribution.QueryString.UtmSource];
            
        if (utmCampaign.HasValue() && settingsContent.HasValue(x => x.UtmCampaignDimensionIndex)) {
            yield return new AttributionEvent(settingsContent.UtmCampaignDimensionIndex.GetValueOrThrow(),
                                              utmCampaign,
                                              expires);
        }
        
        if (utmCampaign.HasValue() && settingsContent.HasValue(x => x.UtmCampaignDimensionIndex)) {
            yield return new AttributionEvent(settingsContent.UtmCampaignDimensionIndex.GetValueOrThrow(),
                                              utmCampaign,
                                              expires);
        }
        
        if (utmMedium.HasValue() && settingsContent.HasValue(x => x.UtmMediumDimensionIndex)) {
            yield return new AttributionEvent(settingsContent.UtmMediumDimensionIndex.GetValueOrThrow(),
                                              utmMedium,
                                              expires);
        }
        
        if (utmSource.HasValue() && settingsContent.HasValue(x => x.UtmSourceDimensionIndex)) {
            yield return new AttributionEvent(settingsContent.UtmSourceDimensionIndex.GetValueOrThrow(),
                                              utmSource,
                                              expires);
        }
    }
}