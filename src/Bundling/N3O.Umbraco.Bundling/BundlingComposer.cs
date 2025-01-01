using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Bundling;

public class BundlingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddScoped<IBundler, Bundler>();
        
        RegisterAll(t => t.ImplementsInterface<IAssetBundle>(),
                    t => builder.Services.AddTransient(typeof(IAssetBundle), t));
    }
}
