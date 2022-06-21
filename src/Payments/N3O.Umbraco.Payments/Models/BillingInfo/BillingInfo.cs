using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Models;

public class BillingInfo : Value, IBillingInfo {
    [JsonConstructor]
    public BillingInfo(Address address, Email email, Name name, Telephone telephone) {
        Address = address;
        Email = email;
        Name = name;
        Telephone = telephone;
    }

    public BillingInfo(IBillingInfo billingInfo)
        : this(billingInfo.Address.IfNotNull(x => new Address(x)),
               billingInfo.Email.IfNotNull(x => new Email(x)),
               billingInfo.Name.IfNotNull(x => new Name(x)),
               billingInfo.Telephone.IfNotNull(x => new Telephone(x))) { }

    public Address Address { get; }
    public Email Email { get; }
    public Name Name { get; }
    public Telephone Telephone { get; }

    [JsonIgnore]
    IAddress IBillingInfo.Address => Address;
    
    [JsonIgnore]
    IEmail IBillingInfo.Email => Email;
    
    [JsonIgnore]
    IName IBillingInfo.Name => Name;

    [JsonIgnore]
    ITelephone IBillingInfo.Telephone => Telephone;
}
