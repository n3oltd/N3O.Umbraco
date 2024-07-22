using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;

namespace N3O.Umbraco.Authentication.Auth0.Controllers;

public class Auth0LoginController : SurfaceController {
    private const string DefaultMemberTypeAlias = global::Umbraco.Cms.Core.Constants.Security.DefaultMemberTypeAlias;

    private readonly IMemberManager _memberManager;
    private readonly IMemberService _memberService;
    private readonly MemberSignInManager _memberSignInManager;

    public Auth0LoginController(IUmbracoContextAccessor umbracoContextAccessor,
                                IUmbracoDatabaseFactory databaseFactory,
                                ServiceContext services,
                                AppCaches appCaches,
                                IProfilingLogger profilingLogger,
                                IPublishedUrlProvider publishedUrlProvider,
                                IMemberManager memberManager,
                                IMemberService memberService,
                                MemberSignInManager memberSignInManager)
        : base(umbracoContextAccessor,
               databaseFactory,
               services,
               appCaches,
               profilingLogger,
               publishedUrlProvider) {
        _memberManager = memberManager;
        _memberService = memberService;
        _memberSignInManager = memberSignInManager;
    }
    
    [HttpPost]
    public IActionResult ExternalLogin(string returnUrl) {
        return Challenge(new AuthenticationProperties {
            RedirectUri = Url.Action(nameof(ExternalLoginCallback)), 
            Items = {{ "returnUrl", returnUrl }}
        }, Auth0MemberLoginProviderOptions.SchemaNameWithPrefix);
    }
    
    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback() {
        var authResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
        
        if (!authResult.Succeeded) {
            throw new Exception("Missing external cookie");
        }

        var email = authResult.Principal.FindFirstValue(ClaimTypes.Email)
                    ?? authResult.Principal.FindFirstValue("email")
                    ?? throw new Exception("Missing email claim");

        var member = await _memberManager.FindByEmailAsync(email);
        if (member == null) {
            _memberService.CreateMemberWithIdentity(email, email, email, DefaultMemberTypeAlias);

            member = await _memberManager.FindByNameAsync(email);
            await _memberManager.AddToRolesAsync(member, new[] { "User" });
        }

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        
        await _memberSignInManager.SignInAsync(member, false);

        var returnUrl = authResult.Properties?.Items["returnUrl"];
        if (returnUrl == null || !Url.IsLocalUrl(returnUrl)) {
            returnUrl = "~/";
        }

        return new RedirectResult(returnUrl);
    }
    
    [HttpPost]
    public async Task<IActionResult> HandleLogout(string returnUrl) {
        if (ModelState.IsValid == false) {
            return CurrentUmbracoPage();
        }

        var isLoggedIn = HttpContext.User.Identity?.IsAuthenticated ?? false;

        if (isLoggedIn) {   
            await HttpContext.SignOutAsync();
                
            await _memberSignInManager.SignOutAsync();
        }
        
        return Redirect(returnUrl);
    }
}
