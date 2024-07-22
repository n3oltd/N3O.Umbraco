using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Authentication.Auth0.Options;
using N3O.Umbraco.Exceptions;
using System.Net.Http;
using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication.Auth0;

public class Auth0ClientFactory : IAuth0ClientFactory {
    private readonly AuthenticationOptions _authenticationOptions;
    private readonly Auth0M2MTokenAccessor _tokenAccessor;
    private readonly IHttpClientFactory _httpClientFactory;

    public Auth0ClientFactory(IOptions<AuthenticationOptions> authenticationOptions,
                              Auth0M2MTokenAccessor tokenAccessor,
                              IHttpClientFactory httpClientFactory) {
        _authenticationOptions = authenticationOptions.Value;
        _tokenAccessor = tokenAccessor;
        _httpClientFactory = httpClientFactory;
    }
    
    public AuthenticationApiClient GetAuthenticationApiClient(ClientType clientType) {
        var loginOptions = GetClientOptions(clientType).Login;
        var httpClient = _httpClientFactory.CreateClient();
        var connection = new HttpClientAuthenticationConnection(httpClient);
            
        var auth0Client = new AuthenticationApiClient(loginOptions.Domain, connection);
            
        return auth0Client;
    }
    
    public async Task<IManagementApiClient> GetManagementApiClientAsync(ClientType clientType) {
        var managementOptions = GetClientOptions(clientType).Management;
        var token = await _tokenAccessor.GetTokenAsync(managementOptions, managementOptions.ApiIdentifier);
        var httpClient = _httpClientFactory.CreateClient();

        var connection = new HttpClientManagementConnection(httpClient);

        var auth0Client = new ManagementApiClient(token, managementOptions.Domain, connection);

        return auth0Client;
    }
    
    private Auth0AuthenticationOptions GetClientOptions(ClientType clientType) {
        if (clientType == ClientTypes.BackOffice) {
            return _authenticationOptions.BackOffice.Auth0;
        } else if (clientType == ClientTypes.Members) {
            return _authenticationOptions.Members.Auth0;
        } else {
            throw UnrecognisedValueException.For(clientType);
        }
    }
}