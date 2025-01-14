using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content.Settings;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crowdfunding.Attributes;

public class UmbracoMemberOrApiKeyAuthorizeFilter : IAsyncAuthorizationFilter {
    public Task OnAuthorizationAsync(AuthorizationFilterContext context) {
        var memberManager = context.HttpContext.RequestServices.GetRequiredService<IMemberManager>();
        var contentLocator = context.HttpContext.RequestServices.GetRequiredService<IContentLocator>();
        
        var authorized = memberManager.IsLoggedIn() || IsApiAuthorized(contentLocator, context.HttpContext.Request);

        if (!authorized) {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new ForbidResult();
        }
        
        return Task.CompletedTask;
    }
    
    public static bool IsApiAuthorized(IContentLocator contentLocator, HttpRequest httpRequest) {
        var apiKeyHeader = httpRequest == null
                               ? null
                               : (string) httpRequest.Headers[CrowdfundingConstants.Http.Headers.ApiHeaderKey];
        
        var apiKey = contentLocator.Single<SettingsContent>().ApiKey;
        
        return apiKey == apiKeyHeader;
    }
}