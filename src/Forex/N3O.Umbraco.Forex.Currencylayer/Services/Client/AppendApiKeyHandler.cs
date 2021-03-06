using Flurl;
using N3O.Umbraco.Content;
using N3O.Umbraco.Forex.Currencylayer.Content;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Forex.Currencylayer;

public class AppendApiKeyHandler : DelegatingHandler {
    private readonly IContentCache _contentCache;

    public AppendApiKeyHandler(HttpMessageHandler innerHandler, IContentCache contentCache) : base(innerHandler) {
        _contentCache = contentCache;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken) {
        var settings = _contentCache.Single<CurrencylayerSettingsContent>();
    
        var url = new Url(request.RequestUri);
        url.SetQueryParam("access_key", settings.ApiKey);

        request.RequestUri = url.ToUri();

        return await base.SendAsync(request, cancellationToken);
    }
}
