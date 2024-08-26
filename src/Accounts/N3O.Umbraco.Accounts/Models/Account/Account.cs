using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.TaxRelief.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class Account : Value, IAccount {
    [JsonConstructor]
    public Account(AccountType type,
                   Name name,
                   Address address,
                   Email email,
                   Telephone telephone,
                   Consent consent,
                   TaxStatus taxStatus) {
        Type = type;
        Name = name;
        Address = address;
        Email = email;
        Telephone = telephone;
        Consent = consent;
        TaxStatus = taxStatus;
    }

    public Account(IAccount account)
        : this(account.Type,
               account.Name.IfNotNull(x => new Name(x)),
               account.Address.IfNotNull(x => new Address(x)),
               account.Email.IfNotNull(x => new Email(x)),
               account.Telephone.IfNotNull(x => new Telephone(x)),
               account.Consent.IfNotNull(x => new Consent(x)),
               account.TaxStatus) { }

    public Account() { }

    public AccountType Type { get; }
    public Name Name { get; }
    public Address Address { get; }
    public Email Email { get; }
    public Telephone Telephone { get; }
    public Consent Consent { get; }
    public TaxStatus TaxStatus { get; }

    public Account WithUpdatedAddress(IAddress address) {
        return new Account(Type, Name, address.IfNotNull(x => new Address(x)), Email, Telephone, Consent, TaxStatus);
    }

    public Account WithUpdatedConsent(IConsent consent) {
        return new Account(Type, Name, Address, Email, Telephone, consent.IfNotNull(x => new Consent(x)), TaxStatus);
    }

    public Account WithUpdatedEmail(IEmail email) {
        return new Account(Type, Name, Address, email.IfNotNull(x => new Email(x)), Telephone, Consent, TaxStatus);
    }

    public Account WithUpdatedName(IName name) {
        return new Account(Type, name.IfNotNull(x => new Name(x)), Address, Email, Telephone, Consent, TaxStatus);
    }

    public Account WithUpdatedTaxStatus(TaxStatus taxStatus) {
        return new Account(Type, Name, Address, Email, Telephone, Consent, taxStatus);
    }

    public Account WithUpdatedTelephone(ITelephone telephone) {
        return new Account(Type, Name, Address, Email, telephone.IfNotNull(x => new Telephone(x)), Consent, TaxStatus);
    }

    [JsonIgnore]
    IName IAccount.Name => Name;

    [JsonIgnore]
    IAddress IAccount.Address => Address;

    [JsonIgnore]
    ITelephone IAccount.Telephone => Telephone;

    [JsonIgnore]
    IEmail IAccount.Email => Email;

    [JsonIgnore]
    IConsent IAccount.Consent => Consent;
}