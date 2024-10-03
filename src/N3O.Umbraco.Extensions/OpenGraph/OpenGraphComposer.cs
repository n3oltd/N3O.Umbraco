using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.OpenGraph;

public class OpenGraphComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IOpenGraphBuilder, OpenGraphBuilder>();
        
        RegisterAll(t => t.ImplementsInterface<IOpenGraphProvider>(),
                    t => builder.Services.AddTransient(typeof(IOpenGraphProvider), t));
    }
}
