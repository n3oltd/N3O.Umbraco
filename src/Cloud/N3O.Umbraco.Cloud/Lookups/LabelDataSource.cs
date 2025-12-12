using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public abstract class LabelDataSource<T> : LookupsDataSource<T> where T : Label {
    public LabelDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => $"{Scope.Name} Labels";
    public override string Description => $"Data source for {Scope.Name.ToLowerInvariant()} labels";
    public override string Icon => "icon-tag";
    
    protected override string GetIcon(T label) => "icon-tag";
    
    protected abstract TagScope Scope { get; }
}