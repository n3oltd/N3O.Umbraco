using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.QueryFilters {
    public class QueryFiltersComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            RegisterAll(t => t.ImplementsGenericInterface(typeof(IContentQueryFilter<>)),
                        t => builder.Services.AddTransient(t, t));
            
            RegisterAll(t => t.ImplementsGenericInterface(typeof(IQueryFilter<,>)),
                        t => builder.Services.AddTransient(t, t));
        }
    }
}