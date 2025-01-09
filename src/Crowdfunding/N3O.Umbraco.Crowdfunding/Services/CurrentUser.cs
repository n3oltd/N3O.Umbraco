using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Extensions;
using Umbraco.Extensions;
using SecurityConstants = Umbraco.Cms.Core.Constants.Security;

namespace N3O.Umbraco.Crowdfunding;

public class CurrentUser {
    private readonly IOptionsSnapshot<CookieAuthenticationOptions> _cookieOptionsSnapshot;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IOptionsSnapshot<CookieAuthenticationOptions> cookieOptionsSnapshot,
                       IHttpContextAccessor httpContextAccessor) {
        _cookieOptionsSnapshot = cookieOptionsSnapshot;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public bool IsLoggedIntoBackOffice() {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null) {
            return false;
        }

        var cookieOptions = _cookieOptionsSnapshot.Get(SecurityConstants.BackOfficeAuthenticationType);
        var backOfficeCookie = httpContext.Request.Cookies[cookieOptions.Cookie.Name!];

        if (string.IsNullOrEmpty(backOfficeCookie)) {
            return false;
        }

        var authenticationTicket = cookieOptions.TicketDataFormat.Unprotect(backOfficeCookie!);
        var backOfficeIdentity = authenticationTicket?.Principal.GetUmbracoIdentity();

        var result = backOfficeIdentity.HasValue(x => x.AuthenticationType) &&
                     backOfficeIdentity.AuthenticationType.EqualsInvariant(SecurityConstants.BackOfficeAuthenticationType);

        return result;
    }
}