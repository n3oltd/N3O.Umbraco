using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class BlockListValueReq : ValueReq {
    [Name("Items")]
    public IEnumerable<BlockListItemReq> Items { get; set; }
    
    [JsonIgnore]
    public override PropertyType Type => PropertyTypes.BlockList;
}