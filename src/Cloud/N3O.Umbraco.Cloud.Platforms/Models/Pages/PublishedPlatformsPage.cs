using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedPlatformsPage {
    public IEnumerable<PublishedPlatformsPageMergeModel> MergeModels { get; set; }
    public Dictionary<string, string> MetaTags { get; set; }
}