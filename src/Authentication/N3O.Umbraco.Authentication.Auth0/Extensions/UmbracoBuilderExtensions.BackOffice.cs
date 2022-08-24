using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using N3O.Umbraco.Authentication.Extensions;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.BackOffice.Security;
using Umbraco.Extensions;

namespace N3O.Umbraco.Authentication.Auth0.Extensions;

public static partial class UmbracoBuilderExtensions {
    public static IUmbracoBuilder AddAuth0BackOfficeAuthentication(this IUmbracoBuilder builder,
                                                                   Action<BackOfficeExternalLoginProviderOptions> configure = null) {
        if (configure != null) {
            builder.Services.AddSingleton(configure);
        }

        builder.Services.ConfigureOptions<Auth0BackOfficeLoginProviderOptions>();

        builder.AddBackOfficeExternalLogins(logins => {
            logins.AddBackOfficeLogin(
                backOfficeAuthenticationBuilder => {
                    var auth0Settings = builder.Config.GetBackOfficeAuthenticationSection(Auth0AuthenticationConstants.Configuration.Section);
                    
                    backOfficeAuthenticationBuilder.AddOpenIdConnect(
                        backOfficeAuthenticationBuilder.SchemeForBackOffice(Auth0BackOfficeLoginProviderOptions.SchemeName), opt => {
                            opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                            opt.Authority = auth0Settings[Auth0AuthenticationConstants.Configuration.Keys.Authority];
                            opt.ClientId = auth0Settings[Auth0AuthenticationConstants.Configuration.Keys.ClientId];
                            opt.ClientSecret = auth0Settings[Auth0AuthenticationConstants.Configuration.Keys.ClientSecret];
                            opt.ResponseType = OpenIdConnectResponseType.Code;
                            opt.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;
                            opt.TokenValidationParameters.NameClaimType = "name";
                            opt.TokenValidationParameters.RoleClaimType = "role";
                            opt.RequireHttpsMetadata = true;
                            opt.GetClaimsFromUserInfoEndpoint = true;
                            opt.SaveTokens = true;
                            opt.UsePkce = true;
                            opt.Events.OnRedirectToIdentityProvider = context => {
                                context.ProtocolMessage.SetParameter("audience", "https://n3o.ltd/karakoram");
                                return Task.CompletedTask;
                            };
                            
                            opt.Scope.Add("openid");
                            opt.Scope.Add("email");
                            opt.Scope.Add("roles");
                        });
                });
        });

        return builder;
    }
}
