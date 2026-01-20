using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PlatformsPage {
    public PlatformsPage(Guid id,
                         PublishedFileKind kind,
                         string path,
                         IReadOnlyDictionary<string, string> metaTags,
                         IEnumerable<PublishedContentResult> mergeModels) {
        Id = id;
        Kind = kind;
        Path = path;
        MetaTags = metaTags;
        MergeModels = mergeModels;
    }

    public Guid Id { get; }
    public PublishedFileKind Kind { get; }
    public string Path { get; }
    public IReadOnlyDictionary<string, string> MetaTags { get; }
    public IEnumerable<PublishedContentResult> MergeModels { get; }
}