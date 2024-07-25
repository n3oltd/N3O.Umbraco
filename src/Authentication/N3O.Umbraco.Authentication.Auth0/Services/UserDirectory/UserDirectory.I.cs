using Auth0.ManagementApi.Models;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication.Auth0;

public interface IUserDirectory {
    Task CreateUserIfNotExistsAsync(ClientType clientType,
                                    string clientId,
                                    string connectionName,
                                    string email,
                                    string firstName,
                                    string lastName,
                                    string password = null);
    
    Task<User> GetUserByEmailAsync(ClientType clientType, string email);
}