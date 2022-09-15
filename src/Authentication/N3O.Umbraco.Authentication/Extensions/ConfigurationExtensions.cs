using Microsoft.Extensions.Configuration;
using System;

namespace N3O.Umbraco.Authentication.Extensions;

public static class ConfigurationExtensions {
    public static IConfigurationSection GetBackOfficeAuthenticationSection(this IConfiguration configuration,
                                                                           string key) {
        return GetAuthenticationSection(configuration, "BackOffice", key);
    }
    
    public static IConfigurationSection GetMembersAuthenticationSection(this IConfiguration configuration,
                                                                        string key) {
        return GetAuthenticationSection(configuration, "Members", key);
    }
    
    private static IConfigurationSection GetAuthenticationSection(IConfiguration configuration,
                                                                  string type,
                                                                  string key) {
        return configuration.GetSection(AuthenticationConstants.Configuration.Section)
                            .GetSection(type)
                            .GetSection(key);
    }
    
    public static bool IsN3OInstance(this IConfiguration configuration) {
        return Environment.GetEnvironmentVariable("Headless__BaseUrl")!.Equals("https://n3oltd.n3o.cloud/");
    }
}
