using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace N3O.Umbraco.Scheduler;

public class HangfireDashboardAuthorizationFilter : IDashboardAsyncAuthorizationFilter {
    public async Task<bool> AuthorizeAsync(DashboardContext context) {
        var result = await context.GetHttpContext().AuthenticateAsync(SchedulerConstants.Dashboard.CookieScheme);

        return result.Succeeded;
    }
}
