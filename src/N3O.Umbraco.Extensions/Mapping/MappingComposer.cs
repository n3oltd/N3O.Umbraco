using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;

namespace N3O.Umbraco.Mapping;

public class MappingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        RegisterAll(t => t.Implements<IMapDefinition>(),
                    t => builder.WithCollectionBuilder<MapDefinitionCollectionBuilder>().Add(t));
    }
}
