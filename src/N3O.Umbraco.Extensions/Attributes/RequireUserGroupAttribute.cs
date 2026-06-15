using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core.Security;
using UmbracoSecurity = Umbraco.Cms.Core.Constants.Security;

namespace N3O.Umbraco.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireUserGroupAttribute : Attribute, IAuthorizationFilter {
    private readonly string[] _allowedGroupAliases;

    public RequireUserGroupAttribute(params string[] allowedGroupAliases) {
        _allowedGroupAliases = allowedGroupAliases ?? Array.Empty<string>();
    }

    public void OnAuthorization(AuthorizationFilterContext context) {
        var securityAccessor = context.HttpContext
                                      .RequestServices
                                      .GetService(typeof(IBackOfficeSecurityAccessor)) as IBackOfficeSecurityAccessor;

        var currentUser = securityAccessor?.BackOfficeSecurity?.CurrentUser;

        if (currentUser == null) {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);

            return;
        }

        var isAdmin = currentUser.Groups.Any(x => x.Alias.EqualsInvariant(UmbracoSecurity.AdminGroupAlias));
        var isInAllowedGroup = currentUser.Groups.Any(x => _allowedGroupAliases.Any(x.Alias.EqualsInvariant));

        if (!isAdmin && !isInAllowedGroup) {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}
