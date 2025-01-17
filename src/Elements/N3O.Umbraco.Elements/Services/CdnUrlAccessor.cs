using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Crm.Engage;

namespace N3O.Umbraco.Elements;

public class CdnUrlAccessor {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ISubscriptionAccessor _subscriptionAccessor;

    public CdnUrlAccessor(IWebHostEnvironment webHostEnvironment, ISubscriptionAccessor subscriptionAccessor) {
        _webHostEnvironment = webHostEnvironment;
        _subscriptionAccessor = subscriptionAccessor;
    }

    public string GetUrl(string path) {
        var subscriptionNumber = _subscriptionAccessor.GetSubscription().Number;
        
        var url = new Url(GetBaseUrl());
        url.AppendPathSegment("elements");
        url.AppendPathSegment(subscriptionNumber);
        url.AppendPathSegment(path);

        return url;
    }

    private string GetBaseUrl() {
        if (_webHostEnvironment.IsProduction()) {
            return "https://static.n3o.cloud";
        } else {
            return "https://static-beta.n3o.cloud";
        }
    }
}