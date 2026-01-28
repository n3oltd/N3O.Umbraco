using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PlatformsPage : Value {
    public PlatformsPage(Guid id,
                         PublishedFileKind kind,
                         string path,
                         Uri url,
                         JObject content,
                         IReadOnlyDictionary<string, string> metaTags,
                         IEnumerable<PublishedContentResult> additionalModels) {
        Id = id;
        Kind = kind;
        Path = path;
        Content = content;
        Url = url;
        MetaTags = metaTags;
        AdditionalModels = additionalModels;
    }

    public Guid Id { get; }
    public PublishedFileKind Kind { get; }
    public string Path { get; }
    public Uri Url { get; }
    public JObject Content { get; }
    public IReadOnlyDictionary<string, string> MetaTags { get; }
    public IEnumerable<PublishedContentResult> AdditionalModels { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Kind;
        yield return Path;
        yield return Url;
        yield return Content;
        yield return MetaTags;
        yield return AdditionalModels;
    }
}