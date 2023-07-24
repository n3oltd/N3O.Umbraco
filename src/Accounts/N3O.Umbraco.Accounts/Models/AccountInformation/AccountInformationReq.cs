using N3O.Umbraco.Accounts.Models;

namespace N3O.Umbraco.Giving.Checkout.Models; 

public class AccountInformationReq {
    public Name Name { get; set; }
    public Address Address { get; set; }
    public Email Email { get; set; }
    public Telephone Telephone { get; set; }
}