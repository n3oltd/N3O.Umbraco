using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Qurbani.Settings.Season.Alias)]
public class QurbaniSeasonContent : UmbracoContent<QurbaniSeasonContent> {
    public string Name => Content().Name;
    public Guid Key => Content().Key;

    public bool ShowOnBehalfOf => GetValue(x => x.ShowOnBehalfOf);
    public IEnumerable<QurbaniSeasonCategoryContent> Categories => Content()
                                                                  .Descendants()
                                                                  .Where(x => x.ContentType.Alias.EqualsInvariant(PlatformsConstants.Qurbani.Settings.Season.Categories.Category.Alias))
                                                                  .As<QurbaniSeasonCategoryContent>();
    public IEnumerable<QurbaniSeasonGroupContent> Groups => Content()
                                                           .Descendants()
                                                           .Where(x => x.ContentType.Alias.EqualsInvariant(PlatformsConstants.Qurbani.Settings.Season.Groups.Group.Alias))
                                                           .As<QurbaniSeasonGroupContent>();
    public IEnumerable<QurbaniSeasonLocationContent> Locations => Content()
                                                                 .Descendants()
                                                                 .Where(x => x.ContentType.Alias.EqualsInvariant(PlatformsConstants.Qurbani.Settings.Season.Locations.Location.Alias))
                                                                 .As<QurbaniSeasonLocationContent>();
    public IEnumerable<QurbaniSeasonUpsellContent> Upsells => Content()
                                                             .Descendants()
                                                             .Where(x => x.ContentType.Alias.EqualsInvariant(PlatformsConstants.Qurbani.Settings.Season.Upsells.Upsell.Alias))
                                                             .As<QurbaniSeasonUpsellContent>();
}
