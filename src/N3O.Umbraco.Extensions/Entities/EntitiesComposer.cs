using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Entities {
    public class EntitiesComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddTransient<IChangeFeedFactory, ChangeFeedFactory>();

            RegisterAll(t => t.ImplementsGenericInterface(typeof(IChangeFeed<>)),
                        t => builder.Services.AddTransient(GetChangeFeedType(t), t));
            
            builder.Components().Append<EntitiesMigrationsComponent>();
        }

        private Type GetChangeFeedType(Type type) {
            var entityType = type.GetParameterTypesForGenericInterface(typeof(IChangeFeed<>)).Single();

            return typeof(IChangeFeed<>).MakeGenericType(entityType);
        }
    }
}