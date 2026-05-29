using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Web.Common.Routing;

namespace N3O.Umbraco.Logging;

public class UmbracoLogEnricher : LogEnricher {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UmbracoLogEnricher(IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }

    public override IReadOnlyDictionary<string, string> GetContextData() {
        var data = new Dictionary<string, string>();

        var publishedRequest = GetPublishedRequest();

        if (publishedRequest != null) {
            data["publishedRequestPath"] = publishedRequest.AbsolutePathDecoded;

            if (publishedRequest.PublishedContent != null) {
                data["publishedContentId"] = publishedRequest.PublishedContent.Key.ToString();
            }
        }

        return data;
    }

    private IPublishedRequest GetPublishedRequest() {
        return _httpContextAccessor.HttpContext
                                   ?.Features
                                   .Get<UmbracoRouteValues>()
                                   ?.PublishedRequest;
    }
}