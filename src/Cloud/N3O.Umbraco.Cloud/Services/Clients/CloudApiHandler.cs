using N3O.Umbraco.Cloud.Models;
using StackExchange.Profiling.Internal;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

public class CloudApiHandler : DelegatingHandler {
    private readonly SubscriptionDescriptor _subscriptionDescriptor;
    private readonly string _bearerToken;
    private readonly string _onBehalfOf;

    public CloudApiHandler(SubscriptionDescriptor subscriptionDescriptor,
                           string bearerToken,
                           string onBehalfOf,
                           HttpMessageHandler innerHandler) {
        _subscriptionDescriptor = subscriptionDescriptor;
        _bearerToken = bearerToken;
        _onBehalfOf = onBehalfOf;

        InnerHandler = innerHandler;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken) {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

        request.Headers.Add("N3O-Subscription-Id", _subscriptionDescriptor.Id);

        if (_onBehalfOf.HasValue()) {
            request.Headers.Add("N3O-OnBehalfOf", _onBehalfOf);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}