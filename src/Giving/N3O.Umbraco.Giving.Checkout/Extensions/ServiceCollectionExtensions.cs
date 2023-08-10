using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Lookups;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout.Extensions; 

public static class ServiceCollectionExtensions {
    public static IServiceCollection UseCustomCheckoutStages<T>(this IServiceCollection services)
        where T : LookupsCollection<CheckoutStage> {
        var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(CheckoutStagesCollection));

        if (serviceDescriptor != null) {
            services.Remove(serviceDescriptor);
        }

        services.AddTransient<ILookupsCollection<CheckoutStage>, T>();

        return services;
    }
}