using N3O.Umbraco.Cloud.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedPlatformsPage {
    public Uri Url { get; set; }
    public IEnumerable<PublishedFileInfo> MergeModels { get; set; }
    public Dictionary<string, string> MetaTags { get; set; }
}