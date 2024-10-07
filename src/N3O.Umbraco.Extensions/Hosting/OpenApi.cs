using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Hosting;

namespace N3O.Umbraco.Extensions;

public static class OpenApi {
    public static bool IsEnabled() {
        if (Composer.WebHostEnvironment.IsProduction() == false ||
            EnvironmentSettings.GetValue("N3O_OpenApi").EqualsInvariant("enabled")) {
            return true;
        }

        return false;
    }
}
