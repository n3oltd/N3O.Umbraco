using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.TaxRelief.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class AccountRes : IAccount {
    public AccountType Type { get; set; }
    public NameRes Name { get; set; }
    public AddressRes Address { get; set; }
    public EmailRes Email { get; set; }
    public TelephoneRes Telephone { get; set; }
    public ConsentRes Consent { get; set; }
    public TaxStatus TaxStatus { get; set; }

    [JsonIgnore]
    IName IAccount.Name => Name;

    [JsonIgnore]
    IAddress IAccount.Address => Address;

    [JsonIgnore]
    IEmail IAccount.Email => Email;

    [JsonIgnore]
    ITelephone IAccount.Telephone => Telephone;
    
    [JsonIgnore]
    IConsent IAccount.Consent => Consent;
}
