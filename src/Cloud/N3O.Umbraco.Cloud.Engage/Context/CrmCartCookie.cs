using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Context;
using N3O.Umbraco.Entities;
using System;

namespace N3O.Umbraco.Cloud.Engage.Context;

public class CrmCartCookie : Cookie {
    public CrmCartCookie(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

    protected override string GetDefaultValue() {
        return EntityId.New();
    }

    protected override void SetOptions(CookieOptions cookieOptions) {
        base.SetOptions(cookieOptions);
        
        cookieOptions.HttpOnly = false;
    }

    protected override string Name => "cartId";
    protected override TimeSpan Lifetime => TimeSpan.FromDays(90);
}