using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Authentication.Extensions;

public static class ConfigurationExtensions {
    public static IConfigurationSection GetAuthenticationSection(this IConfiguration configuration,
                                                                 string key = null) {
        var configSection = configuration.GetSection(AuthenticationConstants.Configuration.Section);

        if (key.HasValue()) {
            configSection = configSection.GetSection(key);
        }

        return configSection;
    }
    
    public static IConfigurationSection GetBackOfficeAuthenticationSection(this IConfiguration configuration) {
        return GetAuthenticationSection(configuration, "BackOffice");
    }
    
    public static IConfigurationSection GetMembersAuthenticationSection(this IConfiguration configuration) {
        return GetAuthenticationSection(configuration, "Members");
    }
}
