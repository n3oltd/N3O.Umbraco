using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Attributes;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class OrganizationReq : IOrganization {
    [Name("Type")]
    public OrganizationType Type { get; set; }
    
    [Name("Name")]
    public string Name { get; set; }
    
    [Name("Contact")]
    public NameReq Contact { get; set; }
    
    [JsonIgnore]
    IName IOrganization.Contact => Contact;
}