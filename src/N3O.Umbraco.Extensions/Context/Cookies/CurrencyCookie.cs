using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System;
using System.Linq;

namespace N3O.Umbraco.Context;

public class CurrencyCookie : Cookie {
    private readonly ILookups _lookups;

    public CurrencyCookie(IHttpContextAccessor httpContextAccessor, ILookups lookups)
        : base(httpContextAccessor) {
        _lookups = lookups;
    }

    protected override string GetDefaultValue() {
        var currency = _lookups.GetAll<Currency>().Single(x => x.IsBaseCurrency);

        return currency.Name;
    }

    protected override void SetOptions(CookieOptions cookieOptions) {
        base.SetOptions(cookieOptions);

        cookieOptions.HttpOnly = false;
    }

    protected override string Name => "Currency";
    protected override TimeSpan Lifetime => TimeSpan.FromDays(365);
}
