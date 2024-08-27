using N3O.Umbraco.Accounts.Lookups;
using Newtonsoft.Json;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Accounts.Models;

public class Organization : Value, IOrganization {
    [JsonConstructor]
    public Organization(OrganizationType type, string name, Name contact) {
        Type = type;
        Name = name;
        Contact = contact;
    }

    public Organization(IOrganization organization)
        : this(organization.Type, organization.Name, organization.Contact.IfNotNull(x => new Name(x))) { }

    public OrganizationType Type { get; }
    public string Name { get; }
    public Name Contact { get; }
    
    [JsonIgnore]
    IName IOrganization.Contact => Contact;
}