using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using N3O.Umbraco.Authentication.Auth0.Options;
using N3O.Umbraco.Authentication.Extensions;
using System;
using System.Security.Claims;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.BackOffice.Security;
using Umbraco.Extensions;

namespace N3O.Umbraco.Authentication.Auth0.Extensions;

public static partial class UmbracoBuilderExtensions {
    public static IUmbracoBuilder AddAuth0BackOfficeAuthentication(this IUmbracoBuilder builder,
                                                                   Action<BackOfficeExternalLoginProviderOptions> configureLoginProviderOptions = null,
                                                                   Action<OpenIdConnectOptions> configureOpenIdConnectOptions = null) {
        if (configureLoginProviderOptions != null) {
            builder.Services.AddSingleton(configureLoginProviderOptions);
        }

        builder.Services.ConfigureOptions<Auth0BackOfficeLoginProviderOptions>();

        builder.AddBackOfficeExternalLogins(logins => {
            logins.AddBackOfficeLogin(
                backOfficeAuthenticationBuilder => {
                    var auth0Settings = builder.Config
                                               .GetBackOfficeAuthenticationSection()
                                               .Get<Auth0BackOfficeAuthenticationOptions>();
                    
                    backOfficeAuthenticationBuilder.AddOpenIdConnect(
                        backOfficeAuthenticationBuilder.SchemeForBackOffice(Auth0BackOfficeLoginProviderOptions.SchemeName), opt => {
                            opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                            opt.Authority = auth0Settings.Auth0.Login.Authority;
                            opt.ClientId = auth0Settings.Auth0.Login.ClientId;
                            opt.ClientSecret = auth0Settings.Auth0.Login.ClientSecret;
                            opt.ResponseType = OpenIdConnectResponseType.Code;
                            opt.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;
                            opt.CallbackPath = "/umbraco/signin-oidc";
                            opt.TokenValidationParameters.NameClaimType = "name";
                            opt.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
                            opt.RequireHttpsMetadata = true;
                            opt.GetClaimsFromUserInfoEndpoint = true;
                            opt.SaveTokens = true;
                            opt.UsePkce = true;
                            
                            configureOpenIdConnectOptions?.Invoke(opt);
                            
                            opt.Scope.Add("openid");
                            opt.Scope.Add("email");
                            opt.Scope.Add("profile");
                        });
                });
        });

        return builder;
    }
}
