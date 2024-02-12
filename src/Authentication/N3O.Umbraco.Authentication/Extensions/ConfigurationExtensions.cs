using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Authentication.Extensions;

public static class ConfigurationExtensions {
    public static IConfigurationSection GetAuthenticationSection(this IConfiguration configuration,
                                                                 string type,
                                                                 string key = null) {
        var configSection = configuration.GetSection(AuthenticationConstants.Configuration.Section).GetSection(type);

        if (key.HasValue()) {
            configSection = configSection.GetSection(key);
        }

        return configSection;
    }
    
    public static IConfigurationSection GetBackOfficeAuthenticationSection(this IConfiguration configuration,
                                                                           string key) {
        return GetAuthenticationSection(configuration, "BackOffice", key);
    }
    
    public static IConfigurationSection GetMembersAuthenticationSection(this IConfiguration configuration,
                                                                        string key) {
        return GetAuthenticationSection(configuration, "Members", key);
    }
}
