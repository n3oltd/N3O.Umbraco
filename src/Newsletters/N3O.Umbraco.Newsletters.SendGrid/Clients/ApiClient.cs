using N3O.Umbraco.Json;
using N3O.Umbraco.Newsletters.SendGrid.Lookups;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public abstract class ApiClient : IApiClient {
    private readonly ISendGridClient _client;
    private readonly IJsonProvider _jsonProvider;

    protected ApiClient(ISendGridClient client, IJsonProvider jsonProvider) {
        _client = client;
        _jsonProvider = jsonProvider;
    }
    
    protected void RaiseApiError(ApiError error) {
        var eventArgs = new ApiErrorEventArgs(error);
        
        var handler = ApiError;
        handler?.Invoke(this, eventArgs);
    }
    
    protected async Task<TRes> DeleteAsync<TReq, TRes>(string urlPath, TReq req)
        where TReq : class
        where TRes : class {
        var res = await RequestAsync<TReq, TRes>(BaseClient.Method.GET , urlPath: urlPath, req);

        return res;
    }
    
    protected async Task DeleteAsync(string urlPath) {
        await DeleteAsync<None, None>(urlPath, None.Empty);
    }
    
    protected async Task GetAsync(string urlPath, CancellationToken cancellationToken = default) {
        await GetAsync<None>(urlPath, cancellationToken);
    }
    
    protected async Task<TRes> GetAsync<TRes>(string urlPath, CancellationToken cancellationToken = default)
        where TRes : class {
        var res = await RequestAsync<None, TRes>(BaseClient.Method.GET,
                                                 urlPath: urlPath,
                                                 cancellationToken: cancellationToken);

        return res;
    }
    
    protected async Task<TRes> PostAsync<TReq, TRes>(string urlPath, TReq req)
        where TReq : class
        where TRes : class {
        var res = await RequestAsync<TReq, TRes>(BaseClient.Method.GET , urlPath: urlPath, req);

        return res;
    }
    
    protected async Task<TRes> PutAsync<TReq, TRes>(string urlPath, TReq req)
        where TReq : class
        where TRes : class {
        var res = await RequestAsync<TReq, TRes>(BaseClient.Method.GET , urlPath: urlPath, req);

        return res;
    }
    
    protected async Task PutAsync<TReq>(string urlPath, TReq req) where TReq : class {
        await PutAsync<TReq, None>(urlPath, req);
    }

    protected async Task<Response> SendEmailAsync(SendGridMessage message,
                                                  CancellationToken cancellationToken = default) {
        var res = await _client.SendEmailAsync(message, cancellationToken);

        return res;
    }

    private async Task<TRes> RequestAsync<TReq, TRes>(BaseClient.Method method,
                                                      string urlPath,
                                                      TReq req = default,
                                                      CancellationToken cancellationToken = default)
        where TReq : class
        where TRes : class {
        var apiBody = req is None ? null : _jsonProvider.SerializeObject(req);

        var apiRes = await _client.RequestAsync(method: method,
                                                urlPath: urlPath,
                                                requestBody: apiBody,
                                                cancellationToken: cancellationToken);

        if (typeof(TRes) == typeof(None)) {
            return None.Empty as TRes;
        }
        
        var json = await apiRes.Body.ReadAsStringAsync(cancellationToken);

        var res = _jsonProvider.DeserializeObject<TRes>(json);

        return res;
    }
    

    public event EventHandler<ApiErrorEventArgs> ApiError;
}