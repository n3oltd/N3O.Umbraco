using N3O.Umbraco.Authentication.Auth0.Lookups;
using System.Threading.Tasks;
using Auth0User = Auth0.ManagementApi.Models.User;

namespace N3O.Umbraco.Authentication.Auth0;

public interface IUserDirectory {
    Task<Auth0User> CreateUserIfNotExistsAsync(ClientType clientType,
                                               string clientId,
                                               string connectionName,
                                               string email,
                                               string firstName,
                                               string lastName,
                                               string password = null);
    
    Task<string> GetPasswordResetUrlAsync(ClientType clientType, string directoryId);
    Task<Auth0User> GetUserByEmailAsync(ClientType clientType, string email);
}