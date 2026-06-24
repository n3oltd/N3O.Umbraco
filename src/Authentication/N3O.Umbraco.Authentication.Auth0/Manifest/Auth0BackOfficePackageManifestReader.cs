using Microsoft.Extensions.Options;
using N3O.Umbraco.Authentication.Auth0.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

namespace N3O.Umbraco.Authentication.Auth0;

public class Auth0BackOfficePackageManifestReader : IPackageManifestReader {
    private readonly IOptions<Auth0BackOfficeAuthenticationOptions> _options;

    public Auth0BackOfficePackageManifestReader(IOptions<Auth0BackOfficeAuthenticationOptions> options) {
        _options = options;
    }

    public Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {
        var autoRedirect = _options.Value.AutoRedirect;

        var manifest = new PackageManifest {
            Name = "N3O.Umbraco.Authentication.Auth0",
            AllowPublicAccess = true,
            Extensions = [
                new {
                    type = "authProvider",
                    alias = "N3O.AuthProvider.Auth0",
                    name = "Auth0 Back-Office Auth Provider",
                    forProviderName = Auth0BackOfficeLoginProviderOptions.SchemaNameWithPrefix,
                    meta = new {
                        label = "Auth0",
                        defaultView = new {
                            icon = "icon-cloud"
                        },
                        behavior = new {
                            autoRedirect
                        }
                    }
                }
            ]
        };

        return Task.FromResult<IEnumerable<PackageManifest>>([manifest]);
    }
}
