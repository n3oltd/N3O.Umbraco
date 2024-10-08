﻿using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class NestedValueReq : ValueReq {
    [Name("Items")]
    public IEnumerable<NestedItemReq> Items { get; set; }
    
    [JsonIgnore]
    public override PropertyType Type => PropertyTypes.Nested;
}