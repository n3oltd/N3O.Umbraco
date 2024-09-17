using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Smidge;
using Smidge.Cache;
using Smidge.FileProcessors;
using Smidge.Nuglify;
using Smidge.Options;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Bundling;

public class BundlingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddScoped<IBundler, Bundler>();
        
        RegisterAll(t => t.ImplementsInterface<IAssetBundle>(),
                    t => builder.Services.AddTransient(typeof(IAssetBundle), t));

        ConfigureSmidge(builder);
    }

    private void ConfigureSmidge(IUmbracoBuilder builder) {
        builder.Services.AddSmidgeNuglify();
        
        builder.Services.Configure<SmidgeOptions>(opt => {
            opt.CacheOptions = new SmidgeCacheOptions {
                UseInMemoryCache = true
            };
            
            opt.DefaultBundleOptions.DebugOptions.SetCacheBusterType<AppDomainLifetimeCacheBuster>();
            opt.DefaultBundleOptions.ProductionOptions.SetCacheBusterType<AppDomainLifetimeCacheBuster>();
            
            opt.PipelineFactory.OnCreateDefault = (_, pipeline) => {
                return pipeline.Replace<JsMinifier, NuglifyJs>(opt.PipelineFactory);
            };
        });
        
        builder.Services.Configure<UmbracoPipelineOptions>(opt => {
            var filter = new UmbracoPipelineFilter("Bundling");
            filter.Endpoints = app => app.UseSmidge();
            
            opt.AddFilter(filter);
        });
    }
}
