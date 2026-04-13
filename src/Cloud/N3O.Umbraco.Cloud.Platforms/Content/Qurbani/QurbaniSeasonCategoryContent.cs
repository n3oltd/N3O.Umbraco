using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Qurbani.Settings.Season.Categories.Category.Alias)]
public class QurbaniSeasonCategoryContent : UmbracoContent<QurbaniSeasonCategoryContent> {
    public string Name => Content().Name;
    public Guid Key => Content().Key;

    public MediaWithCrops Icon => GetValue(x => x.Icon);
    public string Summary => GetValue(x => x.Summary);
}
