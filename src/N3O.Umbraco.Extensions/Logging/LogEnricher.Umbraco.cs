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
        var contextData = new Dictionary<string, string>();

        var publishedRequest = GetPublishedRequest();

        if (publishedRequest != null) {
            contextData["publishedRequestPath"] = publishedRequest.AbsolutePathDecoded;

            if (publishedRequest.PublishedContent != null) {
                contextData["publishedRequestContentId"] = publishedRequest.PublishedContent.Key.ToString();
            }

            if (publishedRequest.Domain != null) {
                contextData["publishedRequestDomain"] = publishedRequest.Domain.Name;
            }
        }

        return contextData;
    }

    public override IReadOnlyDictionary<string, string> GetTags() {
        var tags = new Dictionary<string, string>();

        var publishedRequest = GetPublishedRequest();

        if (publishedRequest != null) {
            if (publishedRequest.PublishedContent != null) {
                tags["publishedContentId"] = publishedRequest.PublishedContent.Key.ToString();
            }

            if (publishedRequest.Domain != null) {
                tags["publishedDomain"] = publishedRequest.Domain.Name;
            }
        }

        return tags;
    }

    private IPublishedRequest GetPublishedRequest() {
        return _httpContextAccessor.HttpContext
                                   ?.Features
                                   .Get<UmbracoRouteValues>()
                                   ?.PublishedRequest;
    }
}