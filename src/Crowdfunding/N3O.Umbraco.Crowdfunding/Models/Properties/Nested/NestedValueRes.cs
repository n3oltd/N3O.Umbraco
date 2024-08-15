using N3O.Umbraco.Attributes;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class NestedValueRes {
    [Name("Items")]
    public IEnumerable<NestedItemRes> Items { get; set; }
}