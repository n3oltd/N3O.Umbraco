using N3O.Umbraco.Utilities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Client {
    public class AuthorizationHandler : DelegatingHandler {
        private readonly string _base64Credentials;

        public AuthorizationHandler(string clientId, string secret) {
            _base64Credentials = Base64.Encode($"{clientId}:{secret}");

            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     CancellationToken cancellationToken) {
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _base64Credentials);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}