using N3O.Umbraco.Cloud.Lookups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class FoundPlatformsPage : PlatformsPage {
    [JsonConstructor]
    public FoundPlatformsPage(Guid id,
                              string path,
                              PublishedFileKind kind,
                              IReadOnlyDictionary<string, object> mergeModel,
                              string redirectUrl)
        : base(id, path, kind, mergeModel) {
        RedirectUrl = redirectUrl;
    }

    public FoundPlatformsPage(PlatformsPage platformsPage, string redirectUrl)
        : this(platformsPage.Id,
               platformsPage.Path,
               platformsPage.Kind,
               platformsPage.MergeModel,
               redirectUrl) { }
    
    public string RedirectUrl { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
        foreach (var item in base.GetAtomicValues()) {
            yield return item;
        }
        
        yield return RedirectUrl;
    }
}