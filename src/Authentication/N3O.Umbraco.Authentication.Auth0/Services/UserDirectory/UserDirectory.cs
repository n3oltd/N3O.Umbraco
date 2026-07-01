using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication.Auth0;

public class UserDirectory : IUserDirectory {
    private IManagementApiClient _managementClient;
    private AuthenticationApiClient _authClient;
    
    private readonly IAuth0ClientFactory _clientFactory;
    private readonly IUserDirectoryConnections _userDirectoryConnections;

    public UserDirectory(IAuth0ClientFactory clientFactory, IUserDirectoryConnections userDirectoryConnections) {
        _clientFactory = clientFactory;
        _userDirectoryConnections = userDirectoryConnections;
    }

    public async Task<Auth0User> CreateUserIfNotExistsAsync(UserDirectoryType userDirectoryType,
                                                            string clientId,
                                                            string connectionName,
                                                            bool passwordless,
                                                            string email,
                                                            string firstName,
                                                            string lastName,
                                                            string password = null) {
        var managementClient = await GetManagementClientAsync(userDirectoryType);

        if (passwordless) {
            return await GetOrCreatePasswordlessUserAsync(managementClient, userDirectoryType, connectionName, email, firstName, lastName);
        } else {
            return await GetOrCreatePasswordUserAsync(managementClient, userDirectoryType, clientId, connectionName, email, firstName, lastName);
        }
    }
    
    public async Task<string> GetPasswordResetUrlAsync(UserDirectoryType userDirectoryType, string directoryId) {
        var managementClient = await GetManagementClientAsync(userDirectoryType);
        
        var isFederated = await IsFederatedByIdAsync(userDirectoryType, directoryId);

        if (isFederated) {
            throw new Exception("Password reset emails cannot be sent for federated users");
        }

        var request = new ChangePasswordTicketRequestContent();
        request.UserId = directoryId;
        request.TtlSec = (int) TimeSpan.FromHours(1).TotalSeconds;

        var ticket = await managementClient.Tickets.ChangePasswordAsync(request);

        return ticket.Ticket;
    }
    
    public async Task<Auth0User> GetUserByEmailAsync(UserDirectoryType userDirectoryType, string email) {
        var managementClient = await GetManagementClientAsync(userDirectoryType);

        var user = await GetDirectoryUserByEmailAsync(managementClient, email);

        return await user.ToAuth0UserAsync(_userDirectoryConnections, userDirectoryType);
    }

    private async Task<Auth0User> GetOrCreatePasswordlessUserAsync(IManagementApiClient managementClient,
                                                                   UserDirectoryType userDirectoryType,
                                                                   string connectionName,
                                                                   string email,
                                                                   string firstName,
                                                                   string lastName) {
        var directoryUser = await GetDirectoryUserByEmailAsync(managementClient, email);
        var user = await directoryUser.ToAuth0UserAsync(_userDirectoryConnections, userDirectoryType);

        if (!user.HasValue() || user.Identities.None(x => x.Connection == connectionName)) {
            var createdUser = await CreateDirectoryUserAsync(managementClient, connectionName, email, firstName, lastName, password: null);
            
            user = await createdUser.ToAuth0UserAsync(_userDirectoryConnections, userDirectoryType);
        }

        return user;
    }

    private async Task<Auth0User> GetOrCreatePasswordUserAsync(IManagementApiClient managementClient,
                                                               UserDirectoryType userDirectoryType,
                                                               string clientId,
                                                               string connectionName,
                                                               string email,
                                                               string firstName,
                                                               string lastName,
                                                               string password = null) {
        var directoryUser = await GetDirectoryUserByEmailAsync(managementClient, email);
        var user = await directoryUser.ToAuth0UserAsync(_userDirectoryConnections, userDirectoryType);

        if (!user.HasValue() || user.Identities.None(x => x.Connection == connectionName)) {
            var isFederated = await _userDirectoryConnections.IsFederatedByEmailAsync(userDirectoryType, email);

            if (isFederated) {
                return null;
            }

            var authClient = GetAuthenticationClient(userDirectoryType);

            password ??= PasswordGenerator.Generate(10,
                                                    PasswordCharacters.UppercaseLetters |
                                                    PasswordCharacters.LowercaseLetters |
                                                    PasswordCharacters.AlphaNumeric);

            var createdUser = await CreateDirectoryUserAsync(managementClient, connectionName, email, firstName, lastName, password);
            user = await createdUser.ToAuth0UserAsync(_userDirectoryConnections, userDirectoryType);

            await SendPasswordResetEmailAsync(authClient, userDirectoryType, clientId, connectionName, email);
        }

        return user;
    }

    private async Task<CreateUserResponseContent> CreateDirectoryUserAsync(IManagementApiClient managementClient,
                                                                           string connectionName,
                                                                           string email,
                                                                           string firstName,
                                                                           string lastName,
                                                                           string password) {
        var request = new CreateUserRequestContent {
            Email = email.ToLowerInvariant(),
            Password = password,
            VerifyEmail = false,
            EmailVerified = true,
            GivenName = firstName,
            FamilyName = lastName,
            Name = $"{firstName} {lastName}".Trim(),
            Connection = connectionName
        };

        var auth0User = await managementClient.Users.CreateAsync(request);

        return auth0User;
    }

    private async Task<UserResponseSchema> GetDirectoryUserByEmailAsync(IManagementApiClient managementClient, string email) {
        var request = new ListUsersByEmailRequestParameters {
            Email = email.ToLowerInvariant()
        };

        var directoryUsers = await managementClient.Users.ListUsersByEmailAsync(request);
        var list = directoryUsers.OrEmpty().ToList();

        if (list.Count > 1) {
            throw new Exception($"Multiple users with email {email.Quote()} found in Auth0");
        }

        return list.FirstOrDefault();
    }
    
    private async Task<GetUserResponseContent> GetDirectoryUserByIdAsync(string directoryId, bool throwIfNotFound) {
            var directoryUser = await _managementClient.Users.GetAsync(directoryId, new GetUserRequestParameters());
    
            if (directoryUser == null && throwIfNotFound) {
                throw new Exception($"No Auth0 user found with ID {directoryId}");
            }
    
            return directoryUser;
        }

    private async Task<bool> IsFederatedByIdAsync(UserDirectoryType userDirectoryType, string directoryId) {
        var user = await GetDirectoryUserByIdAsync(directoryId, true);

        return await _userDirectoryConnections.IsFederatedByEmailAsync(userDirectoryType, user.Email);
    }

    private async Task SendPasswordResetEmailAsync(AuthenticationApiClient authClient,
                                                   UserDirectoryType userDirectoryType,
                                                   string clientId,
                                                   string connectionName,
                                                   string email) {
        var isFederated = await _userDirectoryConnections.IsFederatedByEmailAsync(userDirectoryType, email);

        if (isFederated) {
            throw new Exception("Password reset emails cannot be sent for federated users");
        }

        var changePasswordRequest = new ChangePasswordRequest();
        changePasswordRequest.ClientId = clientId;
        changePasswordRequest.Connection = connectionName;
        changePasswordRequest.Email = email;

        await authClient.ChangePasswordAsync(changePasswordRequest);
    }

    private async Task<IManagementApiClient> GetManagementClientAsync(UserDirectoryType userDirectoryType) {
        if (!_managementClient.HasValue()) {
            _managementClient = await _clientFactory.GetManagementApiClientAsync(userDirectoryType);
        }

        return _managementClient;
    }

    private AuthenticationApiClient GetAuthenticationClient(UserDirectoryType userDirectoryType) {
        if (!_authClient.HasValue()) {
            _authClient = _clientFactory.GetAuthenticationApiClient(userDirectoryType);
        }

        return _authClient;
    }
}
