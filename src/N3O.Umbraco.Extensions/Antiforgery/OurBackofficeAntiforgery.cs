using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Web.BackOffice.Security;
using WebConstants = Umbraco.Cms.Core.Constants;

namespace N3O.Umbraco.Antiforgery;

// TODO workaround for https://github.com/umbraco/Umbraco-CMS/issues/16107
public class OurBackofficeAntiforgery : IBackOfficeAntiforgery {
    private readonly IAntiforgery _internalAntiForgery;
    private readonly CookieBuilder _angularCookieBuilder;

    public OurBackofficeAntiforgery(IAntiforgery antiforgery, 
                                    IOptionsMonitor<GlobalSettings> globalSettings) {
        
        _internalAntiForgery = antiforgery;

        _angularCookieBuilder = new AntiforgeryOptions().Cookie;
        _angularCookieBuilder.HttpOnly = false; 
        _angularCookieBuilder.SecurePolicy = globalSettings.CurrentValue.UseHttps 
                                                 ? CookieSecurePolicy.Always 
                                                 : CookieSecurePolicy.SameAsRequest;
    }

    public async Task<Attempt<string>> ValidateRequestAsync(HttpContext httpContext) {
        try {
            await _internalAntiForgery.ValidateRequestAsync(httpContext);
            
            return Attempt<string?>.Succeed();
        } catch (Exception ex) {
            return Attempt.Fail(ex.Message);
        }
    }

    public void GetAndStoreTokens(HttpContext httpContext) {
        AntiforgeryTokenSet set = _internalAntiForgery.GetAndStoreTokens(httpContext);

        if (set.RequestToken == null) {
            throw new InvalidOperationException("Could not resolve a request token.");
        }

        httpContext.Response.Cookies.Append(WebConstants.Web.AngularCookieName,
                                            set.RequestToken,
                                            _angularCookieBuilder.Build(httpContext));
    }
}
