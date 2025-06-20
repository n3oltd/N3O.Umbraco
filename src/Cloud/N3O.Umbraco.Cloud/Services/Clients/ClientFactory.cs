using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Authentication.Auth0;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

public class ClientFactory<T> {
    private static readonly string BaseUrl = nameof(BaseUrl);
    private const int RetryAttempts = 4;

    private readonly BearerTokenAccessor _bearerTokenAccessor;
    private readonly ICloudUrl _cloudUrl;
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private readonly IUserDirectoryIdAccessor _userDirectoryIdAccessor;
    private readonly ILogger<ServiceClient<T>> _logger;
    private readonly IJsonProvider _jsonProvider;

    public ClientFactory(BearerTokenAccessor bearerTokenAccessor,
                         ICloudUrl cloudUrl,
                         ISubscriptionAccessor subscriptionAccessor,
                         IUserDirectoryIdAccessor userDirectoryIdAccessor,
                         ILogger<ServiceClient<T>> logger,
                         IJsonProvider jsonProvider) {
        _bearerTokenAccessor = bearerTokenAccessor;
        _cloudUrl = cloudUrl;
        _subscriptionAccessor = subscriptionAccessor;
        _userDirectoryIdAccessor = userDirectoryIdAccessor;
        _logger = logger;
        _jsonProvider = jsonProvider;
    }

    public async Task<ServiceClient<T>> CreateAsync(ClientType clientType, string onBehalfOf = null) {
        onBehalfOf ??= await _userDirectoryIdAccessor.GetIdAsync(clientType);

        var httpClient = await GetHttpClientAsync(onBehalfOf);

        var client = (T) Activator.CreateInstance(typeof(T), httpClient);

        var baseUrl = _cloudUrl.ForApi((string) typeof(T).GetProperty(BaseUrl).GetValue(client));

        _jsonProvider.ApplySettings((JsonSerializerSettings) client.GetPropertyInfo("JsonSerializerSettings").GetValue(client));

        return new ServiceClient<T>(client, _jsonProvider, _logger);
    }

    private async Task<HttpClient> GetHttpClientAsync(string onBehalfOf) {
        var transientErrorPolicyHandler = GetTransientErrorPolicyHttpMessageHandler();
        transientErrorPolicyHandler.InnerHandler = new HttpClientHandler();

        var subscription = _subscriptionAccessor.GetSubscription();
        var bearerToken = await _bearerTokenAccessor.GetAsync(ClientTypes.BackOffice);
        var cloudApiHandler = new CloudApiHandler(subscription.Id,
                                                  bearerToken,
                                                  onBehalfOf,
                                                  transientErrorPolicyHandler);

        var httpClient = new HttpClient(cloudApiHandler);

        httpClient.Timeout = TimeSpan.FromMinutes(30);

        return httpClient;
    }

    private PolicyHttpMessageHandler GetTransientErrorPolicyHttpMessageHandler() {
        var policy = HttpPolicyExtensions.HandleTransientHttpError()
                                         .WaitAndRetryAsync(RetryAttempts,
                                                            retryAttempt => TimeSpan.FromSeconds(CloudConstants.Clients.HttpRetry.RetryIntervals[retryAttempt]));

        var handler = new PolicyHttpMessageHandler(policy);

        return handler;
    }
}