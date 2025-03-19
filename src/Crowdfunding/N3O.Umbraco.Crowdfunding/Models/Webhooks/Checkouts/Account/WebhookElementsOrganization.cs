using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Lookups;
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

    public Organization ToOrganization(ILookups lookups) {
        var organizationType = lookups.FindById<OrganizationType>(Type.Id);
        var contact = Contact.ToName();
        
        var organization = new Organization(organizationType, Name, contact);
        
        return organization;
    }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Type;
        yield return Name;
        yield return Contact;
    }
}
