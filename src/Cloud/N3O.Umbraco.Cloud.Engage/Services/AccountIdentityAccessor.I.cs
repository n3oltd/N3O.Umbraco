using N3O.Umbraco.Cloud.Engage.Models;

namespace N3O.Umbraco.Cloud.Engage;

public interface IAccountIdentityAccessor {
    AccountIdentity Get();
    string GetId();
    string GetReference();
    string GetToken();
}
