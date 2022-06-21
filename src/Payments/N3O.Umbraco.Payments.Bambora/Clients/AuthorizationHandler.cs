using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Client;

public class AuthorizationHandler : DelegatingHandler {
    private readonly string _base64Credentials;

    public AuthorizationHandler(string merchantId, string passcode) {
        var bytes = Encoding.UTF8.GetBytes($"{merchantId}:{passcode}");
        _base64Credentials = Convert.ToBase64String(bytes);

        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken) {
        request.Headers.Authorization = new AuthenticationHeaderValue("Passcode", _base64Credentials);

        return await base.SendAsync(request, cancellationToken);
    }
}
