using System.Threading.Tasks;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public interface ITestApiClient : IApiClient {
    Task<bool> ValidateApiKeyAsync();
}