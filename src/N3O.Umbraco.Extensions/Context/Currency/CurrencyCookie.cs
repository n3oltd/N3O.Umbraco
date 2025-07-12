using Microsoft.AspNetCore.Http;
using System;

namespace N3O.Umbraco.Context;

public class CurrencyCookie : Cookie {
    private readonly IDefaultCurrencyProvider _defaultCurrencyProvider;

    public CurrencyCookie(IHttpContextAccessor httpContextAccessor, IDefaultCurrencyProvider defaultCurrencyProvider)
        : base(httpContextAccessor) {
        _defaultCurrencyProvider = defaultCurrencyProvider;
    }
    
    protected override string GetDefaultValue() {
        var currency = _defaultCurrencyProvider.GetDefaultCurrencyAsync().GetAwaiter().GetResult();

        return currency.Code;
    }

    protected override void SetOptions(CookieOptions cookieOptions) {
        base.SetOptions(cookieOptions);

        cookieOptions.HttpOnly = false;
    }

    protected override string Name => "Currency";
    protected override TimeSpan Lifetime => TimeSpan.FromDays(365);
}
