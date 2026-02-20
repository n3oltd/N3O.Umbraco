using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using uSync.BackOffice;

namespace N3O.Umbraco.Sync;

public class SyncComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.AdduSync();
        
        builder.Services.Configure<FormOptions>(x => {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = int.MaxValue;
            x.MemoryBufferThreshold = int.MaxValue;
        });
    }
}
