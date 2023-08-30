using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing.Clients;

public class AuthorizationHandler : DelegatingHandler {
    private readonly string _token;

    public AuthorizationHandler(string token, HttpMessageHandler innerHandler = null) {
        _token = token;
        InnerHandler = innerHandler ?? new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken) {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        return await base.SendAsync(request, cancellationToken);
    }
}
