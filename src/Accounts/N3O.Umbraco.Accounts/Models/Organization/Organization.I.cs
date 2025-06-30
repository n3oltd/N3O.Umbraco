using N3O.Umbraco.Accounts.Lookups;

namespace N3O.Umbraco.Accounts.Models;

public interface IOrganization {
    OrganizationType Type { get; }
    string Name { get; }
    IName Contact { get; }
    string Number { get; }
}