using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Lookups;
using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class DateTimeValueReq : ValueReq {
    [Name("Value")]
    public DateTime? Value { get; set; }

    [JsonIgnore]
    public override PropertyType Type => PropertyTypes.DateTime;
}