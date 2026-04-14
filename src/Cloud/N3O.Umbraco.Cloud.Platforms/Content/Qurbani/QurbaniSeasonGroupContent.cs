using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Clients;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Qurbani.Settings.Season.Group.Alias)]
public class QurbaniSeasonGroupContent : QurbaniSeasonOfferContent<QurbaniSeasonGroupContent> {
    protected override void PopulateCartExtensions(QurbaniCartExtensionsReq extension) {
        extension.Group = Name;
    }
}
