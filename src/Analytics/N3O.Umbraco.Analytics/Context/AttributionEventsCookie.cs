using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using NodaTime;
using System;

namespace N3O.Umbraco.Analytics.Context;

public class AttributionEventsCookie : Cookie {
    private readonly IJsonProvider _jsonProvider;
    private readonly IClock _clock;

    public AttributionEventsCookie(IHttpContextAccessor httpContextAccessor, IJsonProvider jsonProvider, IClock clock)
        : base(httpContextAccessor) {
        _jsonProvider = jsonProvider;
        _clock = clock;
    }

    public AttributionEvents GetEvents() {
        try {
            var json = GetValue();

            if (json.HasValue()) {
                return _jsonProvider.DeserializeObject<AttributionEvents>(json).PurgeExpired(_clock);
            }
        } catch { }

        return AttributionEvents.Empty;
    }
    
    public void SetEvents(AttributionEvents attributionEvents) {
        SetValue(attributionEvents.IfNotNull(x => _jsonProvider.SerializeObject(x.PurgeExpired(_clock))));
    }
    
    protected override void SetOptions(CookieOptions cookieOptions) {
        base.SetOptions(cookieOptions);
        
        cookieOptions.HttpOnly = false;
    }

    protected override string Name => AnalyticsConstants.Attribution.EventsCookie.Name;
    protected override TimeSpan Lifetime => TimeSpan.FromDays(90);
}
