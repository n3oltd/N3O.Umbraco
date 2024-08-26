using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Authentication.Auth0;
using N3O.Umbraco.Engage.Models;
using N3O.Umbraco.Engage.Security;
using N3O.Umbraco.Engage.Services;
using N3O.Umbraco.Extensions;
using Flurl;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using Polly;
using Polly.Extensions.Http;
using N3O.Umbraco.Engage.Constants;

namespace N3O.Umbraco.Engage.Clients;

public class ClientFactory<T> {
    private const int RetryAttempts = 4;
    private const string BaseUrl = nameof(BaseUrl);

    private readonly BearerTokenAccessor _bearerTokenAccessor;
    private readonly CloudUrlAccessor _cloudUrlAccessor;
    private readonly IUserDirectoryIdAccessor _userDirectoryIdAccessor;
    private readonly ILogger<ServiceClient<T>> _logger;

    public ClientFactory(BearerTokenAccessor bearerTokenAccessor,
                         CloudUrlAccessor cloudUrlAccessor,
                         IUserDirectoryIdAccessor userDirectoryIdAccessor,
                         ILogger<ServiceClient<T>> logger) {
        _bearerTokenAccessor = bearerTokenAccessor;
        _cloudUrlAccessor = cloudUrlAccessor;
        _userDirectoryIdAccessor = userDirectoryIdAccessor;
        _logger = logger;
    }

    public async Task<ServiceClient<T>> CreateAsync(SubscriptionInfo subscription, string onBehalfOf = null) {
        onBehalfOf ??= await _userDirectoryIdAccessor.GetIdAsync();

        var httpClient = await GetHttpClientAsync(onBehalfOf);

        var client = (T) Activator.CreateInstance(typeof(T), httpClient);

        var baseUrl = new Url(_cloudUrlAccessor.Get());
        baseUrl.AppendPathSegment(typeof(T).GetProperty(BaseUrl).GetValue(client));

        client.SetPropertyValue(BaseUrl, baseUrl.ToString().Replace("eu1/api", $"{subscription.Region.Slug}/api"));

        return new ServiceClient<T>(client, _logger, subscription.Id);
    }

    private async Task<HttpClient> GetHttpClientAsync(string onBehalfOf) {
        var transientErrorPolicyHandler = GetTransientErrorPolicyHttpMessageHandler();
        transientErrorPolicyHandler.InnerHandler = new HttpClientHandler();

        var bearerToken = await _bearerTokenAccessor.GetAsync(ClientTypes.Members);
        var authHandler = new AuthorizationHandler(bearerToken, onBehalfOf, transientErrorPolicyHandler);

        var httpClient = new HttpClient(authHandler);

        httpClient.Timeout = TimeSpan.FromMinutes(30);

        return httpClient;
    }

    private PolicyHttpMessageHandler GetTransientErrorPolicyHttpMessageHandler() {
        var policy = HttpPolicyExtensions.HandleTransientHttpError()
                                         .WaitAndRetryAsync(RetryAttempts,
                                                            retryAttempt => TimeSpan.FromSeconds(ClientConstants.HttpRetry.RetryIntervals[retryAttempt]));

        var handler = new PolicyHttpMessageHandler(policy);

        return handler;
    }
}