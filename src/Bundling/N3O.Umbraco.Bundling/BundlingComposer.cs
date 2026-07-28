using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Smidge;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Community.Smidge;

namespace N3O.Umbraco.Bundling;

public class BundlingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        // Umbraco 17 dropped Smidge from core. AddRuntimeMinifier() (Umbraco.Community.Smidge) re-registers
        // Smidge, including the SmidgeHelper this package's Bundler depends on. See n3oltd/work#3211.
        builder.AddRuntimeMinifier();

        // Core also used to add Smidge's request pipeline (which serves the generated bundle URLs); re-add it
        // here so consuming sites need no change. TODO verify placement on a running site (render check, #3211).
        builder.Services.Configure<UmbracoPipelineOptions>(options => {
            options.AddFilter(new UmbracoPipelineFilter(nameof(BundlingComposer)) {
                PostRouting = app => app.UseSmidge()
            });
        });

        builder.Services.AddScoped<IBundler, Bundler>();

        RegisterAll(t => t.ImplementsInterface<IAssetBundle>(),
                    t => builder.Services.AddTransient(typeof(IAssetBundle), t));
    }
}
