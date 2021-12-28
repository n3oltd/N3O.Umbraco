using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Newsletters.Models;

public class ContactReq : IContact {
    [Name("Email")]
    public string Email { get; set; }
    
    [Name("First Name")]
    public string FirstName { get; set; }
    
    [Name("Last Name")]
    public string LastName { get; set; }
}
