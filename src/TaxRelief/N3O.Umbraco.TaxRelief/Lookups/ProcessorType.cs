using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.TaxRelief.Lookups;

public class ProcessorType : Lookup {
    public ProcessorType(string id) : base(id) { }
}

public class ProcessorTypes : StaticLookupsCollection<ProcessorType> {
    public static readonly ProcessorType Type1 = new("type1");
    public static readonly ProcessorType Type2 = new("type2");
}