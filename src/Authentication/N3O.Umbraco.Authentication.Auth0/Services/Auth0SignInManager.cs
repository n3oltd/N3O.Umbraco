using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Authentication.Services;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Web.Common.Security;

namespace N3O.Umbraco.Authentication.Auth0;

public class Auth0SignInManager : ISignInManager {
    private readonly IMemberManager _memberManager;
    private readonly IMemberSignInManager _memberSignInManager;
    private readonly IUserDirectory _userDirectory;
    private readonly IUserDirectoryIdAccessor _userDirectoryIdAccessor;

    public Auth0SignInManager(IMemberSignInManager memberSignInManager,
                              IMemberManager memberManager,
                              IUserDirectory userDirectory,
                              IUserDirectoryIdAccessor userDirectoryIdAccessor) {
        _memberSignInManager = memberSignInManager;
        _memberManager = memberManager;
        _userDirectory = userDirectory;
        _userDirectoryIdAccessor = userDirectoryIdAccessor;
    }

    public async Task<string> GetPasswordResetUrlAsync() {
        if (!_memberManager.IsLoggedIn()) {
            throw new UnauthorizedAccessException();
        }
        
        var directoryId = await _userDirectoryIdAccessor.GetIdAsync();
            
        var passwordChangeUrl = await _userDirectory.GetPasswordResetUrlAsync(ClientTypes.Members, directoryId);

        return passwordChangeUrl;
    }
    
    public async Task SignOutAsync(HttpContext httpContext) {
        if (_memberManager.IsLoggedIn()) {   
            await httpContext.SignOutAsync();
                
            await _memberSignInManager.SignOutAsync();
        }
    }
}