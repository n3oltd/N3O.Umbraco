using N3O.Umbraco.Data.Lookups;

namespace N3O.Umbraco.Data.Models;

public abstract class ValueReq {
    public abstract PropertyType Type { get; }
}