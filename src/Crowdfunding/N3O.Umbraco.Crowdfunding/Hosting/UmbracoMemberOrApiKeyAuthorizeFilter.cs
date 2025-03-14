﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content.Settings;
using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crowdfunding.Hosting;

public class UmbracoMemberOrApiKeyAuthorizeFilter : IAsyncAuthorizationFilter {
    public Task OnAuthorizationAsync(AuthorizationFilterContext context) {
        var allowAnonymous = context.HttpContext
                                    .GetEndpoint()?
                                    .Metadata
                                    .Any(x => x.GetType() == typeof(AllowAnonymousAttribute));

        if (allowAnonymous == false) {
            var memberManager = context.HttpContext.RequestServices.GetRequiredService<IMemberManager>();
            var contentLocator = context.HttpContext.RequestServices.GetRequiredService<IContentLocator>();
            
            var apiKeyHeader = context.HttpContext.Request == null
                                   ? null
                                   : (string) context.HttpContext.Request.Headers[CrowdfundingConstants.Http.Headers.RequestApiKey];
        
            var authorized = memberManager.IsLoggedIn() || IsApiAuthorized(contentLocator, apiKeyHeader);

            if (!authorized) {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new ForbidResult();
            }
        }
        
        return Task.CompletedTask;
    }
    
    public static bool IsApiAuthorized(IContentLocator contentLocator, string requestApiKey) {
        var apiKey = contentLocator.Single<SettingsContent>().ApiKey;
        
        return apiKey.HasValue() && apiKey == requestApiKey;
    }
}