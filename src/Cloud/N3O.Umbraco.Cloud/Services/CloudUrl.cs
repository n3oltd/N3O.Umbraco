using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Cloud;

public class CloudUrl : ICloudUrl {
    private readonly SubscriptionInfo _subscription;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CloudUrl(ISubscriptionAccessor subscriptionAccessor, IWebHostEnvironment webHostEnvironment) {
        _subscription = subscriptionAccessor.GetSubscription();
        _webHostEnvironment = webHostEnvironment;
    }
    
    public string ForApi(CloudApiType type, string servicePath) {
        if (type == CloudApiTypes.Connect) {
            return ForConnectApi(servicePath);
        } else if (type == CloudApiTypes.Engage) {
            return ForEngageApi(servicePath);
        } else {
            throw UnrecognisedValueException.For(type);
        }
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

    public string ForConnectApi(string servicePath) {
        var url = new Url(GetConnectApiBaseUrl());
        url.AppendPathSegment(servicePath.Replace("eu1/api", _subscription.DataRegion.Slug));

        return url;
    }

    public string ForEngageApi(string servicePath) {
        var url = GetEngageApiBaseUrl();
        url.AppendPathSegment(servicePath.Replace("eu1/api", $"{_subscription.DataRegion.Slug}/api"));

        return url;
    }

    public string ConnectApiBaseUrl => GetConnectApiBaseUrl();

    private string GetConnectApiBaseUrl() {
        if (_webHostEnvironment.IsDevelopment()) {
            return "https://api-beta.n3o.cloud";
        } else {
            return "https://api.n3o.cloud";
        }
    }
    
    private Url GetEngageApiBaseUrl() {
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