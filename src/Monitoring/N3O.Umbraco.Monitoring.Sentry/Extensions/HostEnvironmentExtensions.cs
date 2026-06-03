using Microsoft.Extensions.Hosting;

namespace N3O.Umbraco.Monitoring.Sentry.Extensions;

public static class HostEnvironmentExtensions {
    public static bool IsSentryEnabled(this IHostEnvironment hostEnvironment) {
        return hostEnvironment.IsProduction() || hostEnvironment.IsStaging();
    }
}