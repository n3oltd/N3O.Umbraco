using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;

namespace N3O.Umbraco.Authentication.Controllers;

[ApiDocument(AuthenticationConstants.ApiName)]
[UmbracoMemberAuthorize]
public class AuthenticationController : SurfaceController {
    private readonly MemberSignInManager _memberSignInManager;

    public AuthenticationController(IUmbracoContextAccessor umbracoContextAccessor,
                                    IUmbracoDatabaseFactory databaseFactory,
                                    ServiceContext services,
                                    AppCaches appCaches,
                                    IProfilingLogger profilingLogger,
                                    IPublishedUrlProvider publishedUrlProvider,
                                    MemberSignInManager memberSignInManager)
        : base(umbracoContextAccessor,
               databaseFactory,
               services,
               appCaches,
               profilingLogger,
               publishedUrlProvider) {
        _memberSignInManager = memberSignInManager;
    }
    
    [HttpGet("signout")]
    public async Task<IActionResult> HandleLogout([FromQuery] string returnUrl) {
        if (HttpContext.User.Identity?.IsAuthenticated == true) {   
            await HttpContext.SignOutAsync();
                
            await _memberSignInManager.SignOutAsync();
        }
        
        return Redirect(returnUrl);
    }
}
