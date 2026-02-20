using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Sync;

public class SyncComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<ISyncFilter>(),
                    t => builder.Services.AddTransient(typeof(ISyncFilter), t));
    }
}