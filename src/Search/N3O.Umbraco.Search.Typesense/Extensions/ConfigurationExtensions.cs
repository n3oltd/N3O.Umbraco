using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Search.Typesense.Extensions;

public static class ConfigurationExtensions {
    public static IConfigurationSection GetTypesenseSection(this IConfiguration configuration,
                                                            string key = null) {
        var configSection = configuration.GetSection(TypesenseConstants.Configuration.Section);

        if (key.HasValue()) {
            configSection = configSection.GetSection(key);
        }

        return configSection;
    }
}
