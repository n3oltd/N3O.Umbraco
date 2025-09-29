using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsContentAppsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.ContentApps().Append<PlatformsPreviewApp>();
    }
}