// builder.ContentApps() removed in Umbraco 14 — Content Apps registered via umbraco-package.json in v17.
// TODO: Register the Platforms Preview App as a Bellissima "contentApp" extension in
//       App_Plugins/N3O.Umbraco.Cloud.Platforms.Preview/umbraco-package.json
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsContentAppsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) { }
}
