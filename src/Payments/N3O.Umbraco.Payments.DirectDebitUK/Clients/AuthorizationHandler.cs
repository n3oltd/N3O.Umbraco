using Flurl;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.DirectDebitUK.Clients;

public class AuthorizationHandler : DelegatingHandler {
    private readonly string _validationApiKey;

    public AuthorizationHandler(string validationApiKey) {
        _validationApiKey = validationApiKey;

        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken) {
        request.RequestUri = new Url(request.RequestUri).SetQueryParam("key", _validationApiKey).ToUri();

        return await base.SendAsync(request, cancellationToken);
    }
}
