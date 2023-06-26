using N3O.Umbraco.Json;
using SendGrid;
using System.Threading.Tasks;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public class TestApiClient : ApiClient, ITestApiClient {
    public TestApiClient(ISendGridClient client, IJsonProvider jsonProvider) : base(client, jsonProvider) { }
    
    public async Task<bool> ValidateApiKeyAsync() {
        try {
            await GetAsync("/scopes");

            return true;
        } catch {
            return false;
        }
    }
}