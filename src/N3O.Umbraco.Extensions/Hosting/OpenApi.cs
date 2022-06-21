using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using System;

namespace N3O.Umbraco.Extensions;

public static class OpenApi {
    public static bool IsEnabled() {
        if (Composer.WebHostEnvironment.IsProduction() == false ||
            Environment.GetEnvironmentVariable("N3O_OpenApi").EqualsInvariant("enabled")) {
            return true;
        }

        return false;
    }
}
