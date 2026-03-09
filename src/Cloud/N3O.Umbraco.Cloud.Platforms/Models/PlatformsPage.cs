using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Content;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PlatformsPage : Value {
    public PlatformsPage(Guid id,
                         PublishedFileKind kind,
                         SpecialContent parent,
                         string path,
                         string title,
                         Uri url,
                         JObject content,
                         IReadOnlyDictionary<string, string> metaTags,
                         IEnumerable<PublishedContentResult> additionalModels) {
        Id = id;
        Kind = kind;
        Parent = parent;
        Path = path;
        Title = title;
        Content = content;
        Url = url;
        MetaTags = metaTags;
        AdditionalModels = additionalModels;
    }

    public Guid Id { get; }
    public PublishedFileKind Kind { get; }
    public SpecialContent Parent { get; }
    public string Path { get; }
    public string Title { get; }
    public Uri Url { get; }
    public JObject Content { get; }
    public IReadOnlyDictionary<string, string> MetaTags { get; }
    public IEnumerable<PublishedContentResult> AdditionalModels { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Kind;
        yield return Parent;
        yield return Path;
        yield return Title;
        yield return Url;
        yield return Content;
        yield return MetaTags;
        yield return AdditionalModels;
    }
}