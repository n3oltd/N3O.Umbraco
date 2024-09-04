using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookCurrency : Value {
    public WebhookCurrency(string code, string name) {
        Code = code;
        Name = name;
    }

    public string Code { get; }
    public string Name { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Code;
        yield return Name;
    }

    public Currency ToCurrency(ILookups lookups) {
        return lookups.FindById<Currency>(Code);
    }
}
