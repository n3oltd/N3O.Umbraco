using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.TaxRelief.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class Account : Value, IAccount {
    [JsonConstructor]
    public Account(EntityId id,
                   string reference,
                   AccountType type,
                   Individual individual,
                   Organization organization,
                   Address address,
                   Email email,
                   Telephone telephone,
                   Consent consent,
                   TaxStatus taxStatus) {
        Id = id;
        Reference = reference;
        Type = type;
        Individual = individual;
        Organization = organization;
        Address = address;
        Email = email;
        Telephone = telephone;
        Consent = consent;
        TaxStatus = taxStatus;
    }

    public Account(IAccount account)
        : this(account.Id,
               account.Reference,
               account.Type,
               account.Individual.IfNotNull(x => new Individual(x)),
               account.Organization.IfNotNull(x => new Organization(x)),
               account.Address.IfNotNull(x => new Address(x)),
               account.Email.IfNotNull(x => new Email(x)),
               account.Telephone.IfNotNull(x => new Telephone(x)),
               account.Consent.IfNotNull(x => new Consent(x)),
               account.TaxStatus) { }

    public Account() { }

    public EntityId Id { get; }
    public string Reference { get; }
    public AccountType Type { get; }
    public Individual Individual { get; }
    public Organization Organization { get; }
    public Address Address { get; }
    public Email Email { get; }
    public Telephone Telephone { get; }
    public Consent Consent { get; }
    public TaxStatus TaxStatus { get; }

    public Account WithUpdatedAddress(IAddress address) {
        return new Account(Id,
                           Reference,
                           Type,
                           Individual,
                           Organization,
                           address.IfNotNull(x => new Address(x)),
                           Email,
                           Telephone,
                           Consent,
                           TaxStatus);
    }

    public Account WithUpdatedConsent(IConsent consent) {
        return new Account(Id,
                           Reference,
                           Type,
                           Individual,
                           Organization,
                           Address,
                           Email,
                           Telephone,
                           consent.IfNotNull(x => new Consent(x)),
                           TaxStatus);
    }

    public Account WithUpdatedEmail(IEmail email) {
        return new Account(Id,
                           Reference,
                           Type,
                           Individual,
                           Organization,
                           Address,
                           email.IfNotNull(x => new Email(x)),
                           Telephone,
                           Consent,
                           TaxStatus);
    }

    public Account WithUpdatedIndividual(IIndividual individual) {
        return new Account(Id,
                           Reference,
                           Type,
                           individual.IfNotNull(x => new Individual(x)),
                           Organization,
                           Address,
                           Email,
                           Telephone,
                           Consent,
                           TaxStatus);
    }

    public Account WithUpdatedOrganization(IOrganization organization) {
        return new Account(Id,
                           Reference,
                           Type,
                           Individual,
                           organization.IfNotNull(x => new Organization(x)),
                           Address,
                           Email,
                           Telephone,
                           Consent,
                           TaxStatus);
    }

    public Account WithUpdatedTaxStatus(TaxStatus taxStatus) {
        return new Account(Id, 
                           Reference,
                           Type, 
                           Individual,
                           Organization, 
                           Address, 
                           Email,
                           Telephone,
                           Consent,
                           taxStatus);
    }

    public Account WithUpdatedTelephone(ITelephone telephone) {
        return new Account(Id,
                           Reference,
                           Type,
                           Individual,
                           Organization,
                           Address,
                           Email,
                           telephone.IfNotNull(x => new Telephone(x)),
                           Consent,
                           TaxStatus);
    }

    public Account WithUpdatedType(AccountType type) {
        return new Account(Id, 
                           Reference,
                           type, 
                           Individual, 
                           Organization, 
                           Address, 
                           Email, 
                           Telephone, 
                           Consent, 
                           TaxStatus);
    }

    [JsonIgnore]
    IIndividual IAccount.Individual => Individual;

    [JsonIgnore]
    IOrganization IAccount.Organization => Organization;

    [JsonIgnore]
    IAddress IAccount.Address => Address;

    [JsonIgnore]
    ITelephone IAccount.Telephone => Telephone;

    [JsonIgnore]
    IEmail IAccount.Email => Email;

    [JsonIgnore]
    IConsent IAccount.Consent => Consent;
}