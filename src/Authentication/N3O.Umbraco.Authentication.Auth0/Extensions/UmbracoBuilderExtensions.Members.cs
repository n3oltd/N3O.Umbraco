using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using N3O.Umbraco.Authentication.Extensions;
using System;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Extensions;

namespace N3O.Umbraco.Authentication.Auth0.Extensions {
    public static partial class UmbracoBuilderExtensions {
        public static IUmbracoBuilder AddAuth0MemberExternalLogins(this IUmbracoBuilder builder,
                                                                   Action<MemberExternalLoginProviderOptions> configure = null) {
            if (configure != null) {
                builder.Services.AddSingleton(configure);
            }

            builder.Services.ConfigureOptions<Auth0MemberLoginProviderOptions>();

            builder.AddMemberExternalLogins(logins => {
                logins.AddMemberLogin(
                    backOfficeAuthenticationBuilder => {
                        var auth0Settings = builder.Config.GetMembersAuthenticationSection(Auth0AuthenticationConstants.Configuration.Section);
                        
                        backOfficeAuthenticationBuilder.AddOpenIdConnect(
                            backOfficeAuthenticationBuilder.SchemeForMembers(Auth0MemberLoginProviderOptions.SchemeName), opt => {
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

                                opt.Scope.Add("openid");
                                opt.Scope.Add("email");
                                opt.Scope.Add("roles");
                            });
                    });
            });

            return builder;
        }
    }
}