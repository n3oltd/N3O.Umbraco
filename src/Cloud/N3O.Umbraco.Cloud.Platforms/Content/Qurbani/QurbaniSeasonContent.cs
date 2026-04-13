using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Qurbani.Settings.Season.Alias)]
public class QurbaniSeasonContent : UmbracoContent<QurbaniSeasonContent> {
    public string Name => Content().Name;
    public Guid Key => Content().Key;
    
    public bool ShowOnBehalfOf => GetValue(x => x.ShowOnBehalfOf);
}
