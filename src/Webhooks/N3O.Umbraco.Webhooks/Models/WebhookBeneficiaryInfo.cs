using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookBeneficiaryInfo : Value {
    public WebhookBeneficiaryInfo(Guid id,
                                  WebhookLookup scheme,
                                  string type,
                                  string reference,
                                  string name,
                                  string location) {
        Id = id;
        Scheme = scheme;
        Type = type;
        Reference = reference;
        Name = name;
        Location = location;
    }

    public Guid Id { get; }
    public WebhookLookup Scheme { get; }
    public string Type { get; }
    public string Reference { get; }
    public string Name { get; }
    public string Location { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Scheme;
        yield return Type;
        yield return Reference;
        yield return Name;
        yield return Location;
    }
}