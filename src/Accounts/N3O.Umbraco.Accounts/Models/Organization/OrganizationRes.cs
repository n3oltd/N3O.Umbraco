using N3O.Umbraco.Accounts.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class OrganizationRes : IOrganization {
    public OrganizationType Type { get; set; }
    public string Name { get; set; }
    public NameRes Contact { get; set; }

    [JsonIgnore]
    IName IOrganization.Contact => Contact;
}