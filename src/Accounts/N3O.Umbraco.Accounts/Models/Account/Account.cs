using N3O.Umbraco.Extensions;
using N3O.Umbraco.TaxRelief.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models {
    public class Account : Value, IAccount {
        [JsonConstructor]
        public Account(Name name, Address address, Telephone telephone, Email email, TaxStatus taxStatus) {
            Name = name;
            Address = address;
            Telephone = telephone;
            Email = email;
            TaxStatus = taxStatus;
        }

        public Account(IAccount account)
            : this(account.Name.IfNotNull(x => new Name(x)),
                   account.Address.IfNotNull(x => new Address(x)),
                   account.Telephone.IfNotNull(x => new Telephone(x)),
                   account.Email.IfNotNull(x => new Email(x)),
                   account.TaxStatus) { }

        public Name Name { get; }
        public Address Address { get; }
        public Telephone Telephone { get; }
        public Email Email { get; }
        public TaxStatus TaxStatus { get; }

        [JsonIgnore]
        IName IAccount.Name => Name;
        
        [JsonIgnore]
        IAddress IAccount.Address => Address;
        
        [JsonIgnore]
        ITelephone IAccount.Telephone => Telephone;
        
        [JsonIgnore]
        IEmail IAccount.Email => Email;
    }
}