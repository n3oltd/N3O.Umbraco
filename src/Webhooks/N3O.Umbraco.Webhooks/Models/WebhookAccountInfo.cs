using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookAccountInfo : Value {
    public WebhookAccountInfo(Guid id, WebhookReference reference, string name) {
        Id = id;
        Reference = reference;
        Name = name;
    }

    public Guid Id { get; }
    public WebhookReference Reference { get; }
    public string Name { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Reference;
        yield return Name;
    }
}