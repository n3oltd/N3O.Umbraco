using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PlatformsPage {
    public PlatformsPage(Guid id,
                         PublishedFileKind kind,
                         string path,
                         JObject content,
                         IReadOnlyDictionary<string, string> metaTags,
                         IEnumerable<PublishedContentResult> additionalModels) {
        Id = id;
        Kind = kind;
        Path = path;
        Content = content;
        MetaTags = metaTags;
        AdditionalModels = additionalModels;
    }

    public Guid Id { get; }
    public PublishedFileKind Kind { get; }
    public string Path { get; }
    public JObject Content { get; }
    public IReadOnlyDictionary<string, string> MetaTags { get; }
    public IEnumerable<PublishedContentResult> AdditionalModels { get; }
}