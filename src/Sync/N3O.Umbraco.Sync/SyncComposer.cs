using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using uSync.BackOffice;

namespace N3O.Umbraco.Sync; 

public class SyncComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.AdduSync();
    }
}