using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

    public OurBackofficeAntiforgery(IOptionsMonitor<GlobalSettings> globalSettings,
                                    ILoggerFactory loggerFactory,
                                    IServiceProvider serviceProvider) {
        CookieSecurePolicy cookieSecurePolicy = globalSettings.CurrentValue.UseHttps ? CookieSecurePolicy.Always : CookieSecurePolicy.SameAsRequest;

        var dpProvider = serviceProvider.GetRequiredService<IDataProtectionProvider>();

        _internalAntiForgery = new ServiceCollection()
                              .AddSingleton(loggerFactory)
                              .AddSingleton<IDataProtectionProvider>((f) => dpProvider)
                              .AddAntiforgery(x => {
                                   x.HeaderName = WebConstants.Web.AngularHeadername;
                                   x.Cookie.Name = WebConstants.Web.CsrfValidationCookieName;
                                   x.Cookie.SecurePolicy = cookieSecurePolicy;
                               })
                              .BuildServiceProvider()
                              .GetRequiredService<IAntiforgery>();

        _angularCookieBuilder = new AntiforgeryOptions().Cookie;
        _angularCookieBuilder.HttpOnly = false; // Needs to be accessed from JavaScript
        _angularCookieBuilder.SecurePolicy = cookieSecurePolicy;
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
