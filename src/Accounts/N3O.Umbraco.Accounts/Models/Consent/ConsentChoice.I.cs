using N3O.Umbraco.Accounts.Lookups;

namespace N3O.Umbraco.Accounts.Models;

public interface IConsentChoice {
    ConsentChannel Channel { get; }
    ConsentCategory Category { get; }
    ConsentResponse Response { get; }
}
