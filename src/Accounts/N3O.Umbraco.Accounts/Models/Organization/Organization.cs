using N3O.Umbraco.Accounts.Lookups;
using Newtonsoft.Json;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Accounts.Models;

public class Organization : Value, IOrganization {
    [JsonConstructor]
    public Organization(OrganizationType type, string name, Name contact, string number) {
        Type = type;
        Name = name;
        Contact = contact;
        Number = number;
    }

    public Organization(IOrganization organization)
        : this(organization.Type,
               organization.Name,
               organization.Contact.IfNotNull(x => new Name(x)),
               organization.Number) { }

    public OrganizationType Type { get; }
    public string Name { get; }
    public Name Contact { get; }
    public string Number { get; }
    
    [JsonIgnore]
    IName IOrganization.Contact => Contact;
}