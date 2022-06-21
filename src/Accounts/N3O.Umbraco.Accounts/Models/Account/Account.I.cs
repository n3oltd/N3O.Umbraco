using N3O.Umbraco.TaxRelief.Lookups;

namespace N3O.Umbraco.Accounts.Models;

public interface IAccount {
    IName Name { get; }
    IAddress Address { get; }
    IEmail Email { get; }
    ITelephone Telephone { get; }
    IConsent Consent { get; }
    TaxStatus TaxStatus { get; }
}
