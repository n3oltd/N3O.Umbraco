using Ganss.Xss;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Data.Models;

public class RawValueReq : ValueReq {
    [Name("Value")]
    public string Value { get; set; }

    [JsonIgnore]
    public string SafeValue {
        get {
            var sanitizer = new HtmlSanitizer();

            return sanitizer.Sanitize(Value);
        }
    }
    
    [JsonIgnore]
    public override PropertyType Type => PropertyTypes.Raw;
}