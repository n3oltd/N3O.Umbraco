using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Financial;

public class Currency : ContentOrPublishedLookup {
    public Currency(string id,
                    string name,
                    Guid? contentId,
                    string code,
                    string symbol,
                    int decimalDigits,
                    bool isBaseCurrency)
        : base(id, name, contentId) {
        Code = code;
        Symbol = symbol;
        Icon = new Uri($"https://cdn.n3o.cloud/assets/icons/currencies/{code.ToLowerInvariant()}.png");
        DecimalDigits = decimalDigits;
        IsBaseCurrency = isBaseCurrency;
    }

    public string Code { get; }
    public string Symbol { get; }
    public Uri Icon { get; }
    public int DecimalDigits { get; }
    public bool IsBaseCurrency { get; }
}
