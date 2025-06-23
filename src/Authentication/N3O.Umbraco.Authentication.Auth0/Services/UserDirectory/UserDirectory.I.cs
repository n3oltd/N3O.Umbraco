using N3O.Umbraco.Authentication.Auth0.Lookups;
using System.Threading.Tasks;
using Auth0User = Auth0.ManagementApi.Models.User;

namespace N3O.Umbraco.Authentication.Auth0;

public interface IUserDirectory {
    Task<Auth0User> CreateUserIfNotExistsAsync(UmbracoAuthType umbracoAuthType,
                                               string clientId,
                                               string connectionName,
                                               bool passwordless,
                                               string email,
                                               string firstName,
                                               string lastName,
                                               string password = null);
    
    Task<string> GetPasswordResetUrlAsync(UmbracoAuthType umbracoAuthType, string directoryId);
    Task<Auth0User> GetUserByEmailAsync(UmbracoAuthType umbracoAuthType, string email);
}