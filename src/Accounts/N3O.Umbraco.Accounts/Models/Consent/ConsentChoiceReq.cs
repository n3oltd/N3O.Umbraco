using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Accounts.Models;

public class ConsentChoiceReq : IConsentChoice {
    [Name("Channel")]
    public ConsentChannel Channel { get; set; }

    [Name("Category")]
    public ConsentCategory Category { get; set; }
    
    [Name("Preference")]
    public ConsentResponse Response { get; set; }

    public string CategoryName => Category?.Name;
}
