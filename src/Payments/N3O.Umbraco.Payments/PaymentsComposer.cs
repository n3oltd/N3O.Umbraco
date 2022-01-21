using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Testing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments {
    public class PaymentsComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<IPaymentsScope, PaymentsScope>();
            //builder.Services.AddSingleton<IPaymentsScope, TestPaymentScope>();
            builder.Services.AddSingleton<TestPaymentsFlow>();

            builder.Services.AddOpenApiDocument(PaymentsConstants.ApiName);
        }
    }
}