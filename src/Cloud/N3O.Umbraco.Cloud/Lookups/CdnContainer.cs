using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class CdnContainer : Lookup {
    public CdnContainer(string id, bool subfolders) : base(id) {
        Subfolders = subfolders;
    }
    
    public bool Subfolders { get; }
    public string Prefix => Id;
}

public class CdnContainers : StaticLookupsCollection<CdnContainer> {
    public static readonly CdnContainer Connect = new("connect", false);
    public static readonly CdnContainer Public = new("public", true);
}