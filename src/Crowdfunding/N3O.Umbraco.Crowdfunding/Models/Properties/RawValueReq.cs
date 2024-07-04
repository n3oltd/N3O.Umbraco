using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Crowdfunding.Models;

public class RawValueReq : ValueReq {
    [Name("Value")]
    public string Value { get; set; }
    
    [JsonIgnore]
    public override PropertyType Type => PropertyTypes.Raw;
}