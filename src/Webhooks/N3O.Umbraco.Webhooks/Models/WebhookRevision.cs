using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookRevision : Value {
    public WebhookRevision(Guid id, int number) {
        Id = id;
        Number = number;
    }

    public Guid Id { get; }
    public int Number { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Number;
    }
}