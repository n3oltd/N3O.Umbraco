using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public abstract class WebhookEntity : Value {
    protected WebhookEntity(WebhookRevision revision, WebhookReference reference) {
        Revision = revision;
        Reference = reference;
    }

    public WebhookRevision Revision { get; }
    public WebhookReference Reference { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Revision.Id;
        yield return Reference?.Text;

        foreach (var value in GetValues()) {
            yield return value;
        }
    }

    protected abstract IEnumerable<object> GetValues();
}