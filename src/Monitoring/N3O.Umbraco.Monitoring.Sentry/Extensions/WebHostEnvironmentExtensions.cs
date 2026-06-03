using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace N3O.Umbraco.Monitoring.Sentry.Extensions;

public static class WebHostEnvironmentExtensions {
    public static bool IsSentryEnabled(this IWebHostEnvironment webHostEnvironment) {
        return webHostEnvironment.IsProduction() || webHostEnvironment.IsStaging();
    }
}