using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Context;

namespace N3O.Umbraco.Cloud.Platforms.Context;

public class UserCookie : ReadOnlyCookie {
    public UserCookie(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }
    
    protected override string Name => "n3o_user_token";
}