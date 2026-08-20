using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Context;

public class CultureAccessor : ICultureAccessor {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly IDefaultCultureAccessor _defaultCultureAccessor;
    private readonly UmbracoRequestPaths _umbracoRequestPaths;

    public CultureAccessor(IHttpContextAccessor httpContextAccessor,
                           IUmbracoContextAccessor umbracoContextAccessor,
                           IVariationContextAccessor variationContextAccessor,
                           IDefaultCultureAccessor defaultCultureAccessor,
                           UmbracoRequestPaths umbracoRequestPaths) {
        _httpContextAccessor = httpContextAccessor;
        _umbracoContextAccessor = umbracoContextAccessor;
        _variationContextAccessor = variationContextAccessor;
        _defaultCultureAccessor = defaultCultureAccessor;
        _umbracoRequestPaths = umbracoRequestPaths;
    }

    public string GetCulture(string culture = null) {
        if (culture.HasValue()) {
            return culture;
        }

        // The request-localization and domain signals describe the staff member on backoffice requests.
        if (!IsBackOfficeRequest()) {
            var routedCulture = GetRoutedCulture();

            if (routedCulture.HasValue()) {
                return routedCulture;
            }

            var requestCulture = GetRequestCulture();

            if (requestCulture.HasValue()) {
                return requestCulture;
            }

            var domainCulture = GetDomainCulture();

            if (domainCulture.HasValue()) {
                return domainCulture;
            }
        }

        var variationCulture = GetVariationCulture();

        if (variationCulture.HasValue()) {
            return variationCulture;
        }

        return _defaultCultureAccessor.DefaultCulture;
    }

    private string GetDomainCulture() {
        if (!_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext) ||
            umbracoContext.Domains == null) {
            return null;
        }

        var requestUri = GetRequestUri();

        if (requestUri == null) {
            return null;
        }

        var domains = umbracoContext.Domains.GetAll(false);
        var domainAndUri = DomainUtilities.SelectDomain(domains,
                                                        requestUri,
                                                        defaultCulture: umbracoContext.Domains.DefaultCulture);

        return domainAndUri?.Culture;
    }

    private string GetRequestCulture() {
        var requestCulture = _httpContextAccessor.HttpContext?.Features.Get<IRequestCultureFeature>();

        return requestCulture?.RequestCulture.Culture.Name;
    }

    private Uri GetRequestUri() {
        return _httpContextAccessor.HttpContext?.Request.Uri();
    }

    private string GetRoutedCulture() {
        if (!_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext)) {
            return null;
        }

        return umbracoContext.PublishedRequest?.Culture;
    }

    private string GetVariationCulture() {
        return _variationContextAccessor.VariationContext?.Culture;
    }

    private bool IsBackOfficeRequest() {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null) {
            return false;
        }

        return _umbracoRequestPaths.IsBackOfficeRequest(httpContext.Request.Path.ToString());
    }
}
