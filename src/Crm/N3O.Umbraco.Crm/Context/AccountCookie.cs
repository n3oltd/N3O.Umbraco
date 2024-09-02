using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Context;
using System;

namespace N3O.Umbraco.Crm.Context;

public class AccountCookie : Cookie {
    public AccountCookie(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

    protected override string GetDefaultValue() {
        return null;
    }

    protected override void SetOptions(CookieOptions cookieOptions) {
        base.SetOptions(cookieOptions);
        
        cookieOptions.HttpOnly = false;
    }

    protected override string Name => "AccountId";
    // TODO Shagufta, use a constant in the base class Session with a specific value to make this a session cookie
    protected override TimeSpan Lifetime => TimeSpan.FromHours(12);
}
