using N3O.Umbraco.Cloud.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PlatformsPage : Value {
    public PlatformsPage(PublishedFileKind kind, IReadOnlyDictionary<string, object> mergeModel) {
        Kind = kind;
        MergeModel = mergeModel;
    }

    public PublishedFileKind Kind { get; }
    public IReadOnlyDictionary<string, object> MergeModel { get; }
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return Kind;
        yield return MergeModel;
    }
}