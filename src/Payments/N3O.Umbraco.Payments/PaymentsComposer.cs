using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;

namespace N3O.Umbraco.Payments {
    public class PaymentsComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<IPaymentsScope, PaymentsScope>();
            builder.Services.AddOpenApiDocument(PaymentsConstants.ApiName);
            
            var types = OurAssemblies.GetTypes(t => t.InheritsGenericClass(typeof(PaymentMethodDataEntryConfiguration<,>)) &&
                                                    !t.IsAbstract);
            foreach (var type in types) {
                builder.Services.AddTransient(type);
            }
        }
    }
}