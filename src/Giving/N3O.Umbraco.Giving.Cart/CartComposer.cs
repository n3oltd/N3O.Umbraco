using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart.Hosting;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Giving.Cart {
    public class CartComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<ICartAccessor, CartAccessor>();
            builder.Services.AddScoped<CartCookieMiddleware, CartCookieMiddleware>();
            builder.Services.AddTransient<ICartIdAccessor, CartIdAccessor>();
            builder.Services.AddTransient<ICartValidator, CartValidator>();

            builder.Services.AddOpenApiDocument(CartConstants.ApiName);

            builder.Services.Configure<UmbracoPipelineOptions>(options => {
                var filter = new UmbracoPipelineFilter("Cart");
                filter.PostPipeline = app => app.UseMiddleware<CartCookieMiddleware>();
            
                options.AddFilter(filter);
            });
        }
    }
}
