using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Data.Lookups;

public class CropShape : NamedLookup {
    public CropShape(string id, string name) : base(id, name) { }
}

public class CropShapes : StaticLookupsCollection<CropShape> {
    public static readonly CropShape Circle = new("circle", "Circle");
    public static readonly CropShape Rectangle = new("rectangle", "Rectangle");
}