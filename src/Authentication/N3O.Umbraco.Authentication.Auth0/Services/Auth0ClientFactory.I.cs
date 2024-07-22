using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication.Auth0;

public interface IAuth0ClientFactory {
    AuthenticationApiClient GetAuthenticationApiClient(ClientType clientType);
    Task<IManagementApiClient> GetManagementApiClientAsync(ClientType clientType);
}