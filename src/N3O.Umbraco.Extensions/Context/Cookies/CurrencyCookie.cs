using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Content;
using N3O.Umbraco.Financial;
using System;

namespace N3O.Umbraco.Context {
    public class CurrencyCookie : Cookie {
        private readonly IContentCache _contentCache;

        public CurrencyCookie(IHttpContextAccessor httpContextAccessor, IContentCache contentCache)
            : base(httpContextAccessor) {
            _contentCache = contentCache;
        }

        protected override string GetDefaultValue() {
            var currency = _contentCache.Single<Currency>(x => x.IsBaseCurrency);

            return currency?.Name;
        }

        protected override void SetOptions(CookieOptions cookieOptions) {
            base.SetOptions(cookieOptions);

            cookieOptions.HttpOnly = false;
        }

        protected override string Name => "Currency";
        protected override TimeSpan Lifetime => TimeSpan.FromDays(365);
    }
}