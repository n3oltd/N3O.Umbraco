using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Website.Controllers;

namespace N3O.Umbraco.Hosting {
    public class HostingComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.Configure<UmbracoRenderingDefaultsOptions>(c => {
                c.DefaultControllerType = typeof(PageController);
            });
        
            builder.Services.AddTransient<IConfigureOptions<MvcOptions>, OurMvcJsonFormatterOptions>();
            builder.Services.AddTransient<IConfigureOptions<MvcOptions>, OurCacheProfileOptions>();
            builder.Services.AddScoped<IActionLinkGenerator, ActionLinkGenerator>();
        }
    }
}
