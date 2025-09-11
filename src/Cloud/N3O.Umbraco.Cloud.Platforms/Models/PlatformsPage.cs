using N3O.Umbraco.Cloud.Lookups;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PlatformsPage : Value {
    public PlatformsPage(Guid id,
                         string path,
                         PublishedFileKind kind,
                         IReadOnlyDictionary<string, object> mergeModel) {
        Id = id;
        Path = path;
        Kind = kind;
        MergeModel = mergeModel;
    }

    public Guid Id { get; }
    public string Path { get; }
    public PublishedFileKind Kind { get; }
    public IReadOnlyDictionary<string, object> MergeModel { get; }
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return Kind;
        yield return MergeModel;
    }
}