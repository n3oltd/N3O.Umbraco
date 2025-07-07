using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Umbraco.Cms.Web.Common.Routing;

namespace N3O.Umbraco.Logging;

public class UmbracoLogEnricher : LogEnricher {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UmbracoLogEnricher(IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }

    public override IReadOnlyDictionary<string, string> GetContextData() {
        var contextData = new Dictionary<string, string>();

        PopulatePublishedRequest(contextData);
        
        return contextData;
    }

    private void PopulatePublishedRequest(Dictionary<string, string> contextData) {
        var publishedRequest = _httpContextAccessor.HttpContext
                                                   ?.Features
                                                   .Get<UmbracoRouteValues>()
                                                   ?.PublishedRequest;

        if (publishedRequest != null) {
            contextData["PublishedRequestPath"] = publishedRequest.AbsolutePathDecoded;

            if (publishedRequest.PublishedContent != null) {
                contextData["PublishedRequestContentId"] = publishedRequest.PublishedContent.Key.ToString();
            }

            if (publishedRequest.Domain != null) {
                contextData["PublishedRequestDomain"] = publishedRequest.Domain.Name;
            }
        }
    }
}