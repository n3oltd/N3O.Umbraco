using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Email.Models;

public class EmailIdentityReq : IEmailIdentity {
    [Name("Email")]
    public string Email { get; set; }
    
    [Name("Name")]
    public string Name { get; set; }
}
