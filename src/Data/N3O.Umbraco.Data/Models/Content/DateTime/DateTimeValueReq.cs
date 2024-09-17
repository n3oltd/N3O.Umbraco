using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Lookups;
using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.Data.Models;

public class DateTimeValueReq : ValueReq {
    [Name("Value")]
    public DateTime? Value { get; set; }

    [JsonIgnore]
    public override PropertyType Type => PropertyTypes.DateTime;
}