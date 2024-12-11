using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Context;
using Newtonsoft.Json.Linq;
using System;
using System.Web;

namespace N3O.Umbraco.Analytics.Context;

public class AttributionCookie : Cookie {
    public AttributionCookie(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

    public JObject GetAttribution() {
        var jObject = default(JObject);

        try {
            jObject = JObject.Parse(HttpUtility.UrlDecode(GetValue()));
        } catch {
            jObject = null;
        }

        return jObject;
    }
    
    protected override void SetOptions(CookieOptions cookieOptions) {
        base.SetOptions(cookieOptions);
        
        cookieOptions.HttpOnly = false;
    }

    protected override string Name => AnalyticsConstants.Attribution.Cookie.Name;
    protected override TimeSpan Lifetime => AnalyticsConstants.Attribution.Cookie.Lifetime;
}
