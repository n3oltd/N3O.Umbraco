using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Analytics.Context;

public class AttributionEventsCookie : Cookie {
    private readonly IJsonProvider _jsonProvider;

    public AttributionEventsCookie(IHttpContextAccessor httpContextAccessor, IJsonProvider jsonProvider)
        : base(httpContextAccessor) {
        _jsonProvider = jsonProvider;
    }

    public Attribution GetAttribution() {
        try {
            var json = GetValue();

            if (json.HasValue()) {
                var attributionDimensions = _jsonProvider.DeserializeObject<IEnumerable<AttributionDimension>>(json);

                return new Attribution(attributionDimensions);
            }
        } catch { }

        return Attribution.Empty;
    }
    
    protected override void SetOptions(CookieOptions cookieOptions) {
        base.SetOptions(cookieOptions);
        
        cookieOptions.HttpOnly = false;
    }

    protected override string Name => AnalyticsConstants.Attribution.EventsCookie.Name;
    protected override TimeSpan Lifetime => TimeSpan.FromDays(90);
}
