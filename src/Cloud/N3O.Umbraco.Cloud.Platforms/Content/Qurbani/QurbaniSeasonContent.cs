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
                                                                  .Where(x => x.ContentType.Alias.EqualsInvariant(PlatformsConstants.Qurbani.Settings.Season.Category.Alias))
                                                                  .As<QurbaniSeasonCategoryContent>()
                                                                  .ToList();
    
    public IEnumerable<QurbaniSeasonGroupContent> Groups => Content()
                                                           .Descendants()
                                                           .Where(x => x.ContentType.Alias.EqualsInvariant(PlatformsConstants.Qurbani.Settings.Season.Group.Alias))
                                                           .As<QurbaniSeasonGroupContent>()
                                                           .ToList();
    
    public IEnumerable<QurbaniSeasonLocationContent> Locations => Content()
                                                                 .Descendants()
                                                                 .Where(x => x.ContentType.Alias.EqualsInvariant(PlatformsConstants.Qurbani.Settings.Season.Location.Alias))
                                                                 .As<QurbaniSeasonLocationContent>()
                                                                 .ToList();
    
    public IEnumerable<QurbaniSeasonUpsellContent> Upsells => Content()
                                                             .Descendants()
                                                             .Where(x => x.ContentType.Alias.EqualsInvariant(PlatformsConstants.Qurbani.Settings.Season.Upsell.Alias))
                                                             .As<QurbaniSeasonUpsellContent>()
                                                             .ToList();
}
