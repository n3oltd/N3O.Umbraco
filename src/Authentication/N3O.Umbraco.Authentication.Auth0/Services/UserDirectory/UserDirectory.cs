using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Extensions;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Auth0User = Auth0.ManagementApi.Models.User;

namespace N3O.Umbraco.Authentication.Auth0;

public class UserDirectory : IUserDirectory {
    private IManagementApiClient _managementClient;
    private AuthenticationApiClient _authClient;
    
    private readonly IAuth0ClientFactory _clientFactory;

    public UserDirectory(IAuth0ClientFactory clientFactory) {
        _clientFactory = clientFactory;
    }

    public async Task<Auth0User> CreateUserIfNotExistsAsync(ClientType clientType,
                                                            string clientId,
                                                            string connectionName,
                                                            string email,
                                                            string firstName,
                                                            string lastName,
                                                            string password = null) {
        var managementClient = await GetManagementClientAsync(clientType); 
        var authClient = GetAuthenticationClient(clientType);
        
        var isFederated = await IsFederatedByEmailAsync(managementClient, email);

        if (isFederated) {
            return null;
        }

        var user = await GetDirectoryUserByEmailAsync(managementClient, email);

        if (user == null) {
            if (password == null) {
                password = PasswordGenerator.Generate(10,
                                                      PasswordCharacters.UppercaseLetters |
                                                      PasswordCharacters.LowercaseLetters |
                                                      PasswordCharacters.AlphaNumeric);
            }

            user = await CreateDirectoryUserAsync(managementClient, connectionName, email, firstName, lastName, password);

            await SendPasswordResetEmailAsync(managementClient, authClient, clientId, connectionName, email);
        }

        return user;
    }
    
    public async Task<Auth0User> GetUserByEmailAsync(ClientType clientType, string email) {
        var managementClient = await GetManagementClientAsync(clientType);
        
        var auth0Users = await managementClient.Users.GetUsersByEmailAsync(email.ToLowerInvariant());

        if (auth0Users.Count > 1) {
            throw new Exception($"Multiple users with email {email.Quote()} found in Auth0");
        }

        return auth0Users.SingleOrDefault();
    }

    private async Task<Auth0User> CreateDirectoryUserAsync(IManagementApiClient managementClient,
                                                           string connectionName,
                                                           string email,
                                                           string firstName,
                                                           string lastName,
                                                           string password) {
        var request = new UserCreateRequest();
        request.Email = email.ToLowerInvariant();
        request.Password = password;
        request.VerifyEmail = false;
        request.EmailVerified = true;
        request.FirstName = firstName;
        request.LastName = lastName;
        request.FullName = $"{firstName} {lastName}".Trim();
        request.Connection = connectionName;

        var auth0User = await managementClient.Users.CreateAsync(request);

        return auth0User;
    }

    private async Task<Auth0User> GetDirectoryUserByEmailAsync(IManagementApiClient managementClient, string email) {
        var auth0Users = await managementClient.Users.GetUsersByEmailAsync(email.ToLowerInvariant());

        if (auth0Users.Count > 1) {
            throw new Exception($"Multiple users with email {email.Quote()} found in Auth0");
        }

        return auth0Users.FirstOrDefault();
    }

    private async Task<bool> IsFederatedByEmailAsync(IManagementApiClient managementClient, string email) {
        var request = new GetConnectionsRequest();

        var pagination = new PaginationInfo(0, 100);

        var connections = await managementClient.Connections.GetAllAsync(request, pagination);

        foreach (var connection in connections) {
            try {
                if (connection.Options.domain_aliases is IEnumerable enumerable) {
                    foreach (var domainAlias in enumerable) {
                        if (email.EndsWith($"@{domainAlias}", StringComparison.InvariantCultureIgnoreCase)) {
                            return true;
                        }
                    }
                }
            } catch { }
        }

        return false;
    }

    private async Task SendPasswordResetEmailAsync(IManagementApiClient managementClient,
                                                   AuthenticationApiClient authClient,
                                                   string clientId,
                                                   string connectionName,
                                                   string email) {
        var isFederated = await IsFederatedByEmailAsync(managementClient, email);

        if (isFederated) {
            throw new Exception("Password reset emails cannot be sent for federated users");
        }

        var changePasswordRequest = new ChangePasswordRequest();
        changePasswordRequest.ClientId = clientId;
        changePasswordRequest.Connection = connectionName;
        changePasswordRequest.Email = email;

        await authClient.ChangePasswordAsync(changePasswordRequest);
    }

    private async Task<IManagementApiClient> GetManagementClientAsync(ClientType clientType) {
        if (!_managementClient.HasValue()) {
            _managementClient = await _clientFactory.GetManagementApiClientAsync(clientType);
        }

        return _managementClient;
    }

    private AuthenticationApiClient GetAuthenticationClient(ClientType clientType) {
        if (!_authClient.HasValue()) {
            _authClient = _clientFactory.GetAuthenticationApiClient(clientType);
        }

        return _authClient;
    }
}