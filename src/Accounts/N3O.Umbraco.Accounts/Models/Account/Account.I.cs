using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Entities;
using N3O.Umbraco.TaxRelief.Lookups;

namespace N3O.Umbraco.Accounts.Models;

public interface IAccount {
    EntityId Id { get; }
    string Reference { get;  }
    AccountType Type { get; }
    IIndividual Individual { get; }
    IOrganization Organization { get; }
    IAddress Address { get; }
    IEmail Email { get; }
    ITelephone Telephone { get; }
    IConsent Consent { get; }
    TaxStatus TaxStatus { get; }
}