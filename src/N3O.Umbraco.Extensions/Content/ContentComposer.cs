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
            builder.Services.AddScoped<IContentHelper, ContentHelper>();
            builder.Services.AddSingleton<IContentLocator, ContentLocator>();
            builder.Services.AddScoped<IContentVisibility, ContentVisibility>();
            builder.Services.AddScoped<IPublishedContentHelper, PublishedContentHelper>();
        
            RegisterContentValidators<IContentValidator>(builder);
            RegisterContentValidators<INestedContentItemValidator>(builder);
        }

        private void RegisterContentValidators<T>(IUmbracoBuilder builder) {
            RegisterAll(t => t.ImplementsInterface<T>(),
                        t => builder.Services.AddTransient(typeof(T), t));
        }
    }
}
