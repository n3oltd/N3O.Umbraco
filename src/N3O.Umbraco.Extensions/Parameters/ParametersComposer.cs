using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Parameters {
    public class ParametersComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddScoped<INamedParameterBinder, NamedParameterBinder>();
            builder.Services.AddScoped<IParameterDataSource, HttpParameterDataSource>();
        
            builder.Services.AddScoped<FluentParameters, FluentParameters>();
            builder.Services.AddScoped<IParameterDataSource>(serviceProvider => serviceProvider.GetRequiredService<FluentParameters>());
            builder.Services.AddScoped<IFluentParameters>(serviceProvider => serviceProvider.GetRequiredService<FluentParameters>());

            RegisterAll(t => t.ImplementsInterface<INamedParameter>(),
                        t => builder.Services.AddScoped(t, serviceProvider => {
                            var binder = serviceProvider.GetRequiredService<INamedParameterBinder>();

                            var namedParameter = binder.Bind(t);

                            return namedParameter;
                        }));
        }
    }
}