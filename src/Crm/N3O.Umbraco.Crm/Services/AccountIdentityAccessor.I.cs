using N3O.Umbraco.Crm.Models;

namespace N3O.Umbraco.Crm;

public interface IAccountIdentityAccessor {
    AccountIdentity Get();
    string GetId();
    string GetReference();
    string GetToken();
}
