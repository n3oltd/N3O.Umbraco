using N3O.Umbraco.Attributes;
using N3O.Umbraco.TaxRelief.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class AccountReq : IAccount {
    [Name("Name")]
    public NameReq Name { get; set; }
    
    [Name("Address")]
    public AddressReq Address { get; set; }
    
    [Name("Email")]
    public EmailReq Email { get; set; }
    
    [Name("Telephone")]
    public TelephoneReq Telephone { get; set; }
    
    [Name("Tax Status")]
    public TaxStatus TaxStatus { get; set; }

    [JsonIgnore]
    IName IAccount.Name => Name;
    
    [JsonIgnore]
    IAddress IAccount.Address => Address;
    
    [JsonIgnore]
    IEmail IAccount.Email => Email;
    
    [JsonIgnore]
    ITelephone IAccount.Telephone => Telephone;
}
