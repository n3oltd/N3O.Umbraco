using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Cloud;

public class CloudUrl : ICloudUrl {
    private readonly SubscriptionInfo _subscription;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CloudUrl(ISubscriptionAccessor subscriptionAccessor, IWebHostEnvironment webHostEnvironment) {
        _subscription = subscriptionAccessor.GetSubscription();
        _webHostEnvironment = webHostEnvironment;
    }

    public string ForApi(string servicePath) {
        var url = GetApiBaseUrl();
        url.AppendPathSegment(servicePath.Replace("eu1/api", $"{_subscription.Region}/api"));

        return url;
    }
    
    public string ForCdn(CdnRoot root, string path) {
        var url = GetCdnBaseUrl();
        
        url.AppendPathSegment($"{root.Container.Prefix}-{_subscription.Id.Code}");

        if (root.PathSegment.HasValue()) {
            url.AppendPathSegment(root.PathSegment);
        }
        
        url.AppendPathSegment(path);

        return url;
    }

    private Url GetApiBaseUrl() {
        if (_webHostEnvironment.IsDevelopment()) {
            return new Url("https://beta.n3o.cloud");
        } else {
            return new Url("https://n3o.cloud");
        }
    }

    private Url GetCdnBaseUrl() {
        if (_webHostEnvironment.IsDevelopment()) {
            return new Url("https://cdn-beta.n3o.cloud");
        } else {
            return new Url("https://cdn.n3o.cloud");
        }
    }
}