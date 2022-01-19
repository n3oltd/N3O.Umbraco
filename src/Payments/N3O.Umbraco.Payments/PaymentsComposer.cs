using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments {
    public class PaymentsComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<IPaymentsScope, PaymentsScope>();

            builder.Services.AddOpenApiDocument(PaymentsConstants.ApiName);
        }
    }
}