using N3O.Umbraco.Webhooks.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookElementsTelephone : Value {
    [JsonConstructor]
    public WebhookElementsTelephone(WebhookLookup country, string number) {
        Country = country;
        Number = number;
    }

    public WebhookLookup Country { get; }
    public string Number { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Country;
        yield return Number;
    }
}
