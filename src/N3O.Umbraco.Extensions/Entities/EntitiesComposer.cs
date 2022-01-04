using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Entities {
    public class EntitiesComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
            builder.Components().Append<EntitiesMigrationsComponent>();
        }
    }
}