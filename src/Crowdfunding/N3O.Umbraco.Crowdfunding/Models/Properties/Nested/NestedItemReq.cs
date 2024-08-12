using N3O.Umbraco.Attributes;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class NestedItemReq {
    [Name("Content Type Alias")]
    public string ContentTypeAlias { get; set; }
    
    [Name("Properties")]
    public IEnumerable<ContentPropertyReq> Properties { get; set; }
}