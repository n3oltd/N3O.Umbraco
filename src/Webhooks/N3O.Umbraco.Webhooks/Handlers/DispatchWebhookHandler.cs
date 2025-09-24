using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Models;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Webhooks.Handlers;

public class DispatchWebhookHandler : IRequestHandler<DispatchWebhookCommand, DispatchWebhookReq, None> {
    private readonly IJsonProvider _jsonProvider;
    private readonly IHttpClientFactory _httpClientFactory;

    public DispatchWebhookHandler(IJsonProvider jsonProvider, IHttpClientFactory httpClientFactory) {
        _jsonProvider = jsonProvider;
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<None> Handle(DispatchWebhookCommand req, CancellationToken cancellationToken) {
        var httpClient = _httpClientFactory.CreateClient();
        var json = req.Model.Body.IfNotNull(x => _jsonProvider.SerializeObject(x)) ?? "";
        
        var request = new HttpRequestMessage(HttpMethod.Post, req.Model.Url);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        
        foreach (var header in req.Model.Headers.OrEmpty()) {
            request.Headers.Add(header.Key, header.Value);
        }
        
        var response = await httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
        
        return None.Empty;
    }
}
