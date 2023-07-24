using N3O.Umbraco.Extensions;
using N3O.Umbraco.TaxRelief.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class Account : Value, IAccount {
    [JsonConstructor]
    public Account(Name name,
                   Address address,
                   Email email,
                   Telephone telephone,
                   Consent consent,
                   TaxStatus taxStatus) {
        Name = name;
        Address = address;
        Email = email;
        Telephone = telephone;
        Consent = consent;
        TaxStatus = taxStatus;
    }

    public Account(IAccount account)
        : this(account.Name.IfNotNull(x => new Name(x)),
               account.Address.IfNotNull(x => new Address(x)),
               account.Email.IfNotNull(x => new Email(x)),
               account.Telephone.IfNotNull(x => new Telephone(x)),
               account.Consent.IfNotNull(x => new Consent(x)),
               account.TaxStatus) { }

    public Name Name { get; }
    public Address Address { get; }
    public Email Email { get; }
    public Telephone Telephone { get; }
    public Consent Consent { get; }
    public TaxStatus TaxStatus { get; }
    public bool IsTaxStatusEligible { get; set; }
    
    public bool IsComplete => IsAccountComplete();
    
    public Account WithUpdatedName(Name name) {
        return new Account(name, Address, Email, Telephone, Consent, TaxStatus);
    }
    
    public Account WithUpdatedAddress(Address address) {
        return new Account(Name, address, Email, Telephone, Consent, TaxStatus);
    }
    
    public Account WithUpdatedEmail(Email email) {
        return new Account(Name, Address, email, Telephone, Consent, TaxStatus);
    }
    
    public Account WithUpdatedTelephone(Telephone telephone) {
        return new Account(Name, Address, Email, telephone, Consent, TaxStatus);
    }
    
    public Account WithUpdatedConsent(Consent consent) {
        return new Account(Name, Address, Email, Telephone, consent, TaxStatus);
    }

    public Account WithUpdatedTaxStatus(TaxStatus taxStatus) {
        return new Account(Name, Address, Email, Telephone, Consent, taxStatus);
    }

    private bool IsAccountComplete() {
        if (!Name.HasValue()) {
            return false;
        }

        if (!Address.HasValue()) {
            return false;
        }

        if (IsTaxStatusEligible && !TaxStatus.HasValue()) {
            return false;
        }

        return true;
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
