using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class NestedItemRes {
    public string ContentTypeAlias { get; set; }
    public IEnumerable<ContentPropertyValueRes> Properties { get; set; }
}