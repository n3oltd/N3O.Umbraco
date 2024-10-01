using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication.Auth0;

public interface IUserDirectoryIdAccessor {
    Task<string> GetIdAsync();
}
