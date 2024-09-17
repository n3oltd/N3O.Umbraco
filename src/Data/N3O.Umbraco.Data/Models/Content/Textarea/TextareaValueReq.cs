using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Data.Models;

public class TextareaValueReq : ValueReq {
    [Name("Value")]
    public string Value { get; set; }
    
    [JsonIgnore]
    public override PropertyType Type => PropertyTypes.Textarea;
}