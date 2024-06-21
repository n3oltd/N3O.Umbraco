using N3O.Umbraco.Crowdfunding.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Crowdfunding.Models;

public class BooleanValueReq : ValueReq {
    public bool? Value { get; set; }
    
    [JsonIgnore]
    public override PropertyType Type => PropertyTypes.Boolean;
}