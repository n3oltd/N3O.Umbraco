using Flurl;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.DirectDebitUK.Clients;

public class AppendApiKeyToQueryStringHandler : DelegatingHandler {
    private readonly string _parameterName;
    private readonly string _parameterValue;

    public AppendApiKeyToQueryStringHandler(string parameterName, string parameterValue) {
        _parameterName = parameterName;
        _parameterValue = parameterValue;

        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken) {
        request.RequestUri = new Url(request.RequestUri).SetQueryParam(_parameterName, _parameterValue).ToUri();

        return await base.SendAsync(request, cancellationToken);
    }
}
