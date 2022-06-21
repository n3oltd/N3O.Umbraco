using N3O.Umbraco.Attributes;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models;

public class TelephoneReq : ITelephone {
    [Name("Country")]
    public Country Country { get; set; }

    [Name("Number")]
    public string Number { get; set; }
}
