using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Accounts.Models;

public class SelectAccountReq {
    [Name("Account ID")]
    public string AccountId { get; set; }
    
    [Name("Account Reference")]
    public string AccountReference { get; set; }
}