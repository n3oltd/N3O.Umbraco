using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Entities;
using N3O.Umbraco.References;
using N3O.Umbraco.TaxRelief.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class AccountRes : IAccount {
    public EntityId Id { get; set; }
    public string Reference { get; set; }
    public AccountType Type { get; set; }
    public IndividualRes Individual { get; set; }
    public OrganizationRes Organization { get; set; }
    public AddressRes Address { get; set; }
    public EmailRes Email { get; set; }
    public TelephoneRes Telephone { get; set; }
    public ConsentRes Consent { get; set; }
    public TaxStatus TaxStatus { get; set; }

    [JsonIgnore]
    IIndividual IAccount.Individual => Individual;

    [JsonIgnore]
    IOrganization IAccount.Organization => Organization;

    [JsonIgnore]
    IAddress IAccount.Address => Address;

    [JsonIgnore]
    IEmail IAccount.Email => Email;

    [JsonIgnore]
    ITelephone IAccount.Telephone => Telephone;

    [JsonIgnore]
    IConsent IAccount.Consent => Consent;
}