using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Authentication.Auth0.Options;
using N3O.Umbraco.Authentication.Extensions;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Authentication.Auth0;

public class Auth0AuthenticationComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.Configure<AuthenticationOptions>(builder.Config.GetAuthenticationSection());
        builder.Services.Configure<Auth0BackOfficeAuthenticationOptions>(builder.Config.GetBackOfficeAuthenticationSection());
        builder.Services.Configure<Auth0MemberAuthenticationOptions>(builder.Config.GetMembersAuthenticationSection());
        
        builder.Services.AddScoped<Auth0M2MTokenAccessor>();
        builder.Services.AddScoped<Auth0TokenAccessor>();
        builder.Services.AddScoped<IAuth0ClientFactory, Auth0ClientFactory>();
        builder.Services.AddScoped<ISignInManager, Auth0MemberSignInManager>();
        builder.Services.AddTransient<IUserDirectory, UserDirectory>();
        builder.Services.AddScoped<IUserDirectoryIdAccessor, UserDirectoryIdAccessor>();
    }
}
