using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class CdnRoot : Lookup {
    public CdnRoot(string id, CdnContainer container) : base(id) {
        Container = container;
    }

    public CdnContainer Container { get; }
    public string PathSegment => Container.Subfolders ? Id : null;
}

public class CdnRoots : StaticLookupsCollection<CdnRoot> {
    public static readonly CdnRoot Assets = new("assets", CdnContainers.Public);
    public static readonly CdnRoot BeneficiaryPhotos = new("beneficiaryPhotos", CdnContainers.Public);
    public static readonly CdnRoot Connect = new("connect", CdnContainers.Connect);
    public static readonly CdnRoot Downloads = new("downloads", CdnContainers.Public);
}