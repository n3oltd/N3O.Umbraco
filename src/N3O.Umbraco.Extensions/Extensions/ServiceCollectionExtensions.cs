using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Attributes;
using System.Reflection;

namespace N3O.Umbraco.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddOpenApiDocument(this IServiceCollection services, string name) {
            services.AddOpenApiDocument(opt => {
                opt.Title = name;
                opt.DocumentName = name;

                opt.AddOperationFilter(ctx => {
                    if (name.EqualsInvariant(ctx.ControllerType.GetCustomAttribute<ApiDocumentAttribute>()?.ApiName)) {
                        return true;
                    } else {
                        return false;
                    }
                });
            });

            return services;
        }
    }
}
