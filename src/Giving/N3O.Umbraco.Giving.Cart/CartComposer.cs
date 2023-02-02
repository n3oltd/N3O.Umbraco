using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Giving.Cart;

public class CartComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<ICartAccessor, CartAccessor>();
        builder.Services.AddTransient<ICartIdAccessor, CartIdAccessor>();
        builder.Services.AddTransient<ICartValidator, CartValidator>();
        builder.Services.AddTransient<UpsellService>();

        builder.Services.AddOpenApiDocument(CartConstants.ApiName);
        builder.Services.AddOpenApiDocument(UpsellConstants.ApiName);
    }
}
