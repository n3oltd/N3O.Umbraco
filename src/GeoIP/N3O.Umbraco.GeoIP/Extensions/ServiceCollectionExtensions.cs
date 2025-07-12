using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Context;

namespace N3O.Umbraco.GeoIP.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection UseGeoIPDefaultCurrencyProvider(this IServiceCollection services) {
        services.AddTransient<IDefaultCurrencyProvider, GeoIPDefaultCurrencyProvider>();

        return services;
    }

}