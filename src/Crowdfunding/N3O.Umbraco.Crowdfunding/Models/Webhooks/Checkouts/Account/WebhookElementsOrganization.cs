using N3O.Umbraco.Webhooks.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookElementsOrganization : Value {
    public WebhookElementsOrganization(WebhookLookup type, string name, WebhookElementsName contact) {
        Type = type;
        Name = name;
        Contact = contact;
    }

    public WebhookLookup Type { get; }
    public string Name { get; }
    public WebhookElementsName Contact { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Type;
        yield return Name;
        yield return Contact;
    }
}
