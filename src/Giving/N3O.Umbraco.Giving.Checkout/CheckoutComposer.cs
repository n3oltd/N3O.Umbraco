using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Giving.Cart.Giving.Checkout {
    public class CheckoutComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddOpenApiDocument(CheckoutConstants.ApiName);
        }
    }
}