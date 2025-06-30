using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication;

public interface ISignInManager {
    Task<string> GetPasswordResetUrlAsync();
    Task SignOutAsync(HttpContext httpContext);
}