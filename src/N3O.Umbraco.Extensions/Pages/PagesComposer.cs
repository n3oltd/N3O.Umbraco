using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Pages {
    public class PagesComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            RegisterAll(t => t.ImplementsInterface<IPageModule>(),
                        t => builder.Services.AddTransient(typeof(IPageModule), t));

            builder.Services.AddTransient<IPagePipeline, PagePipeline>();
        }
    }
}
