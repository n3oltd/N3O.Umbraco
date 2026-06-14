using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.UIBuilder.Extensions;

namespace N3O.Umbraco.UIBuilder;

public class UIBuilderComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        // UIBuilder auto-discovers all IConfigurator implementations from loaded assemblies.
        // Do NOT manually iterate and call them here — that causes every configurator to run
        // twice (once by UIBuilder internally, once by this loop), resulting in duplicate
        // section/dashboard/collection registration errors.
        builder.AddUIBuilder(_ => { });
    }
}
