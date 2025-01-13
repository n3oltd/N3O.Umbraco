using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crowdfunding.Attributes;

public class UmbracoMemberOrBackOfficeAuthorizeFilter : IAsyncAuthorizationFilter {
    public Task OnAuthorizationAsync(AuthorizationFilterContext context) {
        var memberManager = context.HttpContext.RequestServices.GetRequiredService<IMemberManager>();
        var backOfficeUserAccessor = context.HttpContext.RequestServices.GetRequiredService<OurBackofficeUserAccessor>();
        
        var authorized = memberManager.IsLoggedIn() || backOfficeUserAccessor.IsLoggedIntoBackOffice();

        if (!authorized) {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new ForbidResult();
        }
        
        return Task.CompletedTask;
    }
}