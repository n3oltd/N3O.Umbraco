using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Parameters {
    public class ParametersComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddScoped<IFluentParameters, FluentParameters>();
            builder.Services.AddTransient<IFluentParametersBuilder, FluentParametersBuilder>();
            builder.Services.AddScoped<INamedParameterBinder, NamedParameterBinder>();
            builder.Services.AddScoped<IParameterDataSource, HttpParameterDataSource>();
            builder.Services.AddScoped<IParameterDataSource>(serviceProvider => {
                return serviceProvider.GetRequiredService<IFluentParameters>();
            });

            RegisterAll(t => t.ImplementsInterface<INamedParameter>(),
                        t => builder.Services.AddScoped(t, serviceProvider => {
                            var binder = serviceProvider.GetRequiredService<INamedParameterBinder>();

                            var namedParameter = binder.Bind(t);

                            return namedParameter;
                        }));
        }
    }
}