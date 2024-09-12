using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Captcha.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.TaxRelief.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class AccountReq : IAccount {
    [Name("Id")]
    public EntityId Id { get; set; }
    
    [Name("Reference")]
    public string Reference { get; set; }
    
    [Name("Type")]
    public AccountType Type { get; set; }

    [Name("Individual")]
    public IndividualReq Individual { get; set; }

    [Name("Organization")]
    public OrganizationReq Organization { get; set; }

    [Name("Address")]
    public AddressReq Address { get; set; }

    [Name("Email")]
    public EmailReq Email { get; set; }

    [Name("Telephone")]
    public TelephoneReq Telephone { get; set; }

    [Name("Consent")]
    public ConsentReq Consent { get; set; }

    [Name("Tax Status")]
    public TaxStatus TaxStatus { get; set; }

    [Name("Captcha")]
    public CaptchaReq Captcha { get; set; }

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