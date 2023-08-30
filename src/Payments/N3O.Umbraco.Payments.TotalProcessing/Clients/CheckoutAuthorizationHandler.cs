using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing.Clients;

public class CheckoutAuthorizationHandler : DelegatingHandler {
    private readonly string _token;

    public CheckoutAuthorizationHandler(string token, HttpMessageHandler innerHandler = null) {
        _token = token;
        InnerHandler = innerHandler ?? new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken) {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        var uriBuilder = new UriBuilder(request.RequestUri);
        request.RequestUri = uriBuilder.Uri;

        return await base.SendAsync(request, cancellationToken);
    }
}
