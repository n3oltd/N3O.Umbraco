using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Utilities;

public class AddBearerAuthorizationHandler : DelegatingHandler {
    private readonly string _bearerToken;

    public AddBearerAuthorizationHandler(string bearerToken) {
        _bearerToken = bearerToken;

        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken) {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
