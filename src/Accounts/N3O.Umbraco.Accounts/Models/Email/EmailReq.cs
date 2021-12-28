using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Accounts.Models;

public class EmailReq : IEmail {
    [Name("Address")]
    public string Address { get; set; }
}
