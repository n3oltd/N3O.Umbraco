using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OpenIddict.Server;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Security;
using Umbraco.Extensions;
using UmbracoConstants = Umbraco.Cms.Core.Constants;

namespace N3O.Umbraco.Scheduler;

// In v17 the backoffice authenticates the Management API with OpenIddict JWT bearer tokens, so the
// Hangfire dashboard (a server-rendered page loaded by an iframe via a plain cookie navigation) cannot
// be gated by the backoffice bearer scheme. Instead we mint a dedicated, validated auth cookie when a
// backoffice user with Settings-section access signs in, and authorize the dashboard against that cookie
// (see HangfireDashboardAuthorizationFilter). This mirrors the approach taken by Cultiv.Hangfire for v17.
public class HangfireDashboardCookieIssuer :
    IOpenIddictServerHandler<OpenIddictServerEvents.GenerateTokenContext>,
    IOpenIddictServerHandler<OpenIddictServerEvents.ApplyRevocationResponseContext> {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string[] _claimTypes;
    private readonly TimeSpan _timeout;

    public HangfireDashboardCookieIssuer(IHttpContextAccessor httpContextAccessor,
                                         IOptions<GlobalSettings> globalSettings,
                                         IOptions<BackOfficeIdentityOptions> backOfficeIdentityOptions) {
        _httpContextAccessor = httpContextAccessor;
        _timeout = globalSettings.Value.TimeOut;

        _claimTypes = [
            backOfficeIdentityOptions.Value.ClaimsIdentity.UserIdClaimType,
            backOfficeIdentityOptions.Value.ClaimsIdentity.UserNameClaimType
        ];
    }

    public async ValueTask HandleAsync(OpenIddictServerEvents.GenerateTokenContext context) {
        if (context.Principal.Identity?.AuthenticationType != UmbracoConstants.Security.BackOfficeAuthenticationType) {
            return;
        }

        // Gate on Settings-section access: any backoffice user who can reach the Settings section receives
        // the cookie and therefore full Hangfire dashboard control, including destructive actions
        // (delete/requeue/enqueue jobs, purge queues).
        if (!context.Principal.HasClaim(c => c.Issuer == UmbracoConstants.Security.BackOfficeAuthenticationType &&
                                             c.Value == UmbracoConstants.Applications.Settings)) {
            return;
        }

        var principal = new ClaimsPrincipal(new ClaimsIdentity(context.Principal
                                                                      .Claims
                                                                      .Where(claim => _claimTypes.Contains(claim.Type)),
                                                               SchedulerConstants.Dashboard.IdentityAuthenticationType));

        await _httpContextAccessor.GetRequiredHttpContext()
                                  .SignInAsync(SchedulerConstants.Dashboard.CookieScheme,
                                               principal,
                                               GetAuthenticationProperties());
    }

    public async ValueTask HandleAsync(OpenIddictServerEvents.ApplyRevocationResponseContext context) {
        await _httpContextAccessor.GetRequiredHttpContext()
                                  .SignOutAsync(SchedulerConstants.Dashboard.CookieScheme, GetAuthenticationProperties());
    }

    private AuthenticationProperties GetAuthenticationProperties() {
        return new AuthenticationProperties {
            IsPersistent = true,
            IssuedUtc = DateTimeOffset.UtcNow,
            ExpiresUtc = DateTimeOffset.UtcNow.Add(_timeout)
        };
    }
}
