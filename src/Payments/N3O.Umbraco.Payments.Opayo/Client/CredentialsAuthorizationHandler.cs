using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Client {
    public class CredentialsAuthorizationHandler : DelegatingHandler {
        private readonly string _base64Credentials;

        public CredentialsAuthorizationHandler(string integrationKey, string integrationPassword) {
            var bytes = Encoding.UTF8.GetBytes($"{integrationKey}:{integrationPassword}");
            _base64Credentials = Convert.ToBase64String(bytes);

            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     CancellationToken cancellationToken) {
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _base64Credentials);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}