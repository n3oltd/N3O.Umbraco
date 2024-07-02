using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Authentication.Auth0.Options;
using N3O.Umbraco.Authentication.Extensions;
using N3O.Umbraco.Composing;
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

        RegisterAuth0AuthenticationClient(builder);
        RegisterAuth0ManagementClient(builder);
    }
    
    private void RegisterAuth0AuthenticationClient(IUmbracoBuilder builder) {
        builder.Services.AddScoped<AuthenticationApiClient>(serviceProvider => {
            var auth0BackOfficeOptions = serviceProvider.GetRequiredService<IOptions<Auth0BackOfficeAuthenticationOptions>>().Value;
            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
            var connection = new HttpClientAuthenticationConnection(httpClient);
            
            var auth0Client = new AuthenticationApiClient(auth0BackOfficeOptions.Domain, connection);
            
            return auth0Client;
        });
    }
    
    private void RegisterAuth0ManagementClient(IUmbracoBuilder builder) {
        builder.Services.AddScoped<IManagementApiClient>(serviceProvider => {
            var auth0Options = serviceProvider.GetRequiredService<IOptions<Auth0AuthenticationOptions>>().Value;

            var tokenAccessor = serviceProvider.GetRequiredService<Auth0M2MTokenAccessor>();
            var token = tokenAccessor.GetTokenAsync(auth0Options.Management, auth0Options.Management.ApiIdentifier)
                                     .GetAwaiter()
                                     .GetResult();
            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();

            var connection = new HttpClientManagementConnection(httpClient);

            var auth0Client = new ManagementApiClient(token, auth0Options.Management.Domain, connection);

            return auth0Client;
        });
    }
}
