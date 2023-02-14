using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookLookup : Value {
    public WebhookLookup(string id, string name) {
        Id = id;
        Name = name;
    }

    public string Id { get; }
    public string Name { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Name;
    }
}