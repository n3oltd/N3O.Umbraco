using Auth0.ManagementApi;
using System.Net.Http;

namespace N3O.Umbraco.Authentication.Auth0;

internal static class ManagementClientBuilder {
    public static IManagementApiClient Build(IHttpClientFactory httpClientFactory,
                                             ITokenProvider tokenProvider,
                                             string domain) {
        var httpClient = httpClientFactory.CreateClient(AuthenticationConstants.ManagementApiName);

        return new ManagementClient(new ManagementClientOptions {
            Domain = domain,
            TokenProvider = tokenProvider,
            HttpClient = httpClient,
            MaxRetries = 0
        });
    }
}