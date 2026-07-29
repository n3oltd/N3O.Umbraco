using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Smidge;
using Smidge.Nuglify;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Community.Smidge;
using Umbraco.Extensions;

namespace N3O.Umbraco.Bundling;

public class BundlingComposer : Composer {
    // Umbraco 13 bound Smidge to this section, but Umbraco.Community.Smidge binds a top-level "smidge" section
    // instead, so bind it back to keep the sites' existing settings applying. n3oltd/work#3211
    private const string RuntimeMinificationSection = "Umbraco:CMS:RuntimeMinification";

    public override void Compose(IUmbracoBuilder builder) {
        // Umbraco 17 dropped Smidge from core; re-register it, incl. the SmidgeHelper the Bundler uses.
        // n3oltd/work#3211
        builder.AddRuntimeMinifier();

        // Umbraco 13 replaced Smidge's request helper because Brotli compression is very slow; the community
        // package ships the replacement but does not register it
        builder.Services.AddUnique<IRequestHelper, SmidgeRequestHelper>();

        builder.Services.Configure<RuntimeMinificationSettings>(builder.Config.GetSection(RuntimeMinificationSection));
        builder.Services.ConfigureOptions<OurSmidgeOptions>();

        RegisterMiddleware(builder);

        builder.Services.AddScoped<IBundler, Bundler>();

        RegisterAll(t => t.ImplementsInterface<IAssetBundle>(),
                    t => builder.Services.AddTransient(typeof(IAssetBundle), t));
    }

    private void RegisterMiddleware(IUmbracoBuilder builder) {
        builder.Services.Configure<UmbracoPipelineOptions>(opt => {
            var filter = new UmbracoPipelineFilter("Smidge");

            // UseSmidge and UseSmidgeNuglify are UseEndpoints calls, so they belong in the endpoints phase, which
            // is where Umbraco 13 added them (UseBackOfficeEndpoints); adding them any earlier puts a terminating
            // EndpointMiddleware in front of authentication and authorization
            filter.Endpoints = app => {
                var runtimeState = app.ApplicationServices.GetRequiredService<IRuntimeState>();

                if (runtimeState.Level == RuntimeLevel.Run) {
                    app.UseSmidge();
                    app.UseSmidgeNuglify();
                }
            };

            opt.AddFilter(filter);
        });
    }
}
