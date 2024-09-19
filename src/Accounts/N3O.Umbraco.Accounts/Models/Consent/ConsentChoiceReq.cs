using N3O.Umbraco.Attributes;
using N3O.Umbraco.Accounts.Lookups;

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
