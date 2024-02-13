using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Authentication.Auth0.Clients;
using N3O.Umbraco.Authentication.Auth0.Options;
using N3O.Umbraco.Authentication.Extensions;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Json;
using Refit;
using System.Net.Http;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Authentication.Auth0;

public class Auth0AuthenticationComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.Configure<Auth0AuthenticationOptions>(builder.Config.GetAuthenticationSection(Auth0AuthenticationConstants.Configuration.Section));
        builder.Services.Configure<Auth0BackOfficeAuthenticationOptions>(builder.Config.GetBackOfficeAuthenticationSection(Auth0AuthenticationConstants.Configuration.Section));
        builder.Services.Configure<Auth0MemberAuthenticationOptions>(builder.Config.GetMembersAuthenticationSection(Auth0AuthenticationConstants.Configuration.Section));
        
        builder.Services.AddScoped<Auth0M2MTokenAccessor>();
        builder.Services.AddScoped<BearerTokenAccessor>();
        builder.Services.AddTransient<IUserDirectory, UserDirectory>();
        
        builder.Services.AddTransient<IAuth0M2MClient>(serviceProvider => {
            var auth0Options = serviceProvider.GetRequiredService<IOptions<Auth0AuthenticationOptions>>().Value;
            var jsonProvider = serviceProvider.GetRequiredService<IJsonProvider>();
            var jsonSettings = jsonProvider.GetSettings();

            var refitSettings = new RefitSettings();
            jsonSettings.ContractResolver = new SnakeCasePropertyNamesContractResolver();
            refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer(jsonSettings);

            var apiClient = RestService.For<IAuth0M2MClient>($"https://{auth0Options.M2M.Domain}/", refitSettings);

            return apiClient;
        });

        RegisterAuth0AuthenticationClient(builder);
        RegisterAuth0ManagementClient(builder);
    }
    
    private void RegisterAuth0AuthenticationClient(IUmbracoBuilder builder) {
        builder.Services.AddScoped<AuthenticationApiClient>(serviceProvider => {
            var auth0BackOfficeOptions = serviceProvider.GetRequiredService<IOptions<Auth0BackOfficeAuthenticationOptions>>().Value;
            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
            var connection = new HttpClientAuthenticationConnection(httpClient);
            
            var auth0Client = new AuthenticationApiClient($"https://{auth0BackOfficeOptions.Domain}", connection);
            
            return auth0Client;
        });
    }
    
    private void RegisterAuth0ManagementClient(IUmbracoBuilder builder) {
        builder.Services.AddScoped<IManagementApiClient>(serviceProvider => {
            var auth0Options = serviceProvider.GetRequiredService<IOptions<Auth0AuthenticationOptions>>().Value;

            var tokenAccessor = serviceProvider.GetRequiredService<Auth0M2MTokenAccessor>();
            var token = tokenAccessor.GetTokenAsync(auth0Options.Management.ApiIdentifier,
                                                    auth0Options.Management.ClientId,
                                                    auth0Options.Management.ClientSecret)
                                     .GetAwaiter()
                                     .GetResult();
            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();

            var connection = new HttpClientManagementConnection(httpClient);

            var auth0Client = new ManagementApiClient(token, auth0Options.Management.Domain, connection);

            return auth0Client;
        });
    }
}
