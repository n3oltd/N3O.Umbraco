using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Accounts.Models;

public class NameReq : IName {
    [Name("Title")]
    public string Title { get; set; }

    [Name("First Name")]
    public string FirstName { get; set; }

    [Name("Last Name")]
    public string LastName { get; set; }
}
