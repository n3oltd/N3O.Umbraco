using Microsoft.Extensions.Options;
using System;
using Umbraco.Cms.Web.Common.Security;

namespace N3O.Umbraco.Authentication.Auth0;

public class Auth0MemberLoginProviderOptions : IConfigureNamedOptions<MemberExternalLoginProviderOptions> {
    private static readonly string SchemePrefix = global::Umbraco.Cms.Core.Constants.Security.MemberExternalAuthenticationTypePrefix;
    public static readonly string SchemeName = "Auth0";
    public static readonly string SchemaNameWithPrefix = SchemePrefix + SchemeName;
    
    private readonly Action<MemberExternalLoginProviderOptions> _configure;

    public Auth0MemberLoginProviderOptions(Action<MemberExternalLoginProviderOptions> configure = null) {
        _configure = configure;
    }

    public void Configure(string name, MemberExternalLoginProviderOptions options) {
        if (name != SchemaNameWithPrefix) {
            return;
        }

        Configure(options);
    }

    public void Configure(MemberExternalLoginProviderOptions options) {
        options.AutoLinkOptions = new MemberExternalSignInAutoLinkOptions(autoLinkExternalAccount: true,
                                                                          defaultIsApproved: true);
        options.AutoLinkOptions.OnAutoLinking = (_, _) => { };
        options.AutoLinkOptions.OnExternalLogin = (_, _) => true;

        _configure?.Invoke(options);
    }
}
