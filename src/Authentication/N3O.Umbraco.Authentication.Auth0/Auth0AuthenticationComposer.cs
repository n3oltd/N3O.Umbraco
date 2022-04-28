using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Authentication.Auth0.Options;
using N3O.Umbraco.Authentication.Extensions;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Authentication.Auth0 {
    public class Auth0AuthenticationComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.Configure<Auth0BackOfficeAuthenticationOptions>(builder.Config.GetBackOfficeAuthenticationSection(Auth0AuthenticationConstants.Configuration.Section));
            builder.Services.Configure<Auth0MemberAuthenticationOptions>(builder.Config.GetMembersAuthenticationSection(Auth0AuthenticationConstants.Configuration.Section));
        }
    }
}