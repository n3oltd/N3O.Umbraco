using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Sync.Extensions;

public class SyncExtensionsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<IDataSyncConsumer>(),
                    t => builder.Services.AddTransient(typeof(IDataSyncConsumer), t));
        
        RegisterAll(t => t.ImplementsInterface<IDataSyncProducer>(),
                    t => builder.Services.AddTransient(typeof(IDataSyncProducer), t));
    }
}