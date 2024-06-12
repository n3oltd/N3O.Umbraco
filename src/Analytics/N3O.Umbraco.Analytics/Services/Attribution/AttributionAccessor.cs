using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Analytics.Services;

public class AttributionAccessor : IAttributionAccessor {
    private const decimal Percentage = 100m;
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJsonProvider _jsonProvider;

    public AttributionAccessor(IHttpContextAccessor httpContextAccessor, IJsonProvider jsonProvider) {
        _httpContextAccessor = httpContextAccessor;
        _jsonProvider = jsonProvider;
    }

    public Attribution GetAttribution() {
        var cookie = _httpContextAccessor?.HttpContext?.Request.Cookies[AnalyticsConstants.AttributionCookie.Name];

        if (!cookie.HasValue()) {
            return null;
        }

        var attributionState = _jsonProvider.DeserializeObject<AttributionState>(cookie);
        
        var attribution = new Attribution();
        attribution.Dimensions = GetAttributionDimensions(attributionState);

        return attribution;
    }

    private IReadOnlyList<AttributionDimension> GetAttributionDimensions(AttributionState attributionState) {
        var attributionDimensions = new List<AttributionDimension>();
        
        var now = Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime());
        var entries = attributionState.Entries.ExceptWhere(x => x.Expires < now);
        
        foreach (var attributionGroup in entries.GroupBy(x => x.Index)) {
            var percentages = Percentage.SafeDivide(attributionGroup.Count()).ToList();

            var attributionDimensionOptions = new List<AttributionOption>();

            foreach (var(entry, index) in attributionGroup.SelectWithIndex()) {
                var option = new AttributionOption();
                option.Name = entry.Option;
                option.Percentage = percentages.ElementAt(index);
                
                attributionDimensionOptions.Add(option);
            }

            var attributionDimension = new AttributionDimension();
            attributionDimension.Index = attributionGroup.Key;
            attributionDimension.Options = attributionDimensionOptions;
            
            attributionDimensions.Add(attributionDimension);
        }
        
        return attributionDimensions;
    }
}