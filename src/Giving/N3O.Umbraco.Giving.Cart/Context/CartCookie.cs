using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Context;
using N3O.Umbraco.Entities;
using System;

namespace N3O.Umbraco.Giving.Cart.Context {
    public class CartCookie : Cookie {
        public CartCookie(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

        protected override string GetDefaultValue() {
            return new RevisionId(EntityId.New(), 1);
        }

        protected override void SetOptions(CookieOptions cookieOptions) {
            base.SetOptions(cookieOptions);
            
            cookieOptions.HttpOnly = false;
        }

        protected override string Name => "CartId";
        protected override TimeSpan Lifetime => TimeSpan.FromDays(90);
    }
}