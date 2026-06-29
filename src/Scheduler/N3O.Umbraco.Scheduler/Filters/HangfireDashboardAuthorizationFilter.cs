using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace N3O.Umbraco.Scheduler;

// Authorizes the Hangfire dashboard by validating the dedicated auth cookie issued at backoffice login
// (see HangfireDashboardCookieIssuer). AuthenticateAsync runs the cookie scheme's full validation
// (signature, expiry), so access is granted only to a genuinely signed-in Settings-section user rather
// than to anyone presenting an arbitrary backoffice cookie.
public class HangfireDashboardAuthorizationFilter : IDashboardAsyncAuthorizationFilter {
    public async Task<bool> AuthorizeAsync(DashboardContext context) {
        var result = await context.GetHttpContext().AuthenticateAsync(SchedulerConstants.Dashboard.CookieScheme);

        return result.Succeeded;
    }
}
