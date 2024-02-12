using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using N3O.Umbraco.Extensions;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Auth0User = Auth0.ManagementApi.Models.User;

namespace N3O.Umbraco.Authentication.Auth0;

public class UserDirectory : IUserDirectory {
    private readonly IManagementApiClient _managementClient;
    private readonly AuthenticationApiClient _authClient;

    public UserDirectory(IManagementApiClient managementClient, AuthenticationApiClient authClient) {
        _managementClient = managementClient;
        _authClient = authClient;
    }

    public async Task CreateUserIfNotExistsAsync(string clientId,
                                                 string connectionName,
                                                 string email,
                                                 string firstName,
                                                 string lastName,
                                                 string password = null) {
        var isFederated = await IsFederatedByEmailAsync(email);

        if (isFederated) {
            return;
        }

        var user = await GetDirectoryUserByEmailAsync(email);

        if (user == null) {
            if (password == null) {
                password = PasswordGenerator.Generate(10,
                                                      PasswordCharacters.UppercaseLetters |
                                                      PasswordCharacters.LowercaseLetters |
                                                      PasswordCharacters.AlphaNumeric);
            }

            await CreateDirectoryUserAsync(connectionName, email, firstName, lastName, password);

            await SendPasswordResetEmailAsync(clientId, connectionName, email);
        }
    }
    
    public async Task<Auth0User> GetUserByEmailAsync(string email) {
        var auth0Users = await _managementClient.Users.GetUsersByEmailAsync(email.ToLowerInvariant());

        if (auth0Users.Count > 1) {
            throw new Exception($"Multiple users with email {email.Quote()} found in Auth0");
        }

        return auth0Users.SingleOrDefault();
    }

    private async Task<Auth0User> CreateDirectoryUserAsync(string connectionName,
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

        var auth0User = await _managementClient.Users.CreateAsync(request);

        return auth0User;
    }

    private async Task<Auth0User> GetDirectoryUserByEmailAsync(string email) {
        var auth0Users = await _managementClient.Users.GetUsersByEmailAsync(email.ToLowerInvariant());

        if (auth0Users.Count > 1) {
            throw new Exception($"Multiple users with email {email.Quote()} found in Auth0");
        }

        return auth0Users.FirstOrDefault();
    }

    private async Task<bool> IsFederatedByEmailAsync(string email) {
        var request = new GetConnectionsRequest();

        var pagination = new PaginationInfo(0, 100);

        var connections = await _managementClient.Connections.GetAllAsync(request, pagination);

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

    private async Task SendPasswordResetEmailAsync(string clientId, string connectionName, string email) {
        var isFederated = await IsFederatedByEmailAsync(email);

        if (isFederated) {
            throw new Exception("Password reset emails cannot be sent for federated users");
        }

        var changePasswordRequest = new ChangePasswordRequest();
        changePasswordRequest.ClientId = clientId;
        changePasswordRequest.Connection = connectionName;
        changePasswordRequest.Email = email;

        await _authClient.ChangePasswordAsync(changePasswordRequest);
    }
}