using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.CrowdFunding.Lookups;

public class CropType : NamedLookup {
    public CropType(string id, string name) : base(id, name) { }
}

public class CropTypes : StaticLookupsCollection<CropType> {
    public static readonly CropType Circle = new("circle", "circle");
    public static readonly CropType Rectangle = new("rectangle", "rectangle");
}