using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Content;

public class SpecialContent : NamedLookup {
    public SpecialContent(string id, string name, string urlSettingsPropertyAlias) : base(id, name) {
        UrlSettingsPropertyAlias = urlSettingsPropertyAlias;
    }
    
    public string UrlSettingsPropertyAlias { get; }
}

public interface ISpecialContents { }

public class SpecialContents : DistributedLookupsCollection<SpecialContent, ISpecialContents> { }
