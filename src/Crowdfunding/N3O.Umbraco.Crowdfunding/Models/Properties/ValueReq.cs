using N3O.Umbraco.Crowdfunding.Lookups;

namespace N3O.Umbraco.Crowdfunding.Models;

public abstract class ValueReq {
    public abstract PropertyType Type { get; }
}