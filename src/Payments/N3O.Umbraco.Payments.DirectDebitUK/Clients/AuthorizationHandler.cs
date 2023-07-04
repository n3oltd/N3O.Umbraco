using Flurl;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.DirectDebitUK.Clients;

public class AuthorizationHandler : DelegatingHandler {
    private readonly string _loqateApiKey;

    public AuthorizationHandler(string loqateApiKey) {
        _loqateApiKey = loqateApiKey;

        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken) {
        request.RequestUri = new Url(request.RequestUri).SetQueryParam("Key", _loqateApiKey).ToUri();

        return await base.SendAsync(request, cancellationToken);
    }
}
