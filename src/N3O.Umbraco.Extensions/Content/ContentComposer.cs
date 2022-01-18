using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Content {
    public class ContentComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            RegisterAll(t => t.ImplementsInterface<IContentVisibilityFilter>(),
                        t => builder.Services.AddScoped(typeof(IContentVisibilityFilter), t));
        
            builder.Services.AddSingleton<IContentCache, ContentCache>();
            builder.Services.AddSingleton<IContentHelper, ContentHelper>();
            builder.Services.AddSingleton<IContentLocator, ContentLocator>();
            builder.Services.AddTransient<IContentVisibility, ContentVisibility>();
            builder.Services.AddScoped<IPublishedContentHelper, PublishedContentHelper>();
        
            RegisterAll(t => t.ImplementsInterface<IContentValidator>(),
                        t => builder.Services.AddTransient(typeof(IContentValidator), t));
        }
    }
}
