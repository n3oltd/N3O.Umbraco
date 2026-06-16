using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Localization;

namespace N3O.Umbraco.Hosting;

public class OurDomainRequestCultureProvider : DynamicRequestCultureProviderBase {
    public OurDomainRequestCultureProvider(RequestLocalizationOptions localizationOptions)
        : base(localizationOptions) { }

    protected override ProviderCultureResult GetProviderCultureResult(HttpContext httpContext) {
        var umbracoContextAccessor = httpContext.RequestServices.GetService<IUmbracoContextAccessor>();

        if (umbracoContextAccessor == null ||
            !umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext)) {
            return null;
        }

        var domainCache = umbracoContext.PublishedSnapshot.Domains;
        var domains = domainCache?.GetAll(false);

        if (domains == null) {
            return null;
        }

        var domain = DomainUtilities.SelectDomain(domains,
                                                  umbracoContext.CleanedUmbracoUrl,
                                                  defaultCulture: domainCache.DefaultCulture);

        return domain?.Culture is string culture ? new ProviderCultureResult(culture) : null;
    }
}
