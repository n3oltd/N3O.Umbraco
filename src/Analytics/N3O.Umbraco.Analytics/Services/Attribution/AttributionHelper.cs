using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Analytics.Content;
using N3O.Umbraco.Analytics.Context;
using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Utilities;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Analytics.Services;

public class AttributionHelper : IAttributionHelper {
    private static readonly string[] UtmParamters = [
        "utm_source", "utm_medium", "utm_campaign"
    ];
    
    private readonly AttributionCookie _attributionCookie;
    private readonly IJsonProvider _jsonProvider;
    private readonly IContentLocator _contentLocator;
    
    public AttributionHelper(IJsonProvider jsonProvider, IContentLocator contentLocator, AttributionCookie attributionCookie) {
        _jsonProvider = jsonProvider;
        _contentLocator = contentLocator;
        _attributionCookie = attributionCookie;
    }
    
    public void AddOrUpdateAttributionCookie(HttpContext context) {
        var currentAttributions = GetCurrentCookie();
        
        var entries = new List<AttributionStateEntry>();
        var now = Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime());
        
        currentAttributions?.Entries?.IfNotNull(entries.Add);
        entries.RemoveWhere(x => x.Expires < now);
            
        var queryParameters = context.Request.Query;
        
        if (HasAttributionOptions(queryParameters)) {
            AddOrUpdateDimensionEntries(queryParameters, entries);
        } else if (HasUtmParameters(queryParameters)) {
            AddOrUpdateUtmEntries(queryParameters, entries);
        } else {
            return;
        }
        
        var newState = new AttributionState();
        newState.Entries = entries;
        
        _attributionCookie.SetValue(_jsonProvider.SerializeObject(newState));
    }
    
    public bool HasAttributions(IQueryCollection queryParameters) {
        return HasAttributionOptions(queryParameters) || HasUtmParameters(queryParameters);
    }
    
    private AttributionState GetCurrentCookie() {
        var cookie = _attributionCookie.GetValue();

        try {
            return _jsonProvider.DeserializeObject<AttributionState>(cookie);
        } catch (Exception) {
            return null;
        }
    }
    
    private void AddOrUpdateDimensionEntries(IQueryCollection queryParameters,
                                             List<AttributionStateEntry> entries) {
        for (var index = 0; index < 11; index++) {
            var key = $"d{index}";
            
            if (queryParameters.Keys.InvariantContains(key)) {
                var value = Base64.Decode(queryParameters[key]);

                AddOrUpdateEntry(entries, index, value);
            }
        }
    }

    private void AddOrUpdateUtmEntries(IQueryCollection queryParameters,
                                       List<AttributionStateEntry> entries) {
        var attributionContent = _contentLocator.Single<AttributionContent>();

        if (!attributionContent.HasValue()) {
            return;
        }
        
        var utmSource = queryParameters["utm_source"];
        var utmMedium = queryParameters["utm_medium"];
        var utmCampaign = queryParameters["utm_campaign"];

        if (utmSource.HasValue() && attributionContent.UtmSource.HasValue()) {
            AddOrUpdateEntry(entries, attributionContent.UtmSource, utmSource);
        }
        
        if (utmMedium.HasValue() && attributionContent.UtmMedium.HasValue()) {
            AddOrUpdateEntry(entries, attributionContent.UtmMedium, utmMedium);
        }
        
        if (utmCampaign.HasValue() && attributionContent.UtmCampaign.HasValue()) {
            AddOrUpdateEntry(entries, attributionContent.UtmCampaign, utmCampaign);
        }
    }
    
    private void AddOrUpdateEntry(List<AttributionStateEntry> entries,
                                  int index,
                                  string option) {
        var newEntry = new AttributionStateEntry();
        newEntry.Index = index;
        newEntry.Option = option;
        newEntry.Expires = Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime().AddDays(3));

        if (entries.Any(x => x.Option == option && x.Index == index)) {
            entries.RemoveWhere(x => x.Option == option && x.Index == index);
        }
                
        entries.Add(newEntry);
    }
    
    private bool HasAttributionOptions(IQueryCollection queryParameters) {
        return Enumerable.Range(0, 11).Any(index => queryParameters.Keys.Contains($"d{index}"));
    }
    
    private bool HasUtmParameters(IQueryCollection queryParameters) {
        return queryParameters.Keys.Any(x => UtmParamters.Contains(x));
    }
}