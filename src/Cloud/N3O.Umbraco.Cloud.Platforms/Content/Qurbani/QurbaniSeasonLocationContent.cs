using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Clients;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Qurbani.Settings.Season.Location.Alias)]
public class QurbaniSeasonLocationContent : QurbaniSeasonOfferContent<QurbaniSeasonLocationContent> {
    protected override void PopulateCartExtensions(QurbaniCartExtensionsReq extension) {
        extension.Location = Name;
    }
}
