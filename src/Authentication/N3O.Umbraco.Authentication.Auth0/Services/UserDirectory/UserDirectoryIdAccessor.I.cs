using N3O.Umbraco.Authentication.Auth0.Lookups;
using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication.Auth0;

public interface IUserDirectoryIdAccessor {
    Task<string> GetIdAsync(ClientType clientType);
}
