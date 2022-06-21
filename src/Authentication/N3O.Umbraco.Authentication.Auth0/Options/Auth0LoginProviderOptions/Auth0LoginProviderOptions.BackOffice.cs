using Microsoft.Extensions.Options;
using System;
using Umbraco.Cms.Web.BackOffice.Security;

namespace N3O.Umbraco.Authentication.Auth0;

public class Auth0BackOfficeLoginProviderOptions : IConfigureNamedOptions<BackOfficeExternalLoginProviderOptions> {
    private const string EditorGroupAlias = global::Umbraco.Cms.Core.Constants.Security.EditorGroupAlias;
    private const string SchemePrefix = global::Umbraco.Cms.Core.Constants.Security.BackOfficeExternalAuthenticationTypePrefix;
    public const string SchemeName = "Auth0";
    public const string SchemaNameWithPrefix = SchemePrefix + SchemeName;

    private readonly Action<BackOfficeExternalLoginProviderOptions> _configure;

    public Auth0BackOfficeLoginProviderOptions(Action<BackOfficeExternalLoginProviderOptions> configure = null) {
        _configure = configure;
    }

    public void Configure(string name, BackOfficeExternalLoginProviderOptions options) {
        if (name != SchemaNameWithPrefix) {
            return;
        }

        Configure(options);
    }

    public void Configure(BackOfficeExternalLoginProviderOptions options) {
        options.ButtonStyle = "btn-info";
        options.Icon = "fa fa-cloud";

        options.AutoLinkOptions = new ExternalSignInAutoLinkOptions(autoLinkExternalAccount: false,
                                                                    defaultUserGroups: new[] { EditorGroupAlias },
                                                                    defaultCulture: null,
                                                                    allowManualLinking: false);
        options.AutoLinkOptions.OnAutoLinking = (_, _) => { };
        options.AutoLinkOptions.OnExternalLogin = (_, _) => true;
        
        options.DenyLocalLogin = false;
        options.AutoRedirectLoginToExternalProvider = true;
        
        _configure?.Invoke(options);
    }
}
